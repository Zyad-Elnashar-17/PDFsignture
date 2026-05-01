using Microsoft.AspNetCore.Mvc;

namespace PDFsignture.Controllers
{
    public class DocumentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
