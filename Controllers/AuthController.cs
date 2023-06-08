//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using ASM.Models;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace ASM.Controllers
//{
//    public class AuthController : Controller
//    {
//        private UserController _userController;
//        public AuthController(UserController userController)
//        {
//            _userController = userController;
//        }

//        [HttpGet("/login")]
//        public IActionResult Index()
//        {
//            return View("login");
//        }

//        //[HttpPost("/login")]
//        //public async <IActionResult> Regsiter(User user)
//        //{
//        //    return _userController.Createsda(user);
//        //}

//        public Task<IActionResult> Register(User user)
//        {
//            return _userController.Createsda(user);
//        }

//    }
//}

