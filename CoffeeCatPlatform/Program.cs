using Models;
using Repositories;
using Repositories.Impl;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();
