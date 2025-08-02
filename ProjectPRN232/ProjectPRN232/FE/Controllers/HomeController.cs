using System.Diagnostics;
using FE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(); // V?n l� trang ch? m?c ??nh cho m?i ng??i d�ng
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Action cho trang Admin Index
        [Authorize(Roles = "Admin")] // Ch? Admin m?i truy c?p ???c Action n�y
        public IActionResult AdminIndex() // ??i t�n ?? tr�nh xung ??t v?i Index() m?c ??nh
        {
            ViewData["ControllerName"] = "Home";
            // V� AdminIndex.cshtml s? n?m trong Views/Home, n�n ch? c?n return View()
            // ho?c n?u mu?n r� r�ng h?n, c� th? return View("AdminIndex");
            return View();
        }

        // C�c Action qu?n l� kh�c d�nh cho Admin
        [Authorize(Roles = "Admin")]
        public IActionResult AccountsManager()
        {
            // Logic ?? hi?n th? trang qu?n l� t�i kho?n
            // N?u View ManageAccounts.cshtml n?m trong Views/Home/Admin (th? m?c con)
            // b?n c� th? c?n ch? ??nh ???ng d?n: return View("Admin/ManageAccounts");
            // Ho?c n?u n� ? Views/Home/ManageAccounts.cshtml: return View();
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult BookingsManager()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult RevenueManager()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult PitchesManager()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult CheckboxTest()
        {
            return View();
        }

    }
}
