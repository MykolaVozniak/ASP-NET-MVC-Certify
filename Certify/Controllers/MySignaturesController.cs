using Microsoft.AspNetCore.Mvc;

namespace Certify.Controllers
{
    public class MySignaturesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
