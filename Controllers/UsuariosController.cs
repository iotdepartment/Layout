using Layout.Data;
using Layout.Models;
using Layout.Models.Enums;
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
                .Include(x => x.Areas)
                    .ThenInclude(x => x.Area)
                .Include(x => x.TiposFirma)
                    .ThenInclude(x => x.TipoFirma)
                .OrderBy(x => x.NombreCompleto)
                .ToListAsync();

            var usuariosTabla = new List<UsuarioListadoViewModel>();

            foreach (var usuario in usuarios)
            {
                var roles =
                    await _userManager.GetRolesAsync(usuario);

                usuariosTabla.Add(
                    new UsuarioListadoViewModel
                    {
                        Id = usuario.Id,

                        NombreCompleto =
                            usuario.NombreCompleto,

                        Email =
                            usuario.Email,

                        Activo =
                            usuario.Activo,

                        Rol =
                            roles.FirstOrDefault() ?? "-",

                        Areas =
                            usuario.Areas
                                .Select(x => x.Area.Nombre)
                                .ToList(),

                        Firmas = await _context.UsuarioTiposFirma
                                .Where(f => f.UsuarioId == usuario.Id)
                                .Select(f => f.TipoFirma.Nombre)
                                .ToListAsync(),
                        
                        TiposFirma =
                            usuario.TiposFirma
                                .Select(x => x.TipoFirma.Nombre)
                                .ToList()
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

        [Authorize(Roles = "Administrador,Aprobador")]
        public async Task<IActionResult> Pendientes()
        {
            var usuarios = await _userManager.Users
                .Where(x =>
                    x.EstatusUsuario ==
                    EstatusUsuario.Pendiente)
                .OrderBy(x => x.NombreCompleto)
                .ToListAsync();

            return View(usuarios);
        }



        //[Authorize(Roles = "Administrador,Aprobador")]
        //[HttpGet]
        //public async Task<IActionResult> Aprobar(string id)
        //{
        //    var usuario =
        //        await _userManager.FindByIdAsync(id);

        //    if (usuario == null)
        //        return NotFound();

        //    var vm =
        //        new AprobarUsuarioViewModel
        //        {
        //            UsuarioId = usuario.Id,
        //            NombreCompleto =
        //                usuario.NombreCompleto,
        //            Email = usuario.Email
        //        };

        //    return View(vm);
        //}

        [Authorize(Roles = "Administrador,Aprobador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(AprobarUsuarioViewModel model)
        {
            var usuario =
                await _userManager.FindByIdAsync(
                    model.UsuarioId);

            if (usuario == null)
                return NotFound();

            usuario.Activo = true;

            usuario.EstatusUsuario =
                EstatusUsuario.Aprobado;

            await _userManager.UpdateAsync(usuario);

            // Eliminar roles previos
            var rolesActuales =
                await _userManager.GetRolesAsync(usuario);

            if (rolesActuales.Any())
            {
                await _userManager.RemoveFromRolesAsync(
                    usuario,
                    rolesActuales);
            }

            // Asignar rol nuevo
            await _userManager.AddToRoleAsync(
                usuario,
                model.RolSeleccionado);

            // Limpiar áreas
            var areasActuales = _context.UsuarioAreas
                .Where(x => x.UsuarioId == usuario.Id);

            _context.UsuarioAreas.RemoveRange(
                areasActuales);

            // Guardar áreas
            foreach (var areaId in model.AreasSeleccionadas)
            {
                _context.UsuarioAreas.Add(
                    new UsuarioArea
                    {
                        UsuarioId = usuario.Id,
                        AreaId = areaId
                    });
            }

            // Limpiar firmas
            var firmasActuales = _context.UsuarioTiposFirma
                .Where(x => x.UsuarioId == usuario.Id);

            _context.UsuarioTiposFirma.RemoveRange(
                firmasActuales);

            // Guardar firmas
            foreach (var firmaId in model.TiposFirmaSeleccionados)
            {
                _context.UsuarioTiposFirma.Add(
                    new UsuarioTipoFirma
                    {
                        UsuarioId = usuario.Id,
                        TipoFirmaId = firmaId
                    });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Pendientes));
        }

        [Authorize(Roles = "Administrador,Aprobador")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rechazar(string id)
        {
            var usuario =
                await _userManager.FindByIdAsync(id);

            if (usuario == null)
                return NotFound();

            usuario.Activo = false;

            usuario.EstatusUsuario =
                EstatusUsuario.Rechazado;

            await _userManager.UpdateAsync(usuario);

            return RedirectToAction(nameof(Pendientes));
        }

        [HttpGet]
        [Authorize(Roles = "Administrador,Aprobador")]
        public async Task<IActionResult> ObtenerDatosAprobacion(string id)
        {
            var usuario =
                await _userManager.FindByIdAsync(id);

            if (usuario == null)
                return NotFound();

            return Json(new
            {
                usuarioId = usuario.Id,
                nombreCompleto = usuario.NombreCompleto,
                email = usuario.Email,

                roles = await _roleManager.Roles
                    .OrderBy(x => x.Name)
                    .Select(x => new
                    {
                        value = x.Name,
                        text = x.Name
                    })
                    .ToListAsync(),

                areas = await _context.Areas
                    .OrderBy(x => x.Nombre)
                    .Select(x => new
                    {
                        value = x.Id,
                        text = x.Nombre
                    })
                    .ToListAsync(),

                firmas = await _context.TiposFirma
                    .OrderBy(x => x.Nombre)
                    .Select(x => new
                    {
                        value = x.Id,
                        text = x.Nombre
                    })
                    .ToListAsync()
            });
        }
    }
}