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
    public class OrderDetailsController : Controller
    {
        private readonly ASMContext _context;

        public OrderDetailsController(ASMContext context)
        {
            _context = context;
        }

        // GET: Admin/OrderDetails
        [HttpGet("/Admin/OrderDetails")]
        public async Task<IActionResult> Index()
        {
            var accounts = from m in _context.OrderDetail select m;
            var groupedAccounts = accounts.GroupBy(a => a.Product.UserID);

            accounts = accounts.Where(s => s.Product.UserID == 2);
            ViewBag.Count = accounts.Sum(a => a.Quantity * a.Product.Price);

            var aSMContext = _context.OrderDetail.Include(o => o.Order).Include(o => o.Product);
            return View(await aSMContext.ToListAsync());
        }
    }
}
