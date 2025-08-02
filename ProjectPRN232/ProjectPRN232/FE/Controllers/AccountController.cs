using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using FE.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using JsonException = Newtonsoft.Json.JsonException;

namespace FE.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AccountController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.BaseAddress = new Uri(_configuration["ApiSettings:BaseUrl"]);
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(userId == null)
            {             
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            AccountDTO? profile = null; // Khởi tạo null hoặc một đối tượng rỗng
            try
            {
                var response = await _httpClient.GetAsync($"api/Account/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    profile = JsonConvert.DeserializeObject<AccountDTO>(responseContent);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    TempData["ErrorMessage"] = "Phiên làm việc đã hết hạn hoặc bạn không có quyền. Vui lòng đăng nhập lại.";
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Lỗi từ API khi tải profile: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi kết nối đến API khi tải profile: {ex.Message}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi không mong muốn khi tải profile: {ex.Message}");
            }

            // Kiểm tra nếu profile vẫn null hoặc không hợp lệ sau khi gọi API
            if (profile == null || profile.UserId == 0)
            {
                TempData["ErrorMessage"] = TempData["ErrorMessage"] ?? "Không thể tải thông tin profile của bạn. Vui lòng thử lại.";
                return View(new AccountDTO()); // Trả về AccountDTO rỗng
            }

            return View(profile); // Trả về AccountDTO đã lấy được
        }
        
        public async Task<IActionResult> EditProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var response = await _httpClient.GetAsync($"api/Account/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var accountDto = JsonConvert.DeserializeObject<AccountDTO>(responseContent);

                if (accountDto == null)
                {
                    TempData["ErrorMessage"] = "Không thể tải thông tin profile để chỉnh sửa.";
                    return RedirectToAction("Profile");
                }

                // Map AccountDTO sang AccountUpdateDTO. DateOfBirth đã là string?
                var updateModel = new AccountUpdateDTO
                {
                    Email = accountDto.Email,
                    Name = accountDto.Name,
                    Gender = accountDto.Gender,
                    PhoneNumber = accountDto.PhoneNumber,
                    DateOfBirth = accountDto.DateOfBirth // Map trực tiếp string? sang string?
                };
                return View(updateModel);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
            {
                TempData["ErrorMessage"] = "Phiên làm việc đã hết hạn hoặc bạn không có quyền. Vui lòng đăng nhập lại.";
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                TempData["ErrorMessage"] = $"Lỗi khi tải thông tin chỉnh sửa: {response.StatusCode} - {errorContent}";
                return RedirectToAction("Profile");
            }
        }

        // POST: Account/EditProfile
        // Trong FE/Controllers/AccountController.cs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(AccountUpdateDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Lấy JWT Token từ cookie có tên "JwtToken"
            var token = HttpContext.Request.Cookies["JwtToken"]; // <<< SỬ DỤNG TÊN COOKIE MỚI ĐÃ LƯU

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                TempData["ErrorMessage"] = "Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.";
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Auth");
            }

            // Gán JWT Token vào Header Authorization
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var json = JsonConvert.SerializeObject(model);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Account/{userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    // === BẮT ĐẦU PHẦN THAY ĐỔI ===

                    // 1. Lấy tất cả Claims hiện tại của người dùng
                    var currentClaims = User.Claims.ToList();


                    var updatedClaims = new List<Claim>();
                    foreach (var claim in currentClaims)
                    {
                        if (claim.Type == ClaimTypes.Name)
                        {
                            updatedClaims.Add(new Claim(ClaimTypes.Name, model.Name));
                        }

                        else
                        {
                            updatedClaims.Add(claim);
                        }
                    }



                    // 3. Tạo ClaimsIdentity mới
                    var claimsIdentity = new ClaimsIdentity(updatedClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // 4. Lấy lại AuthenticationProperties từ HttpContext hiện tại để duy trì RememberMe, ExpiresUtc, v.v.
                    var currentAuthentication = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = currentAuthentication?.Properties;

                    // 5. SignInAsync lại để cập nhật cookie xác thực
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    TempData["SuccessMessage"] = "Thông tin tài khoản đã được cập nhật thành công!";
                    return RedirectToAction("Profile");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                {
                    TempData["ErrorMessage"] = "Bạn không có quyền hoặc phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.";
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return RedirectToAction("Login", "Auth");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var apiError = JsonConvert.DeserializeAnonymousType(errorContent, new { message = "", errors = new Dictionary<string, List<string>>() });
                        if (apiError?.errors != null)
                        {
                            foreach (var key in apiError.errors.Keys)
                            {
                                foreach (var errorMsg in apiError.errors[key])
                                {
                                    ModelState.AddModelError(key, errorMsg);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(apiError?.message))
                        {
                            ModelState.AddModelError(string.Empty, apiError.message);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, $"Cập nhật thất bại: {response.StatusCode} - {errorContent}");
                        }
                    }
                    catch (JsonException)
                    {
                        ModelState.AddModelError(string.Empty, $"Cập nhật thất bại. Lỗi từ máy chủ: {response.StatusCode} - {errorContent}");
                    }
                    return View(model);
                }
            }
            catch (HttpRequestException ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi kết nối đến dịch vụ: {ex.Message}");
                return View(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi không mong muốn: {ex.Message}");
                return View(model);
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Vui lòng kiểm tra lại các trường mật khẩu.";
                return View("Profile", await GetProfileData());
            }

            try
            {
                var token = HttpContext.Request.Cookies["JwtToken"];
                if (string.IsNullOrEmpty(token))
                {
                    TempData["ErrorMessage"] = "Bạn chưa đăng nhập hoặc phiên đăng nhập đã hết hạn.";
                    return RedirectToAction("Login", "Auth");
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Chuẩn bị dữ liệu đổi mật khẩu
                // Cần đảm bảo rằng model.CurrentPassword và model.NewPassword được sử dụng đúng cách
                // Tên thuộc tính trong đối tượng ẩn danh này phải khớp với DTO của Backend
                var changePasswordDto = new
                {
                    currentPassword = model.CurrentPassword, // Đảm bảo tên này khớp với Backend DTO
                    password = model.Password, // Đảm bảo tên này khớp với Backend DTO, bạn đang dùng model.Password (sai)
                                                     // Không cần gửi ConfirmNewPassword lên backend vì backend không sử dụng
                };

                var jsonContent = new StringContent(
                    Newtonsoft.Json.JsonConvert.SerializeObject(changePasswordDto, new Newtonsoft.Json.JsonSerializerSettings
                    {
                        // Đảm bảo tên thuộc tính được serialize thành camelCase
                        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.PutAsync("api/Account/change-password", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
                    return RedirectToAction("Profile");
                }
                else // Xử lý lỗi từ API
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    try
                    {
                        dynamic apiErrorDynamic = Newtonsoft.Json.JsonConvert.DeserializeObject(errorContent);

                        Newtonsoft.Json.Linq.JObject? apiErrorsJObject = null;
                        if (apiErrorDynamic != null && apiErrorDynamic.errors != null)
                        {
                            apiErrorsJObject = apiErrorDynamic.errors as Newtonsoft.Json.Linq.JObject;
                        }

                        if (apiErrorsJObject != null)
                        {
                            // Lỗi: 'JObject' không có thuộc tính 'Keys'.
                            // Sử dụng .Properties() để lặp qua các thuộc tính của JObject.
                            foreach (var property in apiErrorsJObject.Properties()) // ĐÃ SỬA CHỮA ĐOẠN NÀY
                            {
                                var key = property.Name; 
                                var modelStateKey = char.ToUpper(key[0]) + key.Substring(1);

                                // Lấy danh sách các thông báo lỗi cho từng trường
                                if (property.Value is Newtonsoft.Json.Linq.JArray errorArray)
                                {
                                    foreach (var errorToken in errorArray)
                                    {
                                        var errorMsg = errorToken.ToString();
                                        // Chỉ thêm lỗi vào ModelState nếu khóa trùng khớp
                                        if (modelStateKey == nameof(ChangePasswordDTO.CurrentPassword) ||
                                            modelStateKey == nameof(ChangePasswordDTO.Password) ||
                                            modelStateKey == nameof(ChangePasswordDTO.ConfirmNewPassword))
                                        {
                                            ModelState.AddModelError(modelStateKey, errorMsg);
                                        }
                                        else // Lỗi không liên quan đến trường cụ thể (ví dụ: "message" hoặc lỗi chung)
                                        {
                                            ModelState.AddModelError(string.Empty, errorMsg);
                                        }
                                    }
                                }
                            }
                        }
                        // Xử lý trường hợp chỉ có thuộc tính "message" (lỗi chung không gắn với trường cụ thể)
                        else if (apiErrorDynamic != null && !string.IsNullOrEmpty(apiErrorDynamic.message?.ToString()))
                        {
                            ModelState.AddModelError(string.Empty, apiErrorDynamic.message.ToString());
                        }
                        else
                        {
                            // Trường hợp không parse được lỗi hoặc lỗi không có định dạng mong muốn
                            ModelState.AddModelError(string.Empty, $"Cập nhật thất bại: {response.StatusCode} - {errorContent}");
                        }
                    }
                    catch (Newtonsoft.Json.JsonException ex)
                    {
                        // Lỗi khi không thể parse JSON từ phản hồi API
                        ModelState.AddModelError(string.Empty, $"Cập nhật thất bại. Lỗi từ máy chủ (không thể đọc JSON): {ex.Message}");
                    }
                    catch (Exception ex)
                    {
                        // Lỗi không mong muốn trong quá trình xử lý phản hồi lỗi API
                        ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi không mong muốn trong quá trình xử lý lỗi API: {ex.Message}");
                    }
                    // Dù có lỗi hay không, vẫn hiển thị lại trang Profile với thông tin lỗi
                    return View("Profile", await GetProfileData());
                }
            }
            catch (HttpRequestException ex)
            {
                // Lỗi kết nối đến API (Backend không chạy, sai URL, CORS...)
                ModelState.AddModelError(string.Empty, $"Lỗi kết nối đến dịch vụ: {ex.Message}");
                return View("Profile", await GetProfileData());
            }
            catch (Exception ex)
            {
                // Các lỗi không mong muốn khác
                ModelState.AddModelError(string.Empty, $"Đã xảy ra lỗi không mong muốn: {ex.Message}");
                return View("Profile", await GetProfileData());
            }
        }

        // Helper method to get profile data
        private async Task<AccountDTO> GetProfileData()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return new AccountDTO(); // Trả về đối tượng rỗng nếu không có userId

            var token = HttpContext.Request.Cookies["JwtToken"];
            if (string.IsNullOrEmpty(token)) return new AccountDTO();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"api/Account/{userId}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return System.Text.Json.JsonSerializer.Deserialize<AccountDTO>(content, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new AccountDTO();
            }
            return new AccountDTO();
        }

        // Helper method to get profile data and retain validation errors
        private async Task<AccountDTO> GetProfileDataWithErrors(ChangePasswordDTO changePasswordModel)
        {
            var profileData = await GetProfileData();
            // Nếu bạn muốn giữ lại các giá trị nhập vào của form đổi mật khẩu khi có lỗi,
            // bạn có thể gán chúng vào một thuộc tính tạm thời trong ProfileViewModel
            // Tuy nhiên, ở đây ta chỉ cần trả về AccountDTO, lỗi sẽ hiển thị qua ModelState
            return profileData;
        }



    }
}
