using Microsoft.AspNetCore.Mvc;

namespace ASM.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
