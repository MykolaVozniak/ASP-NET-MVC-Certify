using Microsoft.AspNetCore.Mvc;

namespace Certify.Controllers
{
    public class NotificationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
