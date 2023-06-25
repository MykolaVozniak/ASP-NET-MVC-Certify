using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using Certify.Models;
using Certify.Data;
using Microsoft.AspNetCore.Identity;

namespace Certify.Controllers
{
    public class MyDocumentsController : Controller
    {
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
}
