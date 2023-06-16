using ASM.Data;
using ASM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASM.Controllers
{
    public class AuthController : Controller
    {
        public const string SessionKeyName = "Username";
        public const string SessionKeyEmail = "UserEmail";
        public const string SessionKeyPhone = "UserPhone";
        public const string SessionAddress = "UserAddress";

        private readonly ASMContext _context;

        public AuthController(ASMContext context)
        {
            _context = context;
        }

        [HttpGet("/login")]
        public IActionResult Index()
        {
            Console.WriteLine("Hello");
            return View("Index");
        }

        [HttpGet("/register")]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost("/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(String email, String password)
        {
            try
            {
                var user = _context.User.FirstOrDefault(u => u.Email == email);

                if (user == null)
                {
                    string script = "<script>alert('User not found');window.location='/login';</script>";
                    return Content(script, "text/html");
                }
                else
                {
                    if (user.Email == email && BCrypt.Net.BCrypt.Verify(password, user.Password))
                    {
                        HttpContext.Session.SetString(SessionKeyName, user.Name);
                        HttpContext.Session.SetString(SessionKeyEmail, user.Email);
                        HttpContext.Session.SetString(SessionKeyPhone, user.PhoneNumber);
                        HttpContext.Session.SetString(SessionAddress, user.Address);
                        if (user.Role == "Admin")
                        {
                            return Redirect("/Admin/Sizes");
                        }
                        else if (user.Role == "Supplier")
                        {
                            return Redirect("/Suppiler/Product");
                        }
                        return Redirect("/");
                    }
                    else
                    {
                        string script = "<script>alert('Invalid username or password');window.location='/login';</script>";
                        return Content(script, "text/html");
                    }

                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred during login";
                return View("Error");
            }
        }

        [HttpPost("/register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("UserID,Email,Password,Role,Name,PhoneNumber,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                var userInDb = _context.User.FirstOrDefault(u => u.Email == user.Email);
                if (userInDb != null)
                {
                    string script = "<script>alert('User already exist');window.location='/register';</script>";
                    return Content(script, "text/html");
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Redirect("/login");
            }
            return View(user);
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove(SessionKeyName);
            HttpContext.Session.Remove(SessionKeyEmail);
            HttpContext.Session.Remove(SessionKeyPhone);
            HttpContext.Session.Remove(SessionAddress);
            return Redirect("/");
        }
    }
}
