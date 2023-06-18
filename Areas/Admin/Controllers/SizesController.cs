using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.Models;
using System.Drawing;
using System.Security.Policy;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizesController : Controller
    {
        private readonly ASMContext _context;

        public SizesController(ASMContext context)
        {
            _context = context;
        }

        // GET: Admin/Sizes
        [HttpGet("/Admin/Sizes")]
        public async Task<IActionResult> Index(string searchString)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");

            if (userEmail == null)
            {
                return Redirect("/login");
            }

            var sz = from m in _context.Size select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                int number = int.Parse(searchString);
                sz = sz.Where(s => s.SizeNumber.Equals(number));
            }
            return View(await sz.ToListAsync());
        }

        // GET: Admin/Sizes/Create
        [Route("Admin/Sizes/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Admin/Sizes/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SizeID,SizeNumber")] Models.Size size)
        {
            
                _context.Add(size);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
        }

        // GET: Admin/Sizes/Edit/5
        [HttpGet("Admin/Sizes/Edit/{id}")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Size == null)
            {
                return NotFound();
            }

            var size = await _context.Size.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        // POST: Admin/Sizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Admin/Sizes/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SizeID,SizeNumber")] Models.Size size)
        {
            if (id != size.SizeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.SizeID))
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
            return View(size);
        }

        // GET: Admin/Sizes/Delete/5\
        [HttpGet("Admin/Sizes/Delete/{id}")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Size == null)
            {
                return NotFound();
            }

            var size = await _context.Size
                .FirstOrDefaultAsync(m => m.SizeID == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // POST: Admin/Sizes/Delete/5
        [HttpPost("Admin/Sizes/Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = _context.Product.Where(p => p.SizeID == id).ToList();
            var size = _context.Size.Find(id);

            // Remove the association between products and the size
            foreach (var product in products)
            {
                product.SizeID = null; // Remove the association with the size
            }
            _context.Size.Remove(size);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SizeExists(int id)
        {
          return (_context.Size?.Any(e => e.SizeID == id)).GetValueOrDefault();
        }
    }
}
