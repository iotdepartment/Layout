using Layout.Data;
using Layout.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// 🔌 Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔐 Identity (SOLUCIÓN: Rutas absolutas explícitas para eliminar el error CS0411 de raíz)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// 🍪 Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Pipeline de manejo de errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// =========================================================================
// 📂 CONFIGURACIÓN DE ARCHIVOS ESTÁTICOS Y DINÁMICOS
// =========================================================================

// 1. Soporte para los recursos estáticos nativos (CSS, JS de la app)
app.UseStaticFiles();

// 2. Mapeo físico de la carpeta 'uploads' para contenedores de producción (Docker)
var uploadsPath = Path.Combine(AppContext.BaseDirectory, "wwwroot", "uploads");
if (!Directory.Exists(uploadsPath))
{
    Directory.CreateDirectory(uploadsPath);
}

// 3. Exposición de la ruta virtual para que el modal de las solicitudes pueda leer las imágenes
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});
// =========================================================================

app.UseRouting();

// El orden de estos dos middlewares es obligatorio en .NET Core
app.UseAuthentication();
app.UseAuthorization();

// Optimizador de recursos estáticos nativos en tiempo de compilación (.NET 9)
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
