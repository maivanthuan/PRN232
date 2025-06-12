using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementClient.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
