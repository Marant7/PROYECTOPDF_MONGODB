using Microsoft.AspNetCore.Authentication.Cookies;
using MongoDB.Driver;
using NegocioPDF.Config;
using NegocioPDF.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuración de MongoDB
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

builder.Services.AddSingleton<IMongoClient>(sp => 
    new MongoClient(builder.Configuration.GetSection("MongoDB:ConnectionString").Value));

// Registrar repositorios
builder.Services.AddSingleton<UsuarioRepository>();
builder.Services.AddSingleton<DetalleSuscripcionRepository>();
builder.Services.AddSingleton<OperacionesPDFRepository>();

// Configurar autenticación
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();