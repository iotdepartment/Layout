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
    //[Authorize(Roles = "Usuario")]
    public class SolicitudesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public SolicitudesController(AppDbContext context,
                                     UserManager<ApplicationUser> userManager,
                                     IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // METODO PARA LA VISTA DE CREAR
        [HttpGet]
        public IActionResult Create()
        {
            var vm = new SolicitudCreateViewModel
            {
                Areas = _context.Areas
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = a.Nombre
                    }).ToList()
            };

            return View(vm);
        }

        // CREAR EL REGISTRO DE LAS NUEVAS SOLICITUDES DE MOVIMIENTOS DE LAYOUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SolicitudCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errores = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return Json(new { success = false, message = "Datos inválidos", errors = errores });
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(new { success = false, message = "Error de sesión: El usuario no está autenticado." });
                }

                string imagePath = null;

                if (model.Imagen != null)
                {
                    var folder = Path.Combine(_env.WebRootPath, "uploads");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Imagen.FileName);
                    var fullPath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.Imagen.CopyToAsync(stream);
                    }

                    imagePath = "/uploads/" + fileName;
                }

                var solicitud = new SolicitudMovimiento
                {
                    AreaId = model.AreaId,
                    TipoMovimiento = model.TipoMovimiento,
                    Descripcion = model.Descripcion,
                    Razon = model.Razon,
                    ImagenLayout = imagePath,
                    UsuarioSolicitanteId = user.Id,

                    FechaInicioMovimiento = model.FechaInicioMovimiento,
                    FechaFinMovimiento = model.FechaFinMovimiento,

                    Estatus = EstatusSolicitud.Pendiente,
                    FechaCreacion = DateTime.Now,
                    Folio = $"NM-TEMP-{Guid.NewGuid()}"
                };

                _context.SolicitudesMovimiento.Add(solicitud);
                await _context.SaveChangesAsync();

                // ✅ generar folio real
                solicitud.Folio = $"NM-{solicitud.Id.ToString("D6")}";

                await _context.SaveChangesAsync();

                // =============================
                // ✅ GUARDAR INVENTARIO
                // =============================

                var inventario = new SolicitudInventarioTemporal
                {
                    SolicitudId = solicitud.Id,

                    AplicaValidacion = model.AplicaValidacion,
                    NumeroValidacion = model.AplicaValidacion ? model.NumeroValidacion : null,

                    AplicaResponsable = model.AplicaResponsable,
                    ResponsableInventario = model.AplicaResponsable ? model.ResponsableInventario : null,

                    AplicaMandril = model.AplicaMandril,
                    MandrilKanbanNP = model.AplicaMandril ? model.MandrilKanbanNP : null,

                    AplicaPallets = model.AplicaPallets,
                    NumeroPallets = model.AplicaPallets ? model.NumeroPallets : null,

                    AplicaRazon = model.AplicaRazonInventario,
                    RazonInventario = model.AplicaRazonInventario ? model.RazonInventario : null,
                };

                _context.SolicitudesInventario.Add(inventario);

                await _context.SaveChangesAsync();


                return Json(new
                {
                    success = true,
                    message = "Solicitud guardada con éxito",
                    redirectUrl = Url.Action("Index", "Home")
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // METODO PARA MOSTRAR LA LISTA DE LAS SOLICITUDES REALIZADAS
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var solicitudes = await _context.SolicitudesMovimiento
                .Include(s => s.Area)
                .Include(s => s.InventarioTemporal)
                .Include(s => s.UsuarioAprobador)
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();

            return View(solicitudes);
        }

        [Authorize(Roles = "Aprobador,Administrador")]
        public async Task<IActionResult> CompletarInventario(int id)
        {
            var solicitud = await _context.SolicitudesMovimiento
                .Include(s => s.MovimientosTecnicos)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (solicitud == null)
                return NotFound();

            // Solo las solicitudes EnProceso pueden llenarse
            if (solicitud.Estatus != EstatusSolicitud.EnProceso)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.TiposFirma = await _context.TiposFirma
                .OrderBy(x => x.Nombre)
                .ToListAsync();

            ViewBag.Usuarios = await _userManager.Users
                .OrderBy(x => x.NombreCompleto)
                .ToListAsync();

            var vm = new SolicitudInventarioViewModel
            {
                SolicitudId = solicitud.Id
            };

            if (solicitud.MovimientosTecnicos != null)
            {
                vm.MovimientoITIoT = solicitud.MovimientosTecnicos.MovimientoITIoT;
                vm.MovimientoProgramacion = solicitud.MovimientosTecnicos.MovimientoProgramacion;
                vm.MovimientoElectrico = solicitud.MovimientosTecnicos.MovimientoElectrico;
                vm.MovimientoEHS = solicitud.MovimientosTecnicos.MovimientoEHS;
                vm.CambioNomenclatura = solicitud.MovimientosTecnicos.CambioNomenclatura;
                vm.RequierePCR = solicitud.MovimientosTecnicos.RequierePCR;
                vm.NumeroPCR = solicitud.MovimientosTecnicos.NumeroPCR;
            }

            return View(vm);
        }

        // VALIDA SI LA SOLICITUD ES ACEPTADA O RECHAZADA POR EL USUARIO APROBADOR O ADMINISTRADOR
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Aprobador,Administrador")]
        public async Task<IActionResult> Evaluar(int id, EstatusSolicitud nuevoEstatus, string comentarios)
        {
            var solicitud = await _context.SolicitudesMovimiento
                .FindAsync(id);

            if (solicitud == null)
            {
                return Json(new
                {
                    success = false,
                    message = "La solicitud no existe."
                });
            }

            var user = await _userManager.GetUserAsync(User);

            solicitud.Estatus = nuevoEstatus;

            solicitud.UsuarioAprobadorId = user.Id;
            solicitud.FechaRevision = DateTime.Now;
            solicitud.ComentariosRevision =
                string.IsNullOrWhiteSpace(comentarios)
                    ? "Sin comentarios adicionales"
                    : comentarios;

            await _context.SaveChangesAsync();

            // Si se envió a EnProceso
            if (nuevoEstatus == EstatusSolicitud.EnProceso)
            {
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action(
                        "CompletarInventario",
                        "Solicitudes",
                        new { id = solicitud.Id })
                });
            }

            return Json(new
            {
                success = true,
                message = "Solicitud procesada correctamente."
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletarInventario(SolicitudInventarioViewModel model)
        {
            // Validar usuarios duplicados
            if (model.Firmas != null)
            {
                var usuariosDuplicados = model.Firmas
                    .Where(x => !string.IsNullOrWhiteSpace(x.UsuarioRequeridoId))
                    .GroupBy(x => x.UsuarioRequeridoId)
                    .Any(x => x.Count() > 1);

                if (usuariosDuplicados)
                {
                    ModelState.AddModelError(
                        "",
                        "No puede seleccionar el mismo usuario más de una vez en las firmas requeridas."
                    );

                    ViewBag.TiposFirma = await _context.TiposFirma
                        .OrderBy(x => x.Nombre)
                        .ToListAsync();

                    ViewBag.Usuarios = await _userManager.Users
                        .OrderBy(x => x.NombreCompleto)
                        .ToListAsync();

                    return View(model);
                }
            }

            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value.Errors.Any())
                    .Select(x => new
                    {
                        Campo = x.Key,
                        Errores = x.Value.Errors
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    })
                    .ToList();

                ViewBag.TiposFirma = await _context.TiposFirma
                    .OrderBy(x => x.Nombre)
                    .ToListAsync();

                ViewBag.Usuarios = await _userManager.Users
                    .OrderBy(x => x.NombreCompleto)
                    .ToListAsync();

                return Json(errores);
            }

            // Buscar registro existente
            var movimiento = await _context.SolicitudesMovimientosTecnicos
                .FirstOrDefaultAsync(x =>
                    x.SolicitudId == model.SolicitudId);

            if (movimiento == null)
            {
                movimiento = new SolicitudMovimientosTecnicos
                {
                    SolicitudId = model.SolicitudId
                };

                _context.SolicitudesMovimientosTecnicos.Add(movimiento);
            }

            // Actualizar información
            movimiento.MovimientoITIoT = model.MovimientoITIoT;
            movimiento.MovimientoProgramacion = model.MovimientoProgramacion;
            movimiento.MovimientoElectrico = model.MovimientoElectrico;
            movimiento.MovimientoEHS = model.MovimientoEHS;
            movimiento.CambioNomenclatura = model.CambioNomenclatura;

            movimiento.RequierePCR = model.RequierePCR;
            movimiento.NumeroPCR = model.NumeroPCR;

            // Eliminar firmas existentes
            var firmasExistentes = await _context.SolicitudesFirma
                .Where(x => x.SolicitudId == model.SolicitudId)
                .ToListAsync();

            _context.SolicitudesFirma.RemoveRange(firmasExistentes);

            // Guardar nuevas firmas
            if (model.Firmas != null)
            {
                foreach (var firma in model.Firmas)
                {
                    if (
                        firma.TipoFirmaId.HasValue &&
                        !string.IsNullOrWhiteSpace(firma.UsuarioRequeridoId)
                    )
                    {
                        _context.SolicitudesFirma.Add(
                            new SolicitudFirma
                            {
                                SolicitudId = model.SolicitudId,
                                TipoFirmaId = firma.TipoFirmaId.Value,
                                UsuarioRequeridoId = firma.UsuarioRequeridoId,
                                Firmada = false
                            });
                    }
                }
            }

            // Cambiar estatus
            var solicitud = await _context.SolicitudesMovimiento
                .FirstOrDefaultAsync(x => x.Id == model.SolicitudId);

            if (solicitud != null)
            {
                solicitud.Estatus = EstatusSolicitud.PendienteFirmas;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] =
                "Información técnica y firmas guardadas correctamente.";

            return RedirectToAction(nameof(Index));
        }

        private async Task<List<ApplicationUser>> ObtenerUsuariosPorRolYArea(string rol, int areaId)
        {
            var usuarios = await _context.UsuarioAreas
                .Include(x => x.Usuario)
                .Where(x => x.AreaId == areaId)
                .Select(x => x.Usuario)
                .ToListAsync();

            var resultado = new List<ApplicationUser>();

            foreach (var usuario in usuarios)
            {
                if (await _userManager.IsInRoleAsync(
                    usuario,
                    rol))
                {
                    resultado.Add(usuario);
                }
            }

            return resultado;
        }

        private async Task<List<ApplicationUser>> ObtenerUsuariosPorRol(string rol)
        {
            var usuarios = await _userManager.Users
                .OrderBy(x => x.NombreCompleto)
                .ToListAsync();

            var resultado = new List<ApplicationUser>();

            foreach (var usuario in usuarios)
            {
                if (await _userManager.IsInRoleAsync(usuario, rol))
                {
                    resultado.Add(usuario);
                }
            }

            return resultado;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerUsuariosFirma(
    int tipoFirmaId)
        {
            var usuarios = await _userManager.Users
                .Where(x =>
                    x.Activo &&
                    x.TipoFirmaId == tipoFirmaId)
                .OrderBy(x => x.NombreCompleto)
                .Select(x => new
                {
                    id = x.Id,
                    nombre = x.NombreCompleto
                })
                .ToListAsync();

            return Json(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerFirmas(int solicitudId)
        {
            var firmas = await _context.SolicitudesFirma
                .Include(x => x.TipoFirma)
                .Include(x => x.UsuarioRequerido)
                .Include(x => x.UsuarioFirmante)
                .Where(x => x.SolicitudId == solicitudId)
                .OrderBy(x => x.TipoFirma.Nombre)
                .Select(x => new
                {
                    tipoFirma = x.TipoFirma.Nombre,
                    firmada = x.Firmada,
                    usuarioRequerido = x.UsuarioRequerido.NombreCompleto,
                    usuarioFirmante = x.UsuarioFirmante != null
                        ? x.UsuarioFirmante.NombreCompleto
                        : null,
                    fechaFirma = x.FechaFirma.HasValue
                        ? x.FechaFirma.Value.ToString("dd/MM/yyyy HH:mm")
                        : ""
                })
                .ToListAsync();

            return Json(firmas);
        }



    }
}