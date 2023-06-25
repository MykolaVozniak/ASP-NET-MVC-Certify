using Certify.Data;
using Certify.Models;
<<<<<<< HEAD
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
=======
using Certify.Data;
>>>>>>> origin/Halushka
using Microsoft.AspNetCore.Identity;

namespace Certify.Controllers
{
    public class MyDocumentsController : Controller
    {
<<<<<<< HEAD
        CertifyDbContext _context;
        IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;

        public MyDocumentsController(CertifyDbContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        public IActionResult AddIndex()
        {
            return View(_context.Documents.ToList());
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
                Document file = new Document {
                    Title = uploadedFile.FileName,
                    FileURL = path,
                    UserId = userId,
                    UploadedDate = DateTime.Now,
                    ShortDescription = "defefefef",
                };
                _context.Documents.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("AddIndex");
        }
    }
=======
		private readonly CertifyDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;

        public MyDocumentsController(CertifyDbContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
		{
            _context = context;
            _userManager = userManager;
            _appEnvironment = appEnvironment;
        }

		public IActionResult Index()
        {
            var document = _context.Documents.ToList();

               return View("Index", document);
        }


        [HttpPost]

        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Documents/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                var user = await _userManager.GetUserAsync(HttpContext.User);
                string userId = user.Id;

                Document file = new Document { Title = uploadedFile.FileName, FileURL = path, UploadedDate = DateTime.Now, UserId = userId };

                _context.Documents.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }


		// POST: /Laptops/Add
		[HttpPost]
		public IActionResult Create(Document document)
		{
           


            TempData["alertMessage"] = "Product was successfully created!";

			return RedirectToAction(nameof(Index));
		}
	}
>>>>>>> origin/Halushka
}
