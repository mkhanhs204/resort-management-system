using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Services.MenuCategories;
using PhucNguyenResort.Services.RestaurantMenus;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

// Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.LogoutPath = "/Account/Logout";
        options.Cookie.Name = "PhucNguyenResort.Auth";
        options.SlidingExpiration = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
    });

builder.Services.AddAuthorization();

// Services
builder.Services.AddScoped<IMenuCategoryService, MenuCategoryService>();
builder.Services.AddScoped<IRestaurantMenuService, RestaurantMenuService>();

// DbContext
builder.Services.AddDbContext<ResortDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Services
builder.Services.AddScoped<PhucNguyenResort.Services.Rooms.IRoomService, PhucNguyenResort.Services.Rooms.RoomService>();
builder.Services.AddScoped<PhucNguyenResort.Services.RoomCategories.IRoomCategoryService, PhucNguyenResort.Services.RoomCategories.RoomCategoryService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Users.IUserService, PhucNguyenResort.Services.Users.UserService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Tours.ITourService, PhucNguyenResort.Services.Tours.TourService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Home.IHomeService, PhucNguyenResort.Services.Home.HomeService>();

var app = builder.Build();

// Auto Migration
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<ResortDbContext>();

    dbContext.Database.Migrate();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();