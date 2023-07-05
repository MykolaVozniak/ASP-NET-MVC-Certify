using Certify.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Certify.Controllers
{
    [Authorize]
    public class NotificationsController : Controller
    {
        private readonly NotificationServices _notificationServices;

        public NotificationsController(NotificationServices notificationServices)
        {
            _notificationServices = notificationServices;
        }

        public async Task<IActionResult> Index()
        {
            return View("Index", await _notificationServices.GetPendingSignaturesByUserId());
        }
    }
}