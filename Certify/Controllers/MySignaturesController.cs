using Certify.Data;
using Certify.Models;
using Certify.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace Certify.Controllers
{
    [Authorize]
    public class MySignaturesController : Controller
    {
        CertifyDbContext _context;
        IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;

        public MySignaturesController(CertifyDbContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> ChangeStatusAsync(bool status, int id)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var signature = _context.Signatures.FirstOrDefault(s => s.DocumentId == id && s.UserId == user.Id);

            bool rightUser = await IsUserSignaturer(id);

            if (signature != null && rightUser)
            {
                if (status)
                {
                    signature.IsSigned = true;
                }
                else if (!status)
                {
                    signature.IsSigned = false;
                }

                signature.SignedDate = DateTime.Now;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public async Task<bool> IsUserSignaturer(int documentId)
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);

            bool isUserSignatuer = _context.Signatures.Any(s => s.DocumentId == documentId && s.UserId == currentUser.Id && s.IsSigned == null);

            return isUserSignatuer;
        }


        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            string userId = user.Id;
            var documents = _context.Signatures
                .Where(s => s.UserId == userId && s.IsSigned != null) //змінити на !=
                .Select(s => new MySignatureViewModel
                {
                    Id = s.DocumentId,
                    SignedDate = s.SignedDate,
                    IsSigned = s.IsSigned,
                    Title = s.Document.Title,
                    FileURL = s.Document.FileURL
                })
                .ToList();

            return View("Index", documents);
        }


    
    }
}
