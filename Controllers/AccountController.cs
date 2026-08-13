using Layout.Models;
using Layout.Models.Enums;
using Layout.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Layout.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(SignInManager<ApplicationUser> signInManager,
                                 UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // ✅ GET LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // ✅ POST LOGIN
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                if (!user.Activo)
                {
                    ModelState.AddModelError("",
                        "Su cuenta está pendiente de autorización por parte de un administrador.");

                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName,
                    model.Password,
                    false,
                    false
                );

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError("", "Usuario o contraseña incorrectos");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existe = await _userManager.FindByEmailAsync(
                model.Email);

            if (existe != null)
            {
                ModelState.AddModelError("",
                    "Ya existe una cuenta con ese correo.");

                return View(model);
            }

            var usuario = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,

                Activo = false,

                EstatusUsuario =
                    EstatusUsuario.Pendiente,

                FechaSolicitudAcceso =
                    DateTime.Now
            };

            var result = await _userManager.CreateAsync(
                usuario,
                model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(
                        "",
                        error.Description);
                }

                return View(model);
            }

            TempData["Success"] =
                "La solicitud de acceso fue enviada correctamente. Espere autorización.";

            return RedirectToAction(nameof(Login));
        }

        // ✅ LOGOUT
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }


    }
}