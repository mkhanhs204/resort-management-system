using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using PhucNguyenResort.Data;
using PhucNguyenResort.Services.FoodOrders;
using PhucNguyenResort.Services.MenuCategories;
using PhucNguyenResort.Services.RestaurantMenus;
using PhucNguyenResort.Services.TableFoodOrders;
using PhucNguyenResort.Services.TableReservations;

var builder = WebApplication.CreateBuilder(args);

// MVC
var mvcBuilder = builder.Services.AddControllersWithViews();
if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}

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
        options.Events = new CookieAuthenticationEvents
        {
            OnValidatePrincipal = async context =>
            {
                var dbContext = context.HttpContext.RequestServices.GetRequiredService<ResortDbContext>();
                var userIdString = context.Principal?.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                
                if (int.TryParse(userIdString, out int userId))
                {
                    var isBlocked = await dbContext.Users
                        .Where(u => u.UserId == userId)
                        .Select(u => u.IsBlocked)
                        .FirstOrDefaultAsync();

                    if (isBlocked)
                    {
                        context.RejectPrincipal();
                        await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    }
                }
            }
        };
    })
    .AddCookie("ExternalCookie")
    .AddGoogle(googleOptions =>
    {
        googleOptions.SignInScheme = "ExternalCookie";
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
    });

builder.Services.AddAuthorization();

// Services
builder.Services.AddScoped<IMenuCategoryService, MenuCategoryService>();
builder.Services.AddScoped<IRestaurantMenuService, RestaurantMenuService>();
builder.Services.AddScoped<IFoodOrderService, FoodOrderService>();
builder.Services.AddScoped<ITableReservationService, TableReservationService>();
builder.Services.AddScoped<ITableFoodOrderService, TableFoodOrderService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Kots.IKotService, PhucNguyenResort.Services.Kots.KotService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Tables.IDiningTableService, PhucNguyenResort.Services.Tables.DiningTableService>();

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
builder.Services.AddScoped<PhucNguyenResort.Services.Tours.ITourCatalogService, PhucNguyenResort.Services.Tours.TourCatalogService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Tours.ITourScheduleService, PhucNguyenResort.Services.Tours.TourScheduleService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Tours.ITourBookingService, PhucNguyenResort.Services.Tours.TourBookingService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Tours.ICustomerTourBookingService, PhucNguyenResort.Services.Tours.CustomerTourBookingService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Home.IHomeService, PhucNguyenResort.Services.Home.HomeService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Bookings.IBookingService, PhucNguyenResort.Services.Bookings.BookingService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Bookings.IBookingGuestService, PhucNguyenResort.Services.Bookings.BookingGuestService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Reception.IReceptionService, PhucNguyenResort.Services.Reception.ReceptionService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Payments.IVnPayService, PhucNguyenResort.Services.Payments.VnPayService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Refunds.IRefundService, PhucNguyenResort.Services.Refunds.RefundService>();
builder.Services.AddScoped<PhucNguyenResort.Services.Email.IEmailSender, PhucNguyenResort.Services.Email.EmailSender>();
builder.Services.AddScoped<PhucNguyenResort.Services.Reviews.IReviewService, PhucNguyenResort.Services.Reviews.ReviewService>();
builder.Services.AddDataProtection();
builder.Services.AddHostedService<PhucNguyenResort.Services.Bookings.PdpdDataPurgeJob>();
builder.Services.AddHostedService<PhucNguyenResort.Services.Tours.TourBookingCleanupJob>();
builder.Services.AddHostedService<PhucNguyenResort.Services.Email.OtpCleanupJob>();
builder.Services.AddHostedService<PhucNguyenResort.Services.Bookings.PendingBookingCleanupJob>();

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
