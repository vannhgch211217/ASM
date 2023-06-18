using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASM.Data;
using ASM.Models;

namespace ASM.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorDetailsController : Controller
    {
        private readonly ASMContext _context;

        public ColorDetailsController(ASMContext context)
        {
            _context = context;
        }

        // GET: Admin/ColorDetails
        [HttpGet("/Admin/ColorDetails")]
        public async Task<IActionResult> Index(string searchString)
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");

            if (userEmail == null)
            {
                return Redirect("/login");
            }

            var color = from m in _context.ColorDetail select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                color = color.Where(s => s.Color.Contains(searchString));
            }

            return View(await color.ToListAsync());
        }


        // GET: Admin/ColorDetails/Create
        [Route("Admin/ColorDetails/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ColorDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Admin/ColorDetails/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColorDetailID,Color")] ColorDetail colorDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colorDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colorDetail);
        }

        // GET: Admin/ColorDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ColorDetail == null)
            {
                return NotFound();
            }

            var colorDetail = await _context.ColorDetail.FindAsync(id);
            if (colorDetail == null)
            {
                return NotFound();
            }
            return View(colorDetail);
        }

        // POST: Admin/ColorDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Admin/ColorDetails/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ColorDetailID,Color")] ColorDetail colorDetail)
        {
            if (id != colorDetail.ColorDetailID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colorDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorDetailExists(colorDetail.ColorDetailID))
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
            return View(colorDetail);
        }

        // GET: Admin/ColorDetails/Delete/5
        [HttpGet("Admin/ColorDetails/Delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ColorDetail == null)
            {
                return NotFound();
            }

            var colorDetail = await _context.ColorDetail
                .FirstOrDefaultAsync(m => m.ColorDetailID == id);
            if (colorDetail == null)
            {
                return NotFound();
            }

            return View(colorDetail);
        }

        // POST: Admin/ColorDetails/Delete/5
        [HttpPost("Admin/ColorDetails/Delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ColorDetail == null)
            {
                return Problem("Entity set 'ASMContext.ColorDetail'  is null.");
            }
            var colorDetail = await _context.ColorDetail.FindAsync(id);
            if (colorDetail != null)
            {
                _context.ColorDetail.Remove(colorDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColorDetailExists(int id)
        {
          return (_context.ColorDetail?.Any(e => e.ColorDetailID == id)).GetValueOrDefault();
        }
    }
}
