using BusinessObject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Service.IService;

namespace ASS2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagService _service;
        public TagsController(ITagService service) => _service = service;

        [Authorize]
        [EnableQuery]
        [HttpGet("/odata/Tags")]
        public IQueryable<Tag> GetAllOData() => _service.GetAll();

        //[Authorize]
        //[HttpGet("{id}")]
        //public IActionResult Get(int id)
        //{
        //    var tag = _service.GetById(id);
        //    return tag == null ? NotFound() : Ok(tag);
        //}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(Tag tag)
        {
            _service.Add(tag);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, Tag tag)
        {
            tag.TagId = id;
            _service.Update(tag);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _service.Delete(id);
            return Ok();
        }

    }
}
