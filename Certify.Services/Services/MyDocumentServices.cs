using AutoMapper;
using Certify.Library.ViewModels;
using Data;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Certify.Services.Services
{
    public class MyDocumentServices
    {


        private readonly UserManager<User>? _userManager;
        private readonly CertifyDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _appEnvironment;

        public MyDocumentServices(UserManager<User> userManager, CertifyDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _appEnvironment = appEnvironment;
        }


        //----------------------------------------------Index----------------------------------------------
        public async Task<List<MyDocumentsIndexVM>> GetDocumentForIndexAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var documents = _context.Documents.Where(d => d.UserId == user.Id).ToList();
            List<MyDocumentsIndexVM> documentList = _mapper.Map<List<Document>, List<MyDocumentsIndexVM>>(documents);

            foreach (var document in documentList)
            {
                int currentSignaturesCount = _context.Signatures.Count(s => s.DocumentId == document.Id && s.IsSigned != null);
                int maxSignaturesCount = _context.Signatures.Count(s => s.DocumentId == document.Id);
                bool isFalseSignatureExist = _context.Signatures.Any(s => s.DocumentId == document.Id && s.IsSigned == false);

                document.CurrentSignaturesCount = currentSignaturesCount;
                document.MaxSignaturesCount = maxSignaturesCount;

                if (isFalseSignatureExist)
                {
                    document.IsSigned = false;
                }
                else if (currentSignaturesCount != maxSignaturesCount)
                {
                    document.IsSigned = null;
                }
                else
                {
                    document.IsSigned = true;
                }
            }

            return documentList;

        }


        //----------------------------------------------Info----------------------------------------------
         public async Task<MyDocumentsInfoVM> InfoAsync(int id)
         {
             Document doc = await _context.Documents.Include(d => d.User).FirstAsync(d => d.Id == id);
             MyDocumentsInfoVM document = _mapper.Map<Data.Entity.Document, MyDocumentsInfoVM>(doc);
             SelectUserSigned(document, id);
             document.IsUserSignatuer = await IsUserSignaturer(document.Id);
             document.IsUserOwner = await IsUserOwner(document.Id);

             return document;
         }
        private async Task SelectUserSigned(MyDocumentsInfoVM document, int id)
        {
            var signedUsers = _context.Signatures
                .Include(s => s.User)
                .Where(s => s.DocumentId == id)
                .Select(s => new
                {
                    IsSigned = s.IsSigned,
                    UserDescription = $"{s.User.Firstname} {s.User.Lastname} ({s.User.Email})"
                })
                .ToList();

            document.SignedTrueUsers = signedUsers
                .Where(s => s.IsSigned == true)
                .Select(s => s.UserDescription)
                .ToList();

            document.SignedNullUsers = signedUsers
                .Where(s => s.IsSigned == null)
                .Select(s => s.UserDescription)
                .ToList();

            document.SignedFalseUsers = signedUsers
                .Where(s => s.IsSigned == false)
                .Select(s => s.UserDescription)
                .ToList();

        }


        //----------------------------------------------Delete----------------------------------------------
        public async Task DeleteAsync(int id)
        {
            var document = _context.Documents.Find(id);
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            var signatures = _context.Signatures.Where(s => s.DocumentId == document.Id);
            _context.Signatures.RemoveRange(signatures);

            string filePath = _appEnvironment.WebRootPath + document.FileURL;
            string folderPath = Path.GetDirectoryName(filePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);

                if (Directory.Exists(folderPath) && !Directory.EnumerateFiles(folderPath).Any())
                {
                    Directory.Delete(folderPath);
                }
            }
            _context.Documents.Remove(document);
            _context.SaveChanges();
         
        }


        //----------------------------------------------Create----------------------------------------------
        public async Task AddFile(MyDocumentsCreateVM dasc)
        {

            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string time = DateTime.Now.ToString("yyyyMMddHHmmss");

            string filePath = "/Documents/" + $"/{user.Id}-{time}/" + dasc.UploadedFile.FileName;
            string folderPath = Path.Combine(_appEnvironment.WebRootPath, "Documents", $"{user.Id}-{time}");
            Directory.CreateDirectory(folderPath);

            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + filePath, FileMode.Create))
            {
                await dasc.UploadedFile.CopyToAsync(fileStream);
            }

            Document document = _mapper.Map<MyDocumentsCreateVM, Document>(dasc);
            document.UploadedDate = DateTime.Now;
            document.FileURL = filePath;
            document.UserId = user.Id;

            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            await CreateSignatureAsync(dasc);
        }


        private async Task CreateSignatureAsync(MyDocumentsCreateVM dasc)
        {
            var lastDocument = _context.Documents.OrderByDescending(d => d.Id).First();
            var signatures = new List<Signature>();
            var selectedEmails = JsonConvert.DeserializeObject<List<string>>(dasc.UserEmail);
            foreach (var userEmail in selectedEmails)
            {
                string userId = GetUserIdByEmail(userEmail);
                if (userId != null)
                {
                    var signature = new Signature
                    {
                        IsSigned = null,
                        DocumentId = lastDocument.Id,
                        UserId = userId
                    };

                    signatures.Add(signature);
                }
            }
            _context.Signatures.AddRange(signatures);

            await _context.SaveChangesAsync();
        }

        private string GetUserIdByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user.Id;
        }
        public async Task<List<string>> GetEmailListCreateServices()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string userId = user.Id;

            var emailList = _context.Users
                .Where(u => u.Id != userId)
                .Select(u => u.Email)
                .ToList();

            return emailList;
        }

        public async Task<bool> CheckEmailExistsServices(string email)
        {
            var currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            bool exists = _context.Users.Any(u => u.Email == email && u.Id != currentUser.Id);
            return exists;
        }

        
        //----------------------------------------------All metod----------------------------------------------
        private async Task<bool> IsUserSignaturer(int documentId)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            User? currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            bool isUserSignatuer = _context.Signatures.Any(s => s.DocumentId == documentId && s.UserId == currentUser.Id && s.IsSigned == null);

            return isUserSignatuer;
        }

        public async Task<bool> IsUserOwner(int documentId)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            User? currentUser = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);

            bool isUserOwner = _context.Documents.Any(d => d.Id == documentId && d.UserId == currentUser.Id);

            return isUserOwner;
        }


        //----------------------------------------------ChangeStatus----------------------------------------------
        public async Task ChangeStatusAsync(bool status, int id)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
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
        }

        //----------------------------------------------Edit----------------------------------------------
        public async Task<MyDocumentsEditVM> EditAsync(int id)
        {
            MyDocumentsEditVM document = _mapper.Map<Document, MyDocumentsEditVM>(_context.Documents.Find(id));

            return document;
        }

        public async Task Edit(int id, MyDocumentsEditVM updatedDocument)
        {
            Document? document = _context.Documents.Find(id);

            document.Title = updatedDocument.Title;
            document.ShortDescription = updatedDocument.ShortDescription;

            _context.Documents.Update(document);
            _context.SaveChanges();

            if (updatedDocument.UserEmail != null)
            {
                var lastDocument = _context.Documents.OrderByDescending(d => d.Id).First();
                var signatures = new List<Signature>();
                var selectedEmails = JsonConvert.DeserializeObject<List<string>>(updatedDocument.UserEmail);
                foreach (var userEmail in selectedEmails)
                {
                    string userId = GetUserIdByEmail(userEmail);
                    if (userId != null)
                    {
                        var signature = new Signature
                        {
                            IsSigned = null,
                            DocumentId = lastDocument.Id,
                            UserId = userId
                        };

                        signatures.Add(signature);
                    }
                }
                _context.Signatures.AddRange(signatures);
                _context.SaveChanges();
            }
        }
        public async Task<List<string>> GetEmailListEditServices(int documentId)
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext.User);
            string userId = user.Id;
            var unassignedEmails = _context.Users
                .Where(u => u.Id != userId && !_context.Signatures.Any(s => s.DocumentId == documentId && s.UserId == u.Id))
                .Select(u => u.Email)
                .ToList();

            return unassignedEmails;
        }
    }
}