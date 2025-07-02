using Ass1.Data;
using Ass1.Models;
using Ass1.Repositories;
using Ass1.Repositories.Interfaces;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace Ass1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers()
                .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel())
                .Filter().Select().Expand().OrderBy().Count().SetMaxTop(100));

            builder.Services.AddDbContext<FUNewsManagementSystemContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("Ass1")));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
            builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            
            app.Run();
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<NewsArticle>("NewsArticles");
            builder.EntitySet<SystemAccount>("SystemAccounts");
            builder.EntitySet<Tag>("Tags");
            return builder.GetEdmModel();
        }
    }
}