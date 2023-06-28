using Certify.Data;
using Certify.Models;
using Certify.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json;


namespace Certify.Controllers
{
    public class MyDocumentsController : Controller
    {
        private readonly CertifyDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;

        public MyDocumentsController(CertifyDbContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
        }

        //Index
        [Authorize]
        public IActionResult Index()
        {
            var documents = _context.Documents.ToList();
            return View("Index", documents);
        }

        //Create
        [Authorize]
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
                .Select(u => new
                {
                    Id = u.Id,
                    DisplayName = $"{u.Firstname} {u.Lastname} ({u.Email})"
                })
                .ToList();

            ViewBag.UserList = new SelectList(userList, "Id", "DisplayName");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFile(DocumentCreate dasc)
        {

            if (!ModelState.IsValid)
            {
                await SelectUserAsync();
                return View("Create", dasc);
            }

            var user = await _userManager.GetUserAsync(HttpContext.User);
            string path = "/Documents/" + dasc.UploadedFile.FileName;

            using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
            {
                await dasc.UploadedFile.CopyToAsync(fileStream);
            }

            var document = new Document
            {
                Title = dasc.Title,
                ShortDescription = dasc.ShortDescription,
                FileURL = path,
                UserId = user.Id,
                UploadedDate = DateTime.Now
            };
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

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

            return RedirectToAction("Index");

        }

        [HttpGet]
        public IActionResult CheckEmailExists(string email)
        {
            bool exists = _context.Users.Any(u => u.Email == email);
            return Json(new { exists });
        }
        private string GetUserIdByEmail(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            return user.Id;
        }

        //Info
        [HttpGet]
        public IActionResult Info(int id)
        {
            DocumentInfo documentInfo = new();
            documentInfo.DocumentDI = _context.Documents.Find(id);
            SelectUserSigned(documentInfo);

            if (documentInfo.DocumentDI == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
                ViewBag.CurrentUrl = HttpContext.Request.GetDisplayUrl().ToString();
                return View(documentInfo);
            }
        }

        //Method exist User Signature
        private void SelectUserSigned(DocumentInfo tm)
        {
            var signedUsers = _context.Signatures
                    .Include(s => s.User)
                    .Where(s => s.DocumentId == tm.DocumentDI.Id)
                    .Select(s => new
                    {
                        IsSigned = s.IsSigned,
                        UserDescription = $"{s.User.Firstname} {s.User.Lastname} ({s.User.Email})"
                    })
                    .ToList();

            ViewBag.SignedTrue = signedUsers.Where(s => s.IsSigned == true)
                                            .Select(s => s.UserDescription)
                                            .ToList();
            ViewBag.SignedNull = signedUsers.Where(s => s.IsSigned == null)
                                            .Select(s => s.UserDescription)
                                            .ToList();
            ViewBag.SignedFalse = signedUsers.Where(s => s.IsSigned == false)
                                             .Select(s => s.UserDescription)
                                             .ToList();

            //isIUserSignaturer - тру -> поточний юзер = в табличці з підписами цей документ і налл
            //фолс - шо лібо з цього не вірно
            //isMyDocument - 1 якшо це мій док
        }
    }
}