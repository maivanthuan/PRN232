using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementClient.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
