using AutoMapper;
using BusinessObject.DTOs;
using BusinessObjects.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Repository.IRepositories;
using Repository.Repositories;
using Service.IServices;
using Service.Services;

namespace FE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddHttpClient();

            builder.Services.AddControllersWithViews();

            // L?y chu?i k?t n?i t? c?u hình (appsettings.json)
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<FootballFieldManagerContext>(options =>
    options.UseSqlServer(connectionString)); // Ho?c provider DB khác n?u không ph?i SQL Server

            // ??ng ký các services, repositories và UnitOfWork
            builder.Services.AddScoped<IPitchServices, PitchService>();
            builder.Services.AddScoped<IPitchRepository, PitchRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ??ng ký AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // ???ng d?n ??n trang ??ng nh?p
        options.LogoutPath = "/Auth/Logout"; // ???ng d?n ??n trang ??ng xu?t
        options.AccessDeniedPath = "/Auth/AccessDenied"; // Trang khi ng??i dùng không có quy?n
        options.ExpireTimeSpan = TimeSpan.FromMinutes(builder.Configuration.GetValue<double>("Jwt:ExpiryMinutes")); // Th?i gian h?t h?n c?a cookie
    });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pitch, PitchDTO>().ReverseMap();
                cfg.CreateMap<PitchCreateDTO, Pitch>().ReverseMap();
                cfg.CreateMap<PitchUpdateDTO, Pitch>().ReverseMap();
                // Add mapping for PricePitch if needed to display in PitchDTO
                cfg.CreateMap<PricePitch, PricePitchDTO>().ReverseMap(); // Assuming you have a PricePitchDTO
                cfg.CreateMap<Pitch, PitchDTO>()
                    .ForMember(dest => dest.PricePitch, opt => opt.MapFrom(src => src.PricePitches.OrderByDescending(p => p.TimeStart).Select(p => p.Price).FirstOrDefault()));
            });
            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
