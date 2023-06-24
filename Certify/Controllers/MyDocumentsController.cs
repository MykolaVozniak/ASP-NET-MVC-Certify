using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using Certify.Models;
using Certify.Data;

namespace Certify.Controllers
{
    public class MyDocumentsController : Controller
    {
        private readonly CertifyDbContext _context;

        public MyDocumentsController(CertifyDbContext context)
        {
            _context = context;
        }

        // GET: /Document/Add
        public IActionResult Add()
        {
            return View();
        }

        // POST: /Document/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Document document, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                // Отримання ідентифікатора поточного користувача (UserId)
                string currentUserId = "1"; /* Отримати ідентифікатор поточного користувача */

                // Заповнення додаткових полів документа
                document.Title = "ERORD"; /* Отримати назву документа */
                document.UploadedDate = DateTime.Now;
                document.UserId = currentUserId;

                // Збереження документа в базі даних
                _context.Documents.Add(document);
                _context.SaveChanges();

                // Збереження файлу у папці в рішенні
                string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Documents", file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Збереження шляху до файлу в поле FileURL документа
                document.FileURL = filePath;

                // Оновлення запису документа в базі даних з відповідним шляхом до файлу
                _context.SaveChanges();

                return RedirectToAction("Index", "Home"); // Перенаправлення на головну сторінку після успішного створення документу
            }

            return View(document);
        }
    }
}
