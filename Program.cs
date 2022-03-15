using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TEST.Data.Entities;
using TEST.Middlewares;
using TEST.Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<CategoryService>();
builder.Services.AddTransient<ArticleService>();
builder.Services.AddTransient<TagService>();
builder.Services.AddTransient<HeroImageService>();

ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<EfDBContex>(option => option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly(typeof(EfDBContex).Assembly.FullName)
    ));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<EfDBContex>();


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
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExeptionMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
