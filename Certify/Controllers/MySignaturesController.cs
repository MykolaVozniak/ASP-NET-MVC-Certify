using Certify.Data;
using Certify.Models;
using Certify.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

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
