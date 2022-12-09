using AutoMapper;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.mapper;
using vega.Core.Models;
using vega.Persistence;
using vega.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<VegaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.Configure<PhotoSettings>(builder.Configuration.GetSection("PhotoSettings"));
//Habilita o CORS para aceitar requisições de outros domínios.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, c =>
     {
         c.Authority = $"https://{builder.Configuration["Auth0:Domain"]}";
         c.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
         {
             ValidAudience = builder.Configuration["Auth0:Audience"],
             ValidIssuer = $"{builder.Configuration["Auth0:Domain"]}"
         };
     });

// builder.Services.AddAuthentication(options =>
//         {
//             options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//             options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         }).AddJwtBearer(options =>
//         {
//             options.Authority = "https://projetovega.us.auth0.com/";
//             options.Audience = "https://api.vega.com";
//         });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
