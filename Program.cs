using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Simple_E_commers_App.Models;
using Simple_E_commers_App.Reprositrory;

namespace Simple_E_commers_App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("db")));
            builder.Services.AddScoped<ICatograyRebrestory, CategoryReprosetory>();
            builder.Services.AddScoped<IProductRebrestory, ProductRebrestory>();
            builder.Services.AddScoped<IOrderRebrestory, OrderRebrestory>();
            builder.Services.AddScoped<IOrderItemsRebrestory, OrderItemsRebrestory>();
            builder.Services.AddScoped<ICartReprosetory, CartReprosetory>();
            builder.Services.AddScoped<ICartItemReprosetory, CartItemReprosetory>();

            builder.Services.AddIdentity<AppUser, IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
