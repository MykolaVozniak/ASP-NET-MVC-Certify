using Data;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Certify.Services.Services
{
    public class NotificationServices
    {
        private readonly UserManager<User>? _userManager;
        private readonly CertifyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NotificationServices(UserManager<User> userManager, CertifyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> IsNotification()
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            bool? isNotification = _context.Signatures.Any(s => s.UserId == currentUser.Id && s.IsSigned == null);
            return isNotification ?? false;
        }

        public async Task<List<Signature>> GetPendingSignaturesByUserId()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string userId = user.Id;
            return await _context.Signatures
                .Include(s => s.Document)
                .Where(s => s.UserId == userId && s.IsSigned == null)
                .ToListAsync();
        }
    }
}

