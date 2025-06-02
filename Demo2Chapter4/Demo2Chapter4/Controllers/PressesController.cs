using Demo2Chapter4.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using System;

namespace Demo2Chapter4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PressesController : ODataController
    {
        private readonly BookStoreContext _db;

        public PressesController(BookStoreContext context)
        {
            _db = context;

            _db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            if (_db.Press.Count() == 0)
            {
                foreach (var b in DataSource.GetBooks())
                {
                    context.Press.Add(b.Press);
                    context.Books.Add(b);
                }
                context.SaveChanges();
            }
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Press);
        }
    }
}
