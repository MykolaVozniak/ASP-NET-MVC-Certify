using Certify.Data;
using Certify.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using Certify.ViewModels;
using Microsoft.EntityFrameworkCore;

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
                .Select(u => new
                {
                    Id = u.Id,
                    DisplayName = $"{u.Firstname} {u.Lastname} ({u.Email}) "
                })
                .ToList();

            ViewBag.UserList = new SelectList(userList, "Id", "DisplayName");
        }


        [HttpPost]
        public async Task<IActionResult> AddFile(DocumentCreate dasc)
        {
            if (dasc.UploadedFile != null)
            {
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
                _context.SaveChanges();


                var lastDocument = _context.Documents.OrderByDescending(d => d.Id).First();
                
                for(int i=0; i<dasc.UserId.Count; i++) 
                {
                    var signature = new Signature
                    {
                        IsSigned = null,
                        DocumentId = lastDocument.Id,
                        UserId = dasc.UserId[i]
                    };

                    _context.Signatures.Add(signature);
                    await _context.SaveChangesAsync();
                }


            }

            return RedirectToAction("Index");
        }


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
        }

        public IActionResult Info(int id)
        {
            DocumentInfo documentInfo = new();
            documentInfo.DocumentDI = _context.Documents.Find(id);
            SelectUserSigned(documentInfo);

            //string List<string> signedFalse=

            //string signedTrue =

            //string signedNull =


            if (documentInfo.DocumentDI == null)
            {
                return NotFound();
            }
            else
            {
                ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
                return View(documentInfo);
            }



        }

    }
}