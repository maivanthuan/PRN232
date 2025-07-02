using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ASS2_FE.Pages.Admin
{
    public class AllAccountsModel : PageModel
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;

        public AllAccountsModel(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _config = config;
        }

        public List<AccountDto>? Accounts { get; set; }

        public async Task OnGetAsync()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
            {
                Response.Redirect("/Login");
                return;
            }

            var client = _factory.CreateClient();
            client.BaseAddress = new Uri(_config["ApiBase"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var res = await client.GetAsync("odata/Systems");
            if (res.IsSuccessStatusCode)
            {
                var json = await res.Content.ReadAsStringAsync();
                Accounts = JsonSerializer.Deserialize<List<AccountDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
        }

        public class AccountDto
        {
            public string AccountEmail { get; set; }
            public string AccountName { get; set; }
            public int AccountRole { get; set; }
            
        }
    }
}

