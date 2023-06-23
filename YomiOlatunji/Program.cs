using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YomiOlatunji.Core.DbModel;
using YomiOlatunji.DataSource;
using YomiOlatunji.DataSource.Interface;
using YomiOlatunji.DataSource.Repository;
using YomiOlatunji.DataSource.Services.Interfaces;
using YomiOlatunji.DataSource.Services;

var builder = WebApplication.CreateBuilder(args);


//Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<IPostRepository, PostRepository>();
builder.Services.AddTransient<ILogRepository, LogRepository>();
builder.Services.AddTransient<IPostTagRepository, PostTagRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IContactMessageRepository, ContactMessageRepository>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IPostService, PostService>();
builder.Services.AddTransient<IContactMessageService, ContactMessageService>();
builder.Services.AddTransient<IFileManager, FileManager>();
builder.Services.AddTransient<IPostManager, PostManager>();
builder.Services.AddTransient<IUploadImageService, UploadImageService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();



app.MapGet("/api/test", (IUploadImageService uploadService) =>
{
    return Results.Ok(uploadService.Test());
});
app.MapPost("/api/save-image", (HttpContext httpContext, IUploadImageService uploadService) =>
{
    IFormFile file1 = null;
    foreach (var file in httpContext.Request.Form.Files)
    {
        if (file.Length > 0)
        {
            file1 = file;
            break;
        }
    }
    return Results.Ok(uploadService.Upload(file1));
});
app.MapGet("/api/image", (string filename, IUploadImageService uploadService) =>
{
    return Results.Ok(filename);
});


SeedUserDataService.Initialize(app.Services);

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
app.UseAuthentication();;

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{

    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );

    endpoints.MapControllerRoute("blog",
                             "Blog/{slug}",
                             defaults: new { controller = "Home", action = "Blog" });

    endpoints.MapControllerRoute("blogs",
                             "Blogs",
                             defaults: new { controller = "Home", action = "Blogs" });

    endpoints.MapControllerRoute("about",
                             "About",
                             defaults: new { controller = "Home", action = "About" });

    endpoints.MapControllerRoute("contact",
                             "Contact",
                             defaults: new { controller = "Home", action = "Contact" });

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapRazorPages();

});

//app.MapControllerRoute(
//    name: "areas",
//    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");


//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "areas",
//      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
//    );
//});

app.Run();
