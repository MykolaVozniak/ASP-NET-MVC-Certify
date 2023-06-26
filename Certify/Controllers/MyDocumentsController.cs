using Certify.Data;
using Certify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;

namespace Certify.Controllers
{
    [Authorize]
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

        public IActionResult Index()
        {
            var documents = _context.Documents.ToList();

            return View("Index", documents);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, Document document)
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

                document.FileURL = path;
                document.UserId = userId;
                document.UploadedDate = DateTime.Now;

                _context.Documents.Add(document);
                _context.SaveChanges();

                var lastDocument = _context.Documents.OrderByDescending(d => d.Id).First();

                var signature = new Signature
                {
                    IsSigned = null,
                    DocumentId = lastDocument.Id,
                    UserId = "58907c80-5262-4245-9b5c-eb259f2b8e81"
                };

                _context.Signatures.Add(signature);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
} 
