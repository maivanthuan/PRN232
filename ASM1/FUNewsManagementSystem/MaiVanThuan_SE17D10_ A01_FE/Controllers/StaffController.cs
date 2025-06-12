using System.Text;
using System.Text.Json;
using DataAccessObjects.DTO;
using Microsoft.AspNetCore.Mvc;

namespace NguyenNhatTruong_SE17D10__A01_FE.Controllers
{
	public class StaffController : Controller
	{
		
			private readonly HttpClient _httpClient;

			public StaffController(IHttpClientFactory httpClientFactory)
			{
				_httpClient = httpClientFactory.CreateClient();
				_httpClient.BaseAddress = new Uri("https://localhost:7080/api/");
			}

        public async Task<IActionResult> AllNews(string? keyword = null)
        {
            var response = await _httpClient.GetAsync("news/active");
            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<NewsDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (!string.IsNullOrWhiteSpace(keyword))
                data = data.Where(n => n.NewsTitle?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false).ToList();

            return View("AllNews", data);
        }
        public async Task<IActionResult> MyNews(short staffId, string? keyword = null)
			{
				var response = await _httpClient.GetAsync($"news/author/{staffId}");
				if (!response.IsSuccessStatusCode) return View("Error");

				var json = await response.Content.ReadAsStringAsync();
				var data = JsonSerializer.Deserialize<List<NewsDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				if (!string.IsNullOrWhiteSpace(keyword))
					data = data.Where(n => n.NewsTitle?.Contains(keyword, StringComparison.OrdinalIgnoreCase) ?? false).ToList();

				return View(data);
			}

        // 2. Tạo mới bài viết
        [HttpGet]
        public IActionResult CreateNews()
        {
            // Khởi tạo model với TagIds là list rỗng để tránh lỗi null
            var model = new NewsCreateDto
            {
                TagIds = new List<int>()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNews(short staffId, NewsCreateDto dto)
        {
            // Parse TagIds từ chuỗi input
            var tagString = Request.Form["TagIds"].ToString();
            if (!string.IsNullOrWhiteSpace(tagString))
            {
                dto.TagIds = tagString
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s.Trim(), out var id) ? id : 0)
                    .Where(id => id > 0)
                    .ToList();
            }
            else
            {
                dto.TagIds = new List<int>();
            }

            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"news?authorId={staffId}", jsonContent);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("MyNews", new { staffId });

            TempData["Error"] = "Tạo bài viết thất bại.";
            return View(dto);
        }

        // 3. Cập nhật bài viết
        [HttpGet]
			public async Task<IActionResult> EditNews(string id)
			{

            var response = await _httpClient.GetAsync($"news/id/{id}");
				if (!response.IsSuccessStatusCode) return View("Error");

				var json = await response.Content.ReadAsStringAsync();
				var news = JsonSerializer.Deserialize<NewsCreateDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				return View(news);
			}

			[HttpPost]
			public async Task<IActionResult> EditNews(string id, short staffId, NewsCreateDto dto)
			{
				var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
				var response = await _httpClient.PutAsync($"news/{id}?authorId={staffId}", jsonContent);

				if (response.IsSuccessStatusCode)
					return RedirectToAction("MyNews", new { staffId });

				TempData["Error"] = "Cập nhật thất bại.";
				return View(dto);
			}

			// 4. Xóa bài viết
			[HttpPost]
			public async Task<IActionResult> DeleteNews(string id, short staffId)
			{
				var response = await _httpClient.DeleteAsync($"news/{id}?requesterId={staffId}");
				if (!response.IsSuccessStatusCode)
				{
					TempData["Error"] = "Không thể xóa bài viết.";
				}
				return RedirectToAction("MyNews", new { staffId });
			}

			// 5. Đọc chuyên mục
			public async Task<IActionResult> CategoryList()
			{
				var response = await _httpClient.GetAsync("category");
				if (!response.IsSuccessStatusCode) return View("Error");

				var json = await response.Content.ReadAsStringAsync();
				var categories = JsonSerializer.Deserialize<List<CategoryDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				return View(categories);
			}

			// 6. Thông tin cá nhân (nếu cần)
			public async Task<IActionResult> Profile(short staffId)
			{
				var response = await _httpClient.GetAsync($"account/{staffId}");
				if (!response.IsSuccessStatusCode) return View("Error");

				var json = await response.Content.ReadAsStringAsync();
				var account = JsonSerializer.Deserialize<AccountDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

				return View(account);
			}

			[HttpPost]
			public async Task<IActionResult> Profile(short staffId, AccountDto dto)
			{
				var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
				var response = await _httpClient.PutAsync($"account/{staffId}", jsonContent);

				if (!response.IsSuccessStatusCode)
					TempData["Error"] = "Cập nhật thất bại.";
				else
					TempData["Message"] = "Cập nhật thành công.";

				return RedirectToAction("Profile", new { staffId });
			}
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View(new CategoryDto()); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto dto)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("category", jsonContent);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("CategoryList");

            TempData["Error"] = "Tạo chuyên mục thất bại.";
            return View(dto);
        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(short id)
        {
            var response = await _httpClient.GetAsync($"category/{id}");
            if (!response.IsSuccessStatusCode) return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var category = JsonSerializer.Deserialize<CategoryDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(short id, CategoryDto dto)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"category/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("CategoryList");

            TempData["Error"] = "Cập nhật thất bại.";
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(short id)
        {
            var response = await _httpClient.DeleteAsync($"category/{id}");
            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Xóa thất bại.";

            return RedirectToAction("CategoryList");
        }

    }
}
