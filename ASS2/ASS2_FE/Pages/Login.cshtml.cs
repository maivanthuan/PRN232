using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASS2_FE.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;

        public LoginModel(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _config = config;
        }

        [BindProperty] public string Email { get; set; }
        [BindProperty] public string Password { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync($"{_config["ApiBase"]}/api/Auth/login", new { Email, Password });

            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = "Đăng nhập thất bại!";
                return Page();
            }

            var loginRes = await response.Content.ReadFromJsonAsync<LoginResponse>();
            var token = loginRes?.Token;


            Console.WriteLine("TOKEN = " + token);
            var handler = new JwtSecurityTokenHandler();
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var role = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "";

            // Lưu token vào session
            HttpContext.Session.SetString("token", token);
            HttpContext.Session.SetString("role", role);

            return RedirectToPage(role == "Admin" ? "/Admin/Index" : "/News/ALL");
        }
    }
    public class LoginResponse
    {
        public string Token { get; set; }
    }

}
