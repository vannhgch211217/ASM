using ASM.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            string userEmail = HttpContext.Session.GetString("UserEmail");

            if (userEmail == null)
            {
                return Redirect("/login");
            }

            var orderDetails = _context.OrderDetail.Include(od => od.Product);
            var groupedOds = orderDetails.GroupBy(a => a.Product.UserID);

            float maxSum = 0;
            float maxSum2 = 0;
            
            int maxSumSpupplierId = -1;
            foreach (var odGroup in groupedOds)
            {
                float tempSum = odGroup.Sum(a => a.Quantity * a.Product.Price);
                if (tempSum > maxSum)
                {
                    maxSum = tempSum;
                    maxSumSpupplierId = odGroup.FirstOrDefault().Product.UserID;
                }
            }

            ViewBag.Count1 = maxSum; 
            ViewBag.MaxSumSpupplierId1 = maxSumSpupplierId;

            foreach (var odGroup in groupedOds)
            {
                float tempSum = odGroup.Sum(a => a.Quantity * a.Product.Price);
                if (tempSum > maxSum2&&tempSum<maxSum)
                {
                    maxSum = tempSum;
                    maxSumSpupplierId = odGroup.FirstOrDefault().Product.UserID;
                }
            }
            ViewBag.Count2 = maxSum;
            ViewBag.MaxSumSpupplierId2 = maxSumSpupplierId;

            var aSMContext = _context.OrderDetail.Include(o => o.Order).Include(o => o.Product);

            return View(await aSMContext.ToListAsync());

        }
    }
}
