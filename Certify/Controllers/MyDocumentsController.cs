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
        readonly CertifyDbContext _context;
        readonly IWebHostEnvironment _appEnvironment;
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

        public async Task<IActionResult> CreateAsync()
        {
            await SelectUserAsync();
            return View("Create");
        }
        private async Task SelectUserAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            string userId = user.Id;

            var userList = _context.Users
                .Where(u => u.Id != userId)
                .ToList();

            ViewBag.UserList = new SelectList(userList, nameof(Models.User.Id), nameof(Models.User.Email));
        }


        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile, DocumentAndSignatureCombined dasc)
        {
            if (uploadedFile != null)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                string currentUserId = user.Id;
                string path = "/Documents/" + uploadedFile.FileName;

                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                dasc.DocumentFC.FileURL = path;
                dasc.DocumentFC.UserId = currentUserId;
                dasc.DocumentFC.UploadedDate = DateTime.Now;

                _context.Documents.Add(dasc.DocumentFC);
                _context.SaveChanges();

                var lastDocument = _context.Documents.OrderByDescending(d => d.Id).First();

                var signature = new Signature
                {
                    IsSigned = null,
                    DocumentId = lastDocument.Id,
                    UserId = dasc.SignatureFC.UserId
                };

                _context.Signatures.Add(signature);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}