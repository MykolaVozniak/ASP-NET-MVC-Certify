using Microsoft.AspNetCore.Mvc;

namespace Practice_Project.Controllers
{
    public class NotificationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
