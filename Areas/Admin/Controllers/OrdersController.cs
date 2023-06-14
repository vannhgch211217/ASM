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
    public class OrdersController : Controller
    {
        private readonly ASMContext _context;

        public OrdersController(ASMContext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        [HttpGet("/Admin/Orders")]
        public async Task<IActionResult> Index(string searchString)
        {
            var order = from m in _context.Order select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                order = order.Where(s => s.User.Name.Contains(searchString));
            }

            return View(await order.ToListAsync());
        }
    }
}
