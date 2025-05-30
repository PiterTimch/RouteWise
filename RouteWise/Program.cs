using RouteWise.BLL.Data;
using RouteWise.BLL.Interfaces;
using RouteWise.BLL.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string basePath = Directory.GetCurrentDirectory();

builder.Services.AddScoped<AppDBContext>(provider =>
{
    return new AppDBContext(Path.Combine(basePath, "BLL", "Data", "Json", "TransportStops.json"), Path.Combine(basePath, "BLL", "Data", "Json", "AllTransport.json"));
});

builder.Services.AddScoped<ITransportApiService, TransportApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TransportRoute}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
