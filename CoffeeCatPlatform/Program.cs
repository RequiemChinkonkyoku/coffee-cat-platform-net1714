using Models;
using Repositories;
using Repositories.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
<<<<<<< HEAD
builder.Services.AddScoped<IRepositoryBase<Cat>, CatRepository>();
=======
builder.Services.AddHttpClient();

builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoRepository, MomoRepository>();

builder.Services.AddScoped<ReservationRepository>();
builder.Services.AddScoped<IRepositoryBase<Reservation>, ReservationRepository>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = ".CCP.Session";
});
>>>>>>> 7f3dc3cf5e92b3066f40e7b18621244fa1e1143e

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
