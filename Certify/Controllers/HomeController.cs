using Certify.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Certify.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string[] aboutCertify =
            {
                "Welcome to Certify, where innovation meets security and convenience. We are your premier electronic signature and certification service, redefining the way you authenticate and validate important documents.",
                "At Certify, we understand the critical need for efficient and trustworthy digital solutions in today's fast-paced world. Gone are the days of endless paperwork, cumbersome printing, and laborious signing processes. With our cutting-edge technology, we empower businesses and individuals to streamline their document workflows, saving valuable time, resources, and ultimately, money.",
                "Our mission is simple yet transformative: to provide a seamless and secure platform that enables you to sign, certify, and manage your documents with unparalleled ease. We believe that embracing the digital realm should never compromise the integrity or reliability of your important paperwork. That's why we've developed a comprehensive suite of features that combine state-of-the-art encryption, robust authentication mechanisms, and user-friendly interfaces.",
                "When you choose Certify, you gain access to a range of powerful tools designed to simplify your document handling. Our electronic signature service allows you to sign agreements, contracts, and forms with just a few clicks, eliminating the need for printing, scanning, or mailing. With our advanced certification capabilities, you can ensure the authenticity and integrity of your documents, safeguarding them from any unauthorized modifications.",
                "We take data security seriously and prioritize the protection of your sensitive information. Our platform utilizes industry-leading encryption protocols and adheres to strict compliance standards, ensuring that your data remains confidential and tamper-proof. You can trust Certify to provide a fortified digital environment where your documents are safe from prying eyes and potential breaches.",
                "Our commitment to exceptional user experience extends beyond our technological advancements. We pride ourselves on delivering top-notch customer support, always ready to assist you in navigating our platform and addressing any queries or concerns. Your satisfaction is at the heart of everything we do, and we continuously strive to exceed your expectations.",
                "Whether you are a small business owner, a legal professional, or an individual seeking a more efficient way to manage your documents, Certify is here to transform your workflow. Embrace the power of digital transformation, and join the countless satisfied customers who have made Certify their go-to electronic signature and certification service.",
                "Discover a world where convenience and security harmoniously coexist. Welcome to Certify - where your documents meet peace of mind.",
            };
            return View(aboutCertify);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}