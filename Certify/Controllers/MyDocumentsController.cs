using Microsoft.AspNetCore.Mvc;

namespace Practice_Project.Controllers
{
    public class MyDocumentsController : Controller
    {
        public IActionResult Index()
        {
            int first = 1;
            int second = 2;
            int result = (first + second)*2;
            return View(result);
        }
    }
}
