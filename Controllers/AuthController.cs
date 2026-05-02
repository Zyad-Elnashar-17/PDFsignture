using Microsoft.AspNetCore.Mvc;

namespace PDFsignture.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
