using FE.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace FE.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        public AuthController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]); // Cấu hình BaseUrl trong appsettings.json
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View("~/Views/Home/Login.cshtml");
        }

        // POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken] // Chống tấn công CSRF
        public async Task<IActionResult> Login(LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var loginData = new
                {
                    username = model.Username,
                    password = model.Password
                };

                var json = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST đến API Backend
                var response = await _httpClient.PostAsync("api/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = JsonConvert.DeserializeAnonymousType(responseContent, new { Token = "", User = new { UserId = 0, Name = "", Role = "", StatusOtp = 0 } });

                    if (apiResponse != null && !string.IsNullOrEmpty(apiResponse.Token))
                    {
                        if (apiResponse.User.StatusOtp != 1) // Giả sử 1 là trạng thái đã xác minh OTP
                        {
                            ModelState.AddModelError(string.Empty, "Tài khoản của bạn đã bị chặn. Vui lòng quay lại sau.");
                            return View("~/Views/Auth/Login.cshtml", model); // Hoặc View của trang Login của bạn
                        }
                        // Giải mã JWT để lấy thông tin User
                        var handler = new JwtSecurityTokenHandler();
                        var jwtToken = handler.ReadToken(apiResponse.Token) as JwtSecurityToken;

                        if (jwtToken != null)
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, apiResponse.User.UserId.ToString()),
                                new Claim(ClaimTypes.Name, apiResponse.User.Name),
                                new Claim(ClaimTypes.Role, apiResponse.User.Role)
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var authProperties = new AuthenticationProperties
                            {
                                IsPersistent = model.RememberMe, // Ghi nhớ đăng nhập
                                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"])) // Thời gian hết hạn của cookie, khớp với JWT
                            };

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                            HttpContext.Response.Cookies.Append("JwtToken", apiResponse.Token, new CookieOptions // <--- ĐÂY LÀ DÒNG BẠN CẦN THÊM
                            {
                                HttpOnly = true, // Quan trọng: Ngăn JS truy cập để tăng bảo mật
                                Secure = true,   // Chỉ gửi qua HTTPS
                                SameSite = SameSiteMode.Strict,
                                Expires = DateTimeOffset.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"])) // Thời gian hết hạn của cookie JWT
                            });
                            // Chuyển hướng đến trang Home hoặc trang Admin/User dashboard tùy thuộc vào Role
                            if (apiResponse.User.Role == "Admin")
                            {
                                return RedirectToAction("AdminIndex", "Home"); // Ví dụ: Trang Admin
                            }
                            else if (apiResponse.User.Role == "User")
                            {
                                return RedirectToAction("Index", "Pitch"); // Ví dụ: Trang Home cho User
                            }
                            else
                            {
                                // Xử lý các vai trò khác nếu cần
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Đăng nhập thất bại. Vui lòng thử lại.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Lỗi đăng nhập: {errorContent}");
                }
            }

            // Nếu ModelState không hợp lệ hoặc đăng nhập thất bại, hiển thị lại View
            return View("~/Views/Home/Login.cshtml", model);
        }

        // POST: Auth/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
        public IActionResult Register()
        {
            return View(); // Sẽ tìm Views/Auth/Register.cshtml
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AccountCreateDTO model)
        {
            // Kiểm tra các validation rules đã định nghĩa trong AccountCreateDTO
            if (ModelState.IsValid)
            {
                // KHÔNG CẦN GÁN model.RoleId = 2; ở đây nữa vì đã làm ở BE
                // Tạo một bản sao model để loại bỏ ConfirmPassword trước khi gửi lên API
                var dataToSend = new
                {
                    model.UserName,
                    model.Email,
                    model.Name,
                    model.Gender,
                    model.PhoneNumber,
                    model.Password,
                    // Không gửi ConfirmPassword lên API
                    model.Avata,
                    model.DateOfBirth
                };

                var json = JsonConvert.SerializeObject(dataToSend);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST đến API Backend để tạo tài khoản mới
                // Đảm bảo URL này khớp với endpoint API của bạn (VD: api/Account)
                var response = await _httpClient.PostAsync("api/Account", content);

                if (response.IsSuccessStatusCode)
                {
                    // Đăng ký thành công, chuyển hướng người dùng đến trang đăng nhập
                    TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    // Nếu API Backend trả về lỗi cụ thể, có thể deserialize để hiển thị chi tiết hơn
                    // Ví dụ: tài khoản đã tồn tại, email đã được sử dụng
                    ModelState.AddModelError(string.Empty, $"Đăng ký thất bại: {errorContent}");

                    // Cố gắng phân tích lỗi từ backend (nếu backend trả về JSON có lỗi cụ thể)
                    try
                    {
                        var apiError = JsonConvert.DeserializeAnonymousType(errorContent, new { errors = new Dictionary<string, List<string>>() });
                        if (apiError?.errors != null)
                        {
                            foreach (var key in apiError.errors.Keys)
                            {
                                foreach (var errorMsg in apiError.errors[key])
                                {
                                    // Map lỗi API về trường tương ứng trên Model state
                                    // Ví dụ: nếu API trả về lỗi cho "UserName", nó sẽ hiển thị dưới trường UserName
                                    ModelState.AddModelError(key, errorMsg);
                                }
                            }
                        }
                    }
                    catch (JsonException)
                    {
                        // Không thể deserialize lỗi, giữ nguyên lỗi chung
                    }
                }
            }
            // Nếu ModelState không hợp lệ hoặc đăng ký thất bại, hiển thị lại form với lỗi
            return View(model);
        }

    }
}
