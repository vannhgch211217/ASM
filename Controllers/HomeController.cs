using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Internal;

namespace ASM.Controllers
{
    public class HomeController : Controller
    {
        private readonly ASMContext _context;
        public const string SessionProductName = "ProductName";

        public HomeController(ASMContext context)
        {
            _context = context;
        }

        [HttpGet("/")]
        public async Task<IActionResult> Index()
        {
            List<Product> cart = HttpContext.Session.getJson<List<Product>>("Cart");
            return _context.User != null ?
                          View(await _context.Product.ToListAsync()) :
                          Problem("Entity set 'ASMContext.User'  is null.");
        }

        [HttpPost("/add-to-cart")]
        public IActionResult AddToCart(int productId, string productName, float productPrice, string Image)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            if(userEmail != null)
            {
                User userInDb = _context.User.FirstOrDefault(u => u.Email == userEmail);
                Console.WriteLine(userInDb.Email);

                List<Product> cart = HttpContext.Session.getJson<List<Product>>(userEmail + "Cart") ?? new List<Product>();

                Product product = new Product
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = productPrice,
                    Image = Image,
                    Quantity = 1,
                    UserID = userInDb.UserID
                };

                // Check if the cart exists in the session
                Product existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);
                if (existingProduct != null)
                {
                    existingProduct.Quantity++;
                }
                else
                {
                    // If the product doesn't exist, add it to the cart
                    cart.Add(product);
                }

                // Store the updated cart back in the session
                HttpContext.Session.setJson(userEmail + "Cart", cart);

                // Calculate the grand total
                float grandTotal = cart.Sum(p => p.Price * p.Quantity);
                HttpContext.Session.setJson(userEmail + "GrandTotal", grandTotal);

                // Redirect to the cart view
                return RedirectToAction("Cart");
            }
            return Redirect("/login");
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/cart")]
        public IActionResult Cart()
        {
            float grandTotal = HttpContext.Session.getJson<float>("GrandTotal");
            ViewBag.GrandTotal = grandTotal;
            List<Product> cart = HttpContext.Session.getJson<List<Product>>("Cart");
            return View(cart);
        }

        [HttpPost("/Cart/IncreaseQuantity")]
        public IActionResult IncreaseQuantity(int productId)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            List<Product> cart = HttpContext.Session.getJson<List<Product>>(userEmail + "Cart") ?? new List<Product>();

            Product existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);
            if (existingProduct != null)
            {
                existingProduct.Quantity++;
            }

            HttpContext.Session.setJson(userEmail + "Cart", cart);

            float grandTotal = cart.Sum(p => p.Price * p.Quantity);
            HttpContext.Session.setJson(userEmail + "GrandTotal", grandTotal);

            return RedirectToAction("Cart");
        }

        [HttpPost("/Cart/DecreaseQuantity")]
        public IActionResult DecreaseQuantity(int productId)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            List<Product> cart = HttpContext.Session.getJson<List<Product>>(userEmail + "Cart") ?? new List<Product>();

            Product existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);
            if (existingProduct != null)
            {
                existingProduct.Quantity--;
            }

            HttpContext.Session.setJson(userEmail + "Cart", cart);

            float grandTotal = cart.Sum(p => p.Price * p.Quantity);
            HttpContext.Session.setJson(userEmail + "GrandTotal", grandTotal);

            return RedirectToAction("Cart");
        }

        [HttpPost("/Cart/RemoveFromCart")]
        public IActionResult RemoveFromCart(int productId)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            List<Product> cart = HttpContext.Session.getJson<List<Product>>(userEmail + "Cart") ?? new List<Product>();

            Product productToRemove = cart.FirstOrDefault(p => p.ProductId == productId);
            if (productToRemove != null)
            {
                cart.Remove(productToRemove);
            }

            HttpContext.Session.setJson(userEmail + "Cart", cart);

            float grandTotal = cart.Sum(p => p.Price * p.Quantity);
            HttpContext.Session.setJson(userEmail + "GrandTotal", grandTotal);

            return RedirectToAction("Cart");
        }

        [HttpGet("/checkout")]
        public IActionResult Checkout()
        {
            return View("Checkout");
        }

        [HttpPost("/checkout/information")]
        public IActionResult UpdateProfile(string phoneNumber, string address)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            User userInDb = _context.User.FirstOrDefault(u => u.Email == userEmail);

            if (userInDb != null)
            {
                // Update the user's information
                userInDb.PhoneNumber = phoneNumber;
                userInDb.Address = address;

                // Save the changes to the database
                _context.SaveChanges();

                // Update the user's information in the session
                HttpContext.Session.SetString("UserPhone", phoneNumber);
                HttpContext.Session.SetString("UserAddress", address);
            }

            // Redirect to a suitable page (e.g., profile page)
            return Redirect("/checkout");
        }

        [HttpPost("/checkout")]
        public IActionResult SubmitOrder()
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            User userInDb = _context.User.FirstOrDefault(u => u.Email == userEmail);

            // Retrieve the cart from the session or database
            List<Product> cart = HttpContext.Session.getJson<List<Product>>(userEmail + "Cart");
            float grandTotal = HttpContext.Session.getJson<float>(userEmail + "GrandTotal");

            if (userInDb != null && cart != null)
            {
                // Create a new order
                Order order = new Order
                {
                    UserID = userInDb.UserID,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    TotalPrice = grandTotal
                };

                // Save the order to the database
                _context.Order.Add(order);
                _context.SaveChanges();

                // Create order details for each product in the cart
                foreach (Product product in cart)
                {
                    OrderDetail orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = product.ProductId,
                        Quantity = product.Quantity,
                        UnitPrice = product.Price
                    };

                    // Save the order detail to the database
                    _context.OrderDetail.Add(orderDetail);
                }

                // Clear the cart
                cart.Clear();
                HttpContext.Session.setJson(userEmail + "Cart", cart);
                HttpContext.Session.Remove(userEmail + "GrandTotal");

                // Save the changes to the database
                _context.SaveChanges();
            }

            // Redirect to a suitable page (e.g., order confirmation page)
            return Redirect("/orderInfo");
        }

        [HttpGet("/orderInfo")]
        public IActionResult ConfirmOrder()
        {
            try
            {
                string userEmail = HttpContext.Session.GetString("UserEmail");
                var user = _context.User.FirstOrDefault(u => u.Email == userEmail);
                List<Order> orders = _context.Order
                .Where(o => o.UserID == user.UserID)
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .ToList();
                //List<Order> orders = _context.Order
                //.Where(o => o.UserID == user.UserID)
                //.Include(o => o.OrderDetails.Where(od => od.OrderID == o.OrderID))
                //    .ThenInclude(od => od.Product)
                //.ToList();
                for (int i = 0; i < orders.Count; i++)
                {
                    Console.WriteLine(orders[i].OrderDetails.FirstOrDefault().Product.ProductName);
                }
                return View("OrderInfo", orders);
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred: " + ex.Message;
                ViewData["ErrorMessage"] = errorMessage;
                return View("Privacy");
            }
        }
    }
}