using AutoMapper;
using Certify.Library.ViewModels;
using Certify.Services.Services;
using Data;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Certify.Controllers
{
    public class MyDocumentsController : Controller
    {
        private readonly CertifyDbContext _context;
        private readonly IWebHostEnvironment _appEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly MyDocumentServices _myDocumentServices;

        public MyDocumentsController(CertifyDbContext context, IWebHostEnvironment appEnvironment, UserManager<User> userManager, IMapper mapper, MyDocumentServices myDocumentServices)
        {
            _context = context;
            _appEnvironment = appEnvironment;
            _userManager = userManager;
            _mapper = mapper;
            _myDocumentServices = myDocumentServices;
        }

        //----------------------------------------------Index----------------------------------------------
        [Authorize]
        public async Task<IActionResult> IndexAsync()
        {
            return View("Index", await _myDocumentServices.GetDocumentForIndexAsync());
        }

        //----------------------------------------------Delete----------------------------------------------
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool document = _context.Documents.Any(d => d.Id == id);
            if (document == false || !await _myDocumentServices.IsUserOwner(id))
                return NotFound();

            _myDocumentServices.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        //----------------------------------------------Create----------------------------------------------

        [Authorize]
        public async Task<IActionResult> CreateAsync()
        {
            return View("Create");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddFile(MyDocumentsCreateVM dasc)
        {

            if (!ModelState.IsValid)
                return View("Create", dasc);
            await _myDocumentServices.AddFile(dasc);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<JsonResult> GetEmailListCreate()
        {
            var emailList = await _myDocumentServices.GetEmailListCreateServices();
            return new JsonResult(emailList);
        }

        [HttpGet]
        public async Task<JsonResult> CheckEmailExistsAsync(string email)
        {
            bool exists = await _myDocumentServices.CheckEmailExistsServices(email);
            return new JsonResult(new { exists });
        }

        //----------------------------------------------Info----------------------------------------------

        [HttpGet]
        public async Task<IActionResult> InfoAsync(int id)
        {
            bool doc = await _context.Documents.AnyAsync(d => d.Id == id);
            if (!doc)
                return NotFound();
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            ViewBag.CurrentUrl = HttpContext.Request.GetDisplayUrl().ToString();
            return View(await _myDocumentServices.InfoAsync(id));
        }


        //----------------------------------------------ChangeStatus----------------------------------------------
        public async Task<IActionResult> ChangeStatusAsync(bool status, int id)
        {
            await _myDocumentServices.ChangeStatusAsync(status, id);

            return RedirectToAction("Index", "Notifications");
        }

        //----------------------------------------------Edit----------------------------------------------
        [Authorize]
        public async Task<IActionResult> EditAsync(int id)
        {
            bool document = _context.Documents.Any(d => d.Id == id);
            if (!document || !await _myDocumentServices.IsUserOwner(id))
                return NotFound();

            return View(await _myDocumentServices.EditAsync(id));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditAsync(int id, MyDocumentsEditVM updatedDocument)
        {
            if (!ModelState.IsValid)
                return View("Edit", updatedDocument);
            await _myDocumentServices.Edit(id, updatedDocument);

            return RedirectToAction("Info", new{id});
        }

        [HttpGet]
        public async Task<JsonResult> GetEmailListEdit(int documentId)
        {
            var unassignedEmails = await _myDocumentServices.GetEmailListEditServices(documentId);
            return Json(unassignedEmails);
        }

    }
}