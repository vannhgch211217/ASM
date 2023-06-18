﻿using ASM.Data;
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

        [HttpPost("/add-to-cart")]
        public IActionResult AddToCart(int productId, string productName, float productPrice, string Image)
        {
            Console.WriteLine(Image);
            string userEmail = HttpContext.Session.GetString("UserEmail");
            if (userEmail != null)
            {
                User userInDb = _context.User.FirstOrDefault(u => u.Email == userEmail);

                List<Product> cart = HttpContext.Session.getJson<List<Product>>(userEmail + "Cart") ?? new List<Product>();

                Product product = new Product
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = productPrice,
                    Image = Image,
                    Quantity = 1
                };

                Product existingProduct = cart.FirstOrDefault(p => p.ProductId == productId);
                if (existingProduct != null)
                {
                    existingProduct.Quantity++;
                }
                else
                {
                    cart.Add(product);
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