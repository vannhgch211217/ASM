using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ASM.Controllers
{
    public class CartController : Controller
    {

        private readonly ASMContext _context;

        public CartController(ASMContext context)
        {
            _context = context;
        }

        // GET: /<controller>/
        [HttpGet("/cart")]
        public IActionResult Index()
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.User.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return Redirect("/login");
            }

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

            return Redirect("/cart");
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

            return Redirect("/cart");
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

            return Redirect("/cart");
        }

        [HttpGet("/checkout")]
        public IActionResult Checkout()
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.User.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return Redirect("/login");
            }

            return View("Checkout");
        }

        [HttpPost("/checkout/information")]
        public IActionResult updateAddressAndPhone(string phoneNumber, string address)
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
        public IActionResult ConfirmOrder(int orderId)
        {

            try
            {
                string userEmail = HttpContext.Session.GetString("UserEmail");
                var user = _context.User.FirstOrDefault(u => u.Email == userEmail);

                if (user == null)
                {
                    return Redirect("/login");
                }

                List<Order> orders = _context.Order
                    .Where(o => o.UserID == user.UserID)
                    .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToList();

                List<OrderDetail> orderDetails = _context.OrderDetail
                    .Where(od => od.OrderID == orderId)
                    .Include(od => od.Product)
                    .ToList();

                var orderInfo = Tuple.Create(orders, orderDetails, orderId);

                return View("OrderInfo", orderInfo);
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred: " + ex.Message;
                ViewData["ErrorMessage"] = errorMessage;
                return View("Index"); // You may want to choose a more appropriate error view
            }
        }
    }
}

