using Models;
using Repositories;
using Repositories.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddScoped<IRepositoryBase<Cat>, CatRepository>();

builder.Services.AddHttpClient();

builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoRepository, MomoRepository>();

builder.Services.AddScoped<ReservationRepository>();
builder.Services.AddScoped<IRepositoryBase<Reservation>, ReservationRepository>();

builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IRepositoryBase<Product>, ProductRepository>();

builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<IRepositoryBase<Customer>, CustomerRepository>();

builder.Services.AddScoped<StaffRepository>();
builder.Services.AddScoped<IRepositoryBase<Staff>, StaffRepository>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(1);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = ".CCP.Session";
});


builder.Services.AddScoped<IRepositoryBase<Bill>, BillRepository>();
builder.Services.AddScoped<IRepositoryBase<BillProduct>, BillProductRepository>();
builder.Services.AddScoped<IRepositoryBase<Product>, ProductRepository>();
builder.Services.AddScoped<IRepositoryBase<Promotion>, PromotionRepository>();

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
