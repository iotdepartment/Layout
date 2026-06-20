using Layout.Data;
using Layout.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔌 Base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔐 Identity
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


//// ✅ 🌱 SEED AUTOMÁTICO
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
//    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

//    // 🔹 Roles del sistema
//    string[] roles = { "Usuario", "Aprobador", "Administrador" };

//    foreach (var role in roles)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole(role));
//        }
//    }

//    // 🔹 Admin (también puede aprobar)
//    string adminEmail = "admin@test.com";
//    var adminUser = await userManager.FindByEmailAsync(adminEmail);

//    if (adminUser == null)
//    {
//        adminUser = new ApplicationUser
//        {
//            UserName = adminEmail,
//            Email = adminEmail,
//            NombreCompleto = "Administrador Sistema",
//            EmailConfirmed = true
//        };

//        await userManager.CreateAsync(adminUser, "Admin123!");
//        await userManager.AddToRoleAsync(adminUser, "Administrador");
//        await userManager.AddToRoleAsync(adminUser, "Aprobador");
//    }

//    // 🔹 Usuario normal
//    string userEmail = "user@test.com";
//    var normalUser = await userManager.FindByEmailAsync(userEmail);

//    if (normalUser == null)
//    {
//        normalUser = new ApplicationUser
//        {
//            UserName = userEmail,
//            Email = userEmail,
//            NombreCompleto = "Usuario Prueba",
//            EmailConfirmed = true
//        };

//        await userManager.CreateAsync(normalUser, "User123!");
//        await userManager.AddToRoleAsync(normalUser, "Usuario");
//    }
//}


// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();