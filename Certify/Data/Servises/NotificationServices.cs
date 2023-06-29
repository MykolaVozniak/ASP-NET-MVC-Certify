using Certify.Models;
using Microsoft.AspNetCore.Identity;

namespace Certify.Data.Servises
{
    public class NotificationServices
    {
        private readonly UserManager<User> _userManager;
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
    }
}
