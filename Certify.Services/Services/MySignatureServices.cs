using Certify.Library.ViewModels;
using Data;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Certify.Services.Services
{
    public class MySignaturesServices
    {

        private readonly UserManager<User>? _userManager;
        private readonly CertifyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MySignaturesServices(UserManager<User> userManager, CertifyDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<ForMySignaturesIndex>> GetSignedDocumentsByUserId()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string userId = user.Id;
            var documents = _context.Signatures
                .Where(s => s.UserId == userId && s.IsSigned != null) //змінити на !=
                .Select(s => new ForMySignaturesIndex
                {
                    Id = s.DocumentId,
                    SignedDate = s.SignedDate,
                    IsSigned = s.IsSigned,
                    Title = s.Document.Title,
                    FileURL = s.Document.FileURL
                })
                .ToList();

            return documents;
        }
    }
}
