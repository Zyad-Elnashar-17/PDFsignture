using Microsoft.AspNetCore.Mvc;

namespace PDFsignture.Controllers
{
    public class SignatureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
