using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.IService;
using Service.Service;

namespace ASS2.Controllers
{
    [ApiController]
    [Route("")]

    public class NewsArticlesController : ControllerBase
    {
        private readonly INewsArticleService _service;
        public NewsArticlesController(INewsArticleService service) => _service = service;

        [Authorize]
        [EnableQuery]
        [HttpGet("/odata/News")]
        public IQueryable<NewsArticle> GetAllOData() => _service.GetAll();

        [Authorize]
        [HttpGet("api/NewsArticles/{id}")]
        public IActionResult Get(string id)
        {
            var result = _service.GetById(id);
            return result == null ? NotFound() : Ok(result);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPost("api/NewsArticles")]
        public IActionResult Create([FromBody] NewsArticle article)
        {
            _service.Add(article);
            return Ok();
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPut("api/NewsArticles/{id}")]
        public IActionResult Update(string id, [FromBody] NewsArticle article)
        {
            article.NewsArticleId = id;
            _service.Update(article);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("api/NewsArticles/{id}")]
        public IActionResult Delete(string id)
        {
            _service.Delete(id);
            return Ok();
        }

        [Authorize]
        [HttpGet("api/NewsArticles/search")]
        public IActionResult Search([FromQuery] string keyword)
            => Ok(_service.Search(keyword));

    }
}
