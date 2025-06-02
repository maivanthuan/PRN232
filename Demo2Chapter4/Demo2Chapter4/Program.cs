
using Demo2Chapter4.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System;

namespace Demo2Chapter4
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            
            builder.Services.AddDbContext<BookStoreContext>(opt =>
    opt.UseInMemoryDatabase("ODataDemoDb"));

            // Thêm controller + cấu hình OData
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Book>("Books");
            odataBuilder.EntitySet<Press>("Presses");

            builder.Services.AddControllers()
    .AddOData(opt =>
    {
        opt.AddRouteComponents("odata", GetEdmModel()) // 👈 dùng hàm GetEdmModel()
            .Select()
            .Filter()
            .OrderBy()
            .Expand()
            .Count()
            .SetMaxTop(100);
    });
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            IEdmModel GetEdmModel()
            {
                var builder = new ODataConventionModelBuilder();
                builder.EntitySet<Book>("Books");
                builder.EntitySet<Press>("Presses");
                return builder.GetEdmModel();
            }
        }
    }
}
