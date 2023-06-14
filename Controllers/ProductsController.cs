﻿using System;
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
        public async Task<IActionResult> Products()
        {
            var aSMContext = _context.Product.Include(p => p.Category).Include(p => p.ColorDetail).Include(p => p.Size).Include(p => p.User);
            return View(await aSMContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.ColorDetail)
                .Include(p => p.Size)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}