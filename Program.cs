using Api.Config;
using Api.Data;
using Api.Endpoints.Admins;
using Api.Endpoints.Customers;
using Api.Endpoints.Drivers;
using Api.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

IConfigurationSection appConfigSection = builder.Configuration.GetSection(AppConfig.SectionName);
builder.Services.Configure<AppConfig>(appConfigSection);

var appConfig = appConfigSection.Get<AppConfig>();

if (appConfig is null) throw new Exception("AppConfig not provided.");

builder.WebHost.UseUrls($"http://*:{appConfig.Port}");

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(appConfig.ConnectionString));

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapAdminEndpoints();
app.MapDriverEndpoints();
app.MapCustomerEndpoints();

app.MapGet("health", () => new
{
  service = appConfig.Service,
  status = appConfig.Status,
  port = appConfig.Port,
  time = appConfig.Time
});

await app.RunAsync();
