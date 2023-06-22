using Microsoft.AspNetCore.Mvc;

namespace Practice_Project.Controllers
{
    public class MyDocumentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
