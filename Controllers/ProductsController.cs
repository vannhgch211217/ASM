using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.Models;

namespace ASM.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ASMContext _context;

        public ProductsController(ASMContext context)
        {
            _context = context;
        }

        // GET: Products
        [HttpGet("Products")]
        public async Task<IActionResult> Products(string keyword)
        {
            IQueryable<Product> products = _context.Product;

            if (!String.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.ProductName.Contains(keyword) || p.Category.Name.Contains(keyword));
            }

            var distinctProducts = products.GroupBy(p => p.GroupId)
                                   .Select(g => g.First());

            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "Name");

            return View(await distinctProducts.ToListAsync());
        }

        // GET: Products/Details/5
        [HttpGet("Products/Details/{id}")]
        public async Task<IActionResult> Details(int? id, string groupId)
        {
            Console.WriteLine(groupId);
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            List<Product> products = await _context.Product
            .Where(p => p.GroupId == groupId)
            .Include(p => p.ColorDetail)
            .Include(p => p.Category)
            .Include(p => p.Size)
            .ToListAsync();

            
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }
    }
}
