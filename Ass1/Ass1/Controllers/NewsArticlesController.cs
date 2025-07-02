using Ass1.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ass1.Controllers
{
    [Route("odata/NewsArticles")]
    [ApiController]
    public class NewsArticlesController : ODataController
    {
        private readonly INewsArticleRepository _repository;

        public NewsArticlesController(INewsArticleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ActionName("GetAll")]
        public IActionResult Get()
        {
            return Ok(_repository.GetAllAsync().Result);
        }

        [HttpGet("{id}")]
        [ActionName("GetById")]
        public IActionResult Get([FromRoute] string key)
        {
            var article = _repository.GetByIdAsync(key).Result;
            if (article == null) return NotFound();
            return Ok(article);
        }
    }
}