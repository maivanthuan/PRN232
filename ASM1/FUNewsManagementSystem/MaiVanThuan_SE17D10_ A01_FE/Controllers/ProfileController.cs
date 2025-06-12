using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementClient.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
