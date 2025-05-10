using DepiEcommerce.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();



builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    //options.UseSqlServer("Data Source=.;Initial Catalog=Organization;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
    options.UseSqlServer(builder.Configuration.GetConnectionString("CS"));
});
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseExceptionHandler("/Error");        // Handles 500 errors
    app.UseStatusCodePagesWithReExecute("/Error/{0}");  // Handles 404, etc.
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
                name: "Dashboard",
                pattern: "{area:exists}/{controller=Products}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
