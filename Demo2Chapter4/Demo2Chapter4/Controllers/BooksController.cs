using Demo2Chapter4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Demo2Chapter4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ODataController
    {
        private readonly BookStoreContext _db;

        public BooksController(BookStoreContext context)
        {
            _db = context;
            _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            if (_db.Books.Count() == 0)
            {
                foreach (var b in DataSource.GetBooks())
                {
                    _db.Books.Add(b);
                    _db.Press.Add(b.Press);
                }
                _db.SaveChanges();
            }
        }

        // ✅ GET ALL
        [EnableQuery(PageSize = 1)]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_db.Books);
        }

        // ✅ GET BY ID
        [EnableQuery]
        [HttpGet("{key}/{version}")]
        public IActionResult Get(int key, string version)
        {
            var book = _db.Books.FirstOrDefault(c => c.Id == key);
            if (book == null)
                return NotFound();
            return Ok(book);
        }

        // ✅ POST
        [EnableQuery]
        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
            return Created(book);
        }

        // ✅ DELETE
        [EnableQuery]
        [HttpDelete("{key}")]
        public IActionResult Delete(int key)
        {
            var b = _db.Books.FirstOrDefault(c => c.Id == key);
            if (b == null)
            {
                return NotFound();
            }

            _db.Books.Remove(b);
            _db.SaveChanges();
            return Ok();
        }
    }
}
