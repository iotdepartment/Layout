using Layout.Data;
using Layout.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Identity correcto
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// ✅ Cookies
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// MVC
builder.Services.AddControllersWithViews(options =>
{
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // ==========================================
    // ROLES
    // ==========================================

    string[] roles =
    {
        "Administrador",
        "Aprobador",
        "Gerente"
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // ==========================================
    // GERENTE PRODUCCIÓN
    // ==========================================

    var gerenteProduccion =
        await userManager.FindByEmailAsync("gerente.produccion@layout.com");

    if (gerenteProduccion == null)
    {
        gerenteProduccion = new ApplicationUser
        {
            UserName = "gerente.produccion@layout.com",
            Email = "gerente.produccion@layout.com",
            NombreCompleto = "Gerente Producción",
            EmailConfirmed = true,
            Activo = true,
            TipoFirmaId = 4 // Gte. Producción
        };

        var result = await userManager.CreateAsync(
            gerenteProduccion,
            "Gerente123$");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(
                gerenteProduccion,
                "Gerente");
        }
    }

    // ==========================================
    // GERENTE CALIDAD
    // ==========================================

    var gerenteCalidad =
        await userManager.FindByEmailAsync("gerente.calidad@layout.com");

    if (gerenteCalidad == null)
    {
        gerenteCalidad = new ApplicationUser
        {
            UserName = "gerente.calidad@layout.com",
            Email = "gerente.calidad@layout.com",
            NombreCompleto = "Gerente Calidad",
            EmailConfirmed = true,
            Activo = true,
            TipoFirmaId = 5 // Gte. Calidad
        };

        var result = await userManager.CreateAsync(
            gerenteCalidad,
            "Gerente123$");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(
                gerenteCalidad,
                "Gerente");
        }
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();