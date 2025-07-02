using BusinessObject.Models;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Repository.IRepository;
using Repository.Repository;
using Service.IService;
using Service.Service;
using System.Text;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");
        builder.Services.AddDbContext<FunewsManagementContext>(options =>
            options.UseSqlServer(connectionString));



        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            })

            .AddOData(options =>
            {
                var modelBuilder = new ODataConventionModelBuilder();
                modelBuilder.EntitySet<NewsArticle>("News");
                modelBuilder.EntitySet<Category>("Cates");
                modelBuilder.EntitySet<Tag>("Tags");
                modelBuilder.EntitySet<SystemAccount>("Systems");

                options.AddRouteComponents("odata", modelBuilder.GetEdmModel())
                       .Select().Filter().OrderBy().Expand().Count().SetMaxTop(100);
                

            });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "FUNews API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Nhập token dạng: Bearer <token>"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
        });

        builder.Services.AddScoped<INewsArticleService, NewsArticleService>();
        builder.Services.AddScoped<INewsArticleRepository, NewsArticleRepository>();

        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

        builder.Services.AddScoped<ITagService, TagService>();
        builder.Services.AddScoped<ITagRepository, TagRepository>();

        builder.Services.AddScoped<ISystemAccountService, SystemAccountService>();
        builder.Services.AddScoped<ISystemAccountRepository, SystemAccountRepository>();

        builder.Services.AddScoped<NewsArticleDAO>();
        builder.Services.AddScoped<CategoryDAO>();
        builder.Services.AddScoped<TagDAO>();
        builder.Services.AddScoped<SystemAccountDAO>();




        builder.Services.Configure<AdminConfig>(builder.Configuration.GetSection("AdminAccount"));

        builder.Services.AddScoped<ITokenService, TokenService>();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"]!)
            ),
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"]
                };

            });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseCors("AllowAll");

        app.UseHttpsRedirection();
        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
