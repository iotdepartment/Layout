using Layout.Data;
using Layout.Models;
using Layout.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Layout.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsuariosController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            var vm = new UsuarioCreateViewModel
            {
                Areas = await _context.Areas
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Nombre
                    })
                    .ToListAsync(),

                Roles = await _roleManager.Roles
                    .OrderBy(r => r.Name)
                    .Select(r => new SelectListItem
                    {
                        Value = r.Name,
                        Text = r.Name
                    })
                    .ToListAsync()
            };

            var usuarios = await _userManager.Users
                .OrderBy(x => x.NombreCompleto)
                .ToListAsync();

            var usuariosTabla = new List<UsuarioListadoViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles = await _userManager.GetRolesAsync(usuario);

                usuariosTabla.Add(new UsuarioListadoViewModel
                {
                    Id = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    Email = usuario.Email,
                    Activo = usuario.Activo,
                    Rol = roles.FirstOrDefault() ?? "-"
                });
            }

            ViewBag.Usuarios = usuariosTabla;

            return View(vm);
        }

        private async Task CargarCombos(UsuarioCreateViewModel model)
        {
            model.Areas = await _context.Areas
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.Nombre
                })
                .ToListAsync();

            model.TiposFirma = await _context.TiposFirma
                .Select(t => new SelectListItem
                {
                    Value = t.Id.ToString(),
                    Text = t.Nombre
                })
                .ToListAsync();

            model.Roles = await _roleManager.Roles
    .OrderBy(r => r.Name)
    .Select(r => new SelectListItem
    {
        Value = r.Name,
        Text = r.Name
    })
    .ToListAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(UsuarioCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await CargarCombos(model);
                return View(model);
            }

            var usuario = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,

                TipoFirmaId = model.TipoFirmaId,

                EmailConfirmed = true,
                Activo = true
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

                await CargarCombos(model);

                return View(model);
            }

            // Asignar Rol
            await _userManager.AddToRoleAsync(
                usuario,
                model.Rol);

            // Asignar Áreas
            if (model.AreasSeleccionadas != null &&
                model.AreasSeleccionadas.Any())
            {
                foreach (var areaId in model.AreasSeleccionadas)
                {
                    _context.UsuarioAreas.Add(
                        new UsuarioArea
                        {
                            UsuarioId = usuario.Id,
                            AreaId = areaId
                        });
                }

                await _context.SaveChangesAsync();
            }

            TempData["Success"] =
                "Usuario registrado correctamente.";

            return RedirectToAction(nameof(Create));
        }
    }
}