using Certify.Data;
using Certify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Certify.Controllers
{
    public class MyDocumentsController : Controller
    {
        CertifyDbContext _context;
        IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;

        public MyDocumentsController(CertifyDbContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        public IActionResult Create()
        {

            var document = _context.Documents.ToList();

            return View("Create", document);
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                string userId = user.Id;
                string path = "/Documents/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                Document file = new Document
                {
                    Title = uploadedFile.FileName,
                    FileURL = path,
                    UserId = userId,
                    UploadedDate = DateTime.Now,
                    ShortDescription = "defefefef",
                };
                _context.Documents.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Create");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
