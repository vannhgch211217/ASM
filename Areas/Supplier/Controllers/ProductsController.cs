using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.Models;

namespace ASM.Areas.Controllers
{
    [Area("Supplier")]
    public class ProductsController : Controller
    {
        private readonly ASMContext _context;

        public ProductsController(ASMContext context)
        {
            _context = context;
        }

        // GET: Supplier/Products
        public async Task<IActionResult> Index()
        {
            var aSMContext = _context.Product.Include(p => p.Category).Include(p => p.ColorDetail).Include(p => p.Size).Include(p => p.Supplier);
            return View(await aSMContext.ToListAsync());
        }

        // GET: Supplier/Products/Details/5
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
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Supplier/Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID");
            ViewData["ColorDetailID"] = new SelectList(_context.ColorDetail, "ColorDetailID", "ColorDetailID");
            ViewData["SizeID"] = new SelectList(_context.Size, "SizeID", "SizeID");
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "SupplierID");
            return View();
        }

        // POST: Supplier/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,SupplierID,CategoryID,SizeID,ColorDetailID,ProductName,Price,Quantity,Description,Image")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["ColorDetailID"] = new SelectList(_context.ColorDetail, "ColorDetailID", "ColorDetailID", product.ColorDetailID);
            ViewData["SizeID"] = new SelectList(_context.Size, "SizeID", "SizeID", product.SizeID);
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "SupplierID", product.SupplierID);
            return View(product);
        }

        // GET: Supplier/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["ColorDetailID"] = new SelectList(_context.ColorDetail, "ColorDetailID", "ColorDetailID", product.ColorDetailID);
            ViewData["SizeID"] = new SelectList(_context.Size, "SizeID", "SizeID", product.SizeID);
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "SupplierID", product.SupplierID);
            return View(product);
        }

        // POST: Supplier/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,SupplierID,CategoryID,SizeID,ColorDetailID,ProductName,Price,Quantity,Description,Image")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["ColorDetailID"] = new SelectList(_context.ColorDetail, "ColorDetailID", "ColorDetailID", product.ColorDetailID);
            ViewData["SizeID"] = new SelectList(_context.Size, "SizeID", "SizeID", product.SizeID);
            ViewData["SupplierID"] = new SelectList(_context.Supplier, "SupplierID", "SupplierID", product.SupplierID);
            return View(product);
        }

        // GET: Supplier/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.ColorDetail)
                .Include(p => p.Size)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Supplier/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'ASMContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
