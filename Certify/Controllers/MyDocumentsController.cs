using Certify.Data;
using Certify.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Certify.Controllers
{
    public class MyDocumentsController : Controller
    {
        CertifyDbContext _context;
        IWebHostEnvironment _appEnvironment;

        public MyDocumentsController(CertifyDbContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult AddIndex()
        {
            return View(_context.Documents.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> AddIndex(IFormFile uploadedFile)
        {

            string currentUserId = User.Identity.Name;

            if (uploadedFile != null)
            {
                // путь к папке Document
                string path = "/Documents/" + uploadedFile.FileName;
                // сохраняем файл в папку Document в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                Document file = new Document {
                    Id = 1,
                    Title = uploadedFile.FileName,
                    FileURL = path,
                    UserId = currentUserId,
                    UploadedDate = DateTime.Now,
                    ShortDescription = "defefefef",
                };
                _context.Documents.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("AddIndex");
        }
    }
}
