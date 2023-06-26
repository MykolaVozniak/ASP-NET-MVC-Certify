using Certify.Data;
using Certify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Certify.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        CertifyDbContext _context;
        IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;

        public NotificationsController(CertifyDbContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            string userId = user.Id;
            var signatures = await _context.Signatures
                .Where(s => s.UserId == userId)
                .ToListAsync();

            foreach (var signature in signatures)
            {
                signature.Document = await _context.Documents.FindAsync(signature.DocumentId);
            }

            return View("Index", signatures);
        }
    }
}