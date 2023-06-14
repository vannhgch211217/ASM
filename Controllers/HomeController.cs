using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;

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

        [HttpPost("/")]
        public IActionResult AddToCart(int productId, string productName, float productPrice)
        {
            // Create a new product instance
            Product product = new Product
            {
                ProductId = productId,
                ProductName = productName,
                Price = productPrice,
                Quantity = 1
            };

            // Check if the cart exists in the session
            List<Product> cart = HttpContext.Session.getJson<List<Product>>("Cart") ?? new List<Product>();

            // Check if the product already exists in the cart
            Product existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);
            if (existingProduct != null)
            {
                // If the product already exists, update the quantity
                existingProduct.Quantity++;
            }
            else
            {
                // If the product doesn't exist, add it to the cart
                cart.Add(product);
            }

            // Store the updated cart back in the session
            HttpContext.Session.setJson("Cart", cart);

            // Calculate the grand total
            float grandTotal = cart.Sum(p => p.Price * p.Quantity);
            HttpContext.Session.setJson("GrandTotal", grandTotal);

            // Redirect to the cart view
            return RedirectToAction("Cart");
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
            List<Product> cart = HttpContext.Session.getJson<List<Product>>("Cart");
            Product existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);
            if (existingProduct != null)
            {
                existingProduct.Quantity++;
            }

            HttpContext.Session.setJson("Cart", cart);

            float grandTotal = cart.Sum(p => p.Price * p.Quantity);
            HttpContext.Session.setJson("GrandTotal", grandTotal);

            return RedirectToAction("Cart");
        }

        [HttpPost("/Cart/DecreaseQuantity")]
        public IActionResult DecreaseQuantity(int productId)
        {
            List<Product> cart = HttpContext.Session.getJson<List<Product>>("Cart");
            Product existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);
            if (existingProduct != null && existingProduct.Quantity > 1)
            {
                existingProduct.Quantity--;
            }

            HttpContext.Session.setJson("Cart", cart);

            float grandTotal = cart.Sum(p => p.Price * p.Quantity);
            HttpContext.Session.setJson("GrandTotal", grandTotal);

            return RedirectToAction("Cart");
        }

    }
}