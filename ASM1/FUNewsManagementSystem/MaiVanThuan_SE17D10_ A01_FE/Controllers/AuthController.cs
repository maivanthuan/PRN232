using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace MaiVanThuan_SE17D10__A01_FE.Controllers
{
    public class AuthController : Controller
    {

        private readonly HttpClient _httpClient;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7080/api/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var request = new { Email = email, Password = password };
            var json = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("Auth/login", json);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Sai thông tin đăng nhập";
                return View();
            }

            var result = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var role = result.RootElement.GetProperty("role").GetString();
            var name = result.RootElement.GetProperty("name").GetString();
            if (role != "Admin")
            {
                var id = result.RootElement.GetProperty("id").GetInt32();
                HttpContext.Session.SetInt32("id", id!);
            }
            HttpContext.Session.SetString("UserRole", role!);
            HttpContext.Session.SetString("UserName", name!);
            

            if (role == "Admin") return RedirectToAction("Admin", "Home");
            if (role == "Staff") return RedirectToAction("Staff", "Home");
            if (role == "Lecturer") return RedirectToAction("Index", "News");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["Message"] = "Đăng xuất thành công.";
            return RedirectToAction("Index", "Home");
        }
    }
}
