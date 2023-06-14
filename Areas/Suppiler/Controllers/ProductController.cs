using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ASM.Areas.Suppiler.Controllers
{
    [Area("Suppiler")]
    public class ProductController : Controller
    {
        private readonly ASMContext _context;

        public ProductController(ASMContext context)
        {
            _context = context;
        }

        // GET: Suppiler/Product
        [HttpGet("/Suppiler/Product")]
        public async Task<IActionResult> Index()
        {
            var aSMContext = _context.Product.Include(p => p.Category).Include(p => p.ColorDetail).Include(p => p.Size).Include(p => p.User);
            return View(await aSMContext.ToListAsync());
        }

        // GET: Suppiler/Product/Details/5
        [HttpGet("/Suppiler/Product/Details/{id}")]
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

        // GET: Suppiler/Product/Create
        [HttpGet("/Suppiler/Product/Create")]
        public IActionResult CreateProduct()
        {
            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID");
            ViewData["ColorDetailID"] = new SelectList(_context.ColorDetail, "ColorDetailID", "ColorDetailID");
            ViewData["SizeID"] = new SelectList(_context.Size, "SizeID", "SizeID");
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID");
            Console.Write("Hello");
            return View("Create");
        }

        // POST: Suppiler/Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("/Suppiler/Product/Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,UserID,CategoryID,SizeID,ColorDetailID,ProductName,Price,Quantity,Description")] Product product, IFormFile imageFile)
        {
            Debug.WriteLine(imageFile.FileName);

            var imgPath = Path.Combine("wwwroot/images/ProductImage", imageFile.FileName);

            using (var stream = System.IO.File.Create(imgPath))
            {
                await imageFile.CopyToAsync(stream);
                product.Image = imageFile.FileName;
            }
            try
            {
                _context.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return View("Error", ex.Message);
            }
        }

        // GET: Suppiler/Product/Edit/5
        [HttpGet("Suppiler/Product/Edit/{id}")]
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
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", product.UserID);
            return View(product);
        }


        // POST: Suppiler/Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Suppiler/Product/Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,UserID,CategoryID,SizeID,ColorDetailID,ProductName,Price,Quantity,Description,Image")] Models.Product product)
        {

            if (id != product.ProductId)
            {
                return NotFound();
            }

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

            ViewData["CategoryID"] = new SelectList(_context.Category, "CategoryID", "CategoryID", product.CategoryID);
            ViewData["ColorDetailID"] = new SelectList(_context.ColorDetail, "ColorDetailID", "ColorDetailID", product.ColorDetailID);
            ViewData["SizeID"] = new SelectList(_context.Size, "SizeID", "SizeID", product.SizeID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", product.UserID);

            return RedirectToAction(nameof(Index));
        }

        // GET: Suppiler/Product/Delete/5
        [HttpGet("Suppiler/Product/Delete/{id}")]
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
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Suppiler/Product/Delete/5
        [HttpPost("Suppiler/Product/Delete/{id}"), ActionName("Delete")]
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
