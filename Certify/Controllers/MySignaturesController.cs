using Certify.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Certify.Controllers
{
    [Authorize]
    public class MySignaturesController : Controller
    {

        MySignaturesServices _mySignaturesServices;
        public MySignaturesController(MySignaturesServices mySignaturesServices)
        {
            _mySignaturesServices = mySignaturesServices;

        }

        public async Task<IActionResult> IndexAsync()
        {

            return View("Index", await _mySignaturesServices.GetSignedDocumentsByUserId());
        }



    }
}
