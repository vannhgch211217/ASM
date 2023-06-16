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
    public class UserController : Controller
    {
        //public const string SessionKeyName = "Username";
        public const string SessionKeyEmail = "UserEmail";
        //public const string SessionKeyPhone = "UserPhone";
        //public const string SessionAddress = "UserAddress";

        private readonly ASMContext _context;

        public UserController(ASMContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'ASMContext.User'  is null.");
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.UserID == id)).GetValueOrDefault();
        }

        public IActionResult Error()
        {
            string alertMessage = TempData["AlertMessage"] as string;
            ViewBag.AlertMessage = alertMessage;
            return View();
        }

        [HttpGet("/profile")]
        public IActionResult profile()
        {
            string userEmail = HttpContext.Session.GetString("UserEmail");
            var user = _context.User.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return Redirect("/login");
            }
            return View("Profile", user);
        }

        [HttpPost("/profile")]
        public async Task<IActionResult> updateProfile(User updatedUser)
        {
            String userEmail = HttpContext.Session.GetString(SessionKeyEmail);
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Address = updatedUser.Address;
            user.PhoneNumber = updatedUser.PhoneNumber;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(profile));
        }


    }
}
