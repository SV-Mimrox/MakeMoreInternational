using MakeMoreInternational.Models;
using MakeMoreInternational.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));

builder.Services.AddSingleton<IMongoClient>(s =>
{
    var settings = s.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<AdminService>();
builder.Services.AddSingleton<WebSettingService>();
builder.Services.AddSingleton<CategoryService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<HarvestCategoryService>();
builder.Services.AddSingleton<HarvestProductService>();
builder.Services.AddSingleton<HarvestSeasonService>();
builder.Services.AddSingleton<AboutUsService>();
builder.Services.AddSingleton<TeamMemberService>();
builder.Services.AddSingleton<ContactService>();
builder.Services.AddSingleton<InquiryService>();
builder.Services.AddSingleton<SliderService>();
builder.Services.AddSingleton<CertificateService>();
builder.Services.AddSingleton<BlogService>();
builder.Services.AddSingleton<PageSEOService>();

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
	name: "areas",
	pattern: "{area:exists}/{controller=Login}/{action=Index}/{id?}");


app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
