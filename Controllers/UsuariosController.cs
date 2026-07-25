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
                    .OrderBy(x => x.Nombre)
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Nombre
                    })
                    .ToListAsync(),

                TiposFirma = await _context.TiposFirma
                    .OrderBy(x => x.Nombre)
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
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre
                })
                .ToListAsync();

            model.TiposFirma = await _context.TiposFirma
                .OrderBy(x => x.Nombre)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Nombre
                })
                .ToListAsync();

            model.Roles = await _roleManager.Roles
                .OrderBy(x => x.Name)
                .Select(x => new SelectListItem
                {
                    Value = x.Name,
                    Text = x.Name
                })
                .ToListAsync();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(
    UsuarioCreateViewModel model)
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
            }

            // Asignar Tipos de Firma
            if (model.TiposFirmaSeleccionados != null &&
                model.TiposFirmaSeleccionados.Any())
            {
                foreach (var tipoFirmaId in model.TiposFirmaSeleccionados)
                {
                    _context.UsuarioTiposFirma.Add(
                        new UsuarioTipoFirma
                        {
                            UsuarioId = usuario.Id,
                            TipoFirmaId = tipoFirmaId
                        });
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Usuario registrado correctamente.";

            return RedirectToAction(nameof(Create));
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> AreasAsignadas()
        {
            var usuarios = await _userManager.Users
                .Include(x => x.TiposFirma)
                    .ThenInclude(x => x.TipoFirma)
                .Include(x => x.Areas)
                    .ThenInclude(x => x.Area)
                .OrderBy(x => x.NombreCompleto)
                .ToListAsync();
            return View(usuarios);
        }

    }
}