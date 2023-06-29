using Certify.Data;
using Certify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
             .Where(s => s.UserId == userId && s.IsSigned == null)
             .Select(s => s.Document)
             .ToList();

            return View("Index", documents);
        }
    }
}
