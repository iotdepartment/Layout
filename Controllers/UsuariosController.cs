using Layout.Data;
using Layout.Models;
using Layout.Models.ViewModels;
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

        public UsuariosController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new UsuarioCreateViewModel
            {
                Areas = await _context.Areas
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Nombre
                    }).ToListAsync(),

                TiposFirma = await _context.TiposFirma
                    .Select(x => new SelectListItem
                    {
                        Value = x.Id.ToString(),
                        Text = x.Nombre
                    }).ToListAsync(),

                Roles = new List<SelectListItem>
                {
                    new("Administrador","Administrador"),
                    new("Aprobador","Aprobador"),
                    new("Gerente","Gerente"),
                    new("Usuario","Usuario")
                }
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            UsuarioCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                NombreCompleto = model.NombreCompleto,
                AreaId = model.AreaId,
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
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

            await _userManager.AddToRoleAsync(
                usuario,
                model.Rol);

            return RedirectToAction(nameof(Create));
        }
    }
}