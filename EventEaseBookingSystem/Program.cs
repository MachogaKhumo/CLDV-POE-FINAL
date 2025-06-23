using Microsoft.EntityFrameworkCore;
using EventEaseBookingSystem.Models;
using EventEaseBookingSystem.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load connection string safely from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ Register MVC services
builder.Services.AddControllersWithViews();

// ✅ Register EF Core with Azure SQL connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// ✅ Register Azure Blob Storage service with injected config
builder.Services.AddScoped<AzureBlobStorageService>();

var app = builder.Build();

// ✅ Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
