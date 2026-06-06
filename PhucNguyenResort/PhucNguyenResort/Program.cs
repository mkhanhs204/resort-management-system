using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();