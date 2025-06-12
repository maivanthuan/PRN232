/*using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementClient.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var loginResult = await _apiService.LoginAsync(dto.Email, dto.Password);

        if (loginResult == null)
        {
            ModelState.AddModelError("", "Invalid credentials");
            return View(dto);
        }

        // Lưu role và username vào session
        HttpContext.Session.SetString("Role", loginResult.Role);
        HttpContext.Session.SetString("UserName", loginResult.Name);

        return RedirectToAction("Index", "Home");
    }
}
*/