using Layout.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Layout.Controllers
{
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles
                .OrderBy(x => x.Name)
                .ToList();

            return View(new RolCreateViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(RolCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (await _roleManager.RoleExistsAsync(model.Nombre))
            {
                ModelState.AddModelError("", "El rol ya existe.");
                return View(model);
            }

            var resultado = await _roleManager.CreateAsync(
                new IdentityRole(model.Nombre));

            if (!resultado.Succeeded)
            {
                foreach (var error in resultado.Errors)
                {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
            }

            TempData["Success"] = "Rol creado correctamente";

            return RedirectToAction(nameof(Create));
        }
    }
}