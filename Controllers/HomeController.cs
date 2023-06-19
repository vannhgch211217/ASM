using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

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

        [HttpGet("/about")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet("/contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost("/add-to-cart")]
        public IActionResult AddToCart(int productId, string productName, float productPrice, string Image, [FromForm] int colorID, [FromForm] int sizeID, int quantity, string groupId)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != null)
            {
                var productInDb = _context.Product.FirstOrDefault(p => p.GroupId == groupId && p.ColorDetailID == colorID);
                User userInDb = _context.User.FirstOrDefault(u => u.Email == userEmail);

                if(productInDb == null)
                {
                    Console.WriteLine("damn");
                }

                List<Product> cart = HttpContext.Session.getJson<List<Product>>(userEmail + "Cart") ?? new List<Product>();

                if (quantity > productInDb.Quantity)
                {
                    string script = "<script>alert('Product out of stock');window.location='/Products';</script>";
                    return Content(script, "text/html");
                }
                else
                {
                    Product existingProduct = cart.FirstOrDefault(p => p.ProductId == productId && p.ColorDetailID == colorID && p.SizeID == sizeID);

                    if (existingProduct != null)
                    {
                        existingProduct.Quantity += quantity;
                    }
                    else
                    {
                        Product product = new Product
                        {
                            ProductId = productId,
                            ProductName = productName,
                            Price = productPrice,
                            Image = "/images/ProductImage/" + productInDb.Image,
                            ColorDetailID = colorID,
                            SizeID = sizeID,
                            Quantity = quantity,
                            GroupId = groupId
                        };

                        cart.Add(product);
                    }

                    productInDb.Quantity -= quantity;
                    _context.SaveChanges();
                }

                HttpContext.Session.setJson(userEmail + "Cart", cart);

                float grandTotal = cart.Sum(p => p.Price * p.Quantity);
                HttpContext.Session.setJson(userEmail + "GrandTotal", grandTotal);

                return Redirect("Cart");
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
    }
}