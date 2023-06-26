using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Certify.Controllers
{
    [Authorize]
    public class MySignaturesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
