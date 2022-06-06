using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pump.Models;

namespace pump.Controllers
{
    public class HomeController : Controller
    {
        private string company_id = "75e87d7c-fa35-4de6-ad70-149d67bec9f5";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        async public Task<IActionResult> Index()
        {
            var productContext = new ProductContext();
            ViewBag.Products = await productContext.GetByCompanyId(company_id);
            return View();
        }

        public IActionResult Form()
        {
            return View();
        }

        [HttpPost]
        public ActionResult New(string name, string unit, string price, string img)
        {
            price = price.Replace(",", ".");

            ProductModel productModel = new ProductModel()
            {
                Name = name,
                Price = double.Parse(price),
                Unit = unit,
                Img = img,
                Company_Id = company_id,
                Timestamp = new System.DateTime()
            };
            var productContext = new ProductContext();
            productContext.SetProduct(productModel);

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
