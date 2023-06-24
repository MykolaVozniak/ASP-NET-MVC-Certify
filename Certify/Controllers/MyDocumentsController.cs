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
		private readonly CertifyDbContext context;

		public MyDocumentsController(CertifyDbContext context)
		{
			this.context = context;
		}

		public IActionResult Index()
        {
            return View();
        }
		
		[HttpGet]
		public IActionResult Add()
		{
			UsersList();
			return View();
		}

		private void UsersList()
		{
			var usersList = context.User.ToList();
			ViewBag.OSList = new SelectList(usersList, nameof(Users), nameof(OperationSystem.Name));
		}

		// POST: /Laptops/Add
		[HttpPost]
		public IActionResult Create(Document document)
		{
			if (!ModelState.IsValid)
			{
				UsersList();
				return View(document);
			}

			context.Laptops.Add(document);
			context.SaveChanges();

			TempData["alertMessage"] = "Product was successfully created!";

			return RedirectToAction(nameof(Index));
		}
	}
}
