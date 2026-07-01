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
                    Estatus = EstatusSolicitud.Pendiente,
                    FechaCreacion = DateTime.Now,

                    // ✅ valor temporal
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

                    AplicaFecha = model.AplicaFecha,
                    FechaCompromiso = model.AplicaFecha ? model.FechaCompromiso : null
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
            // Obtenemos las solicitudes incluyendo los datos del Área relacionada
            // Puedes ordenar por FechaCreacion para ver las más recientes primero
            var solicitudes = await _context.SolicitudesMovimiento
                .Include(s => s.Area)
                .Include(s => s.InventarioTemporal)
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();

            return View(solicitudes);
        }

        // VALIDA SI LA SOLICITUD ES ACEPTADA O RECHAZADA POR EL USUARIO APROBADOR O ADMINISTRADOR
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Aprobador,Administrador")]
        public async Task<IActionResult> Evaluar(int id, EstatusSolicitud nuevoEstatus, string comentarios)
        {
            var solicitud = await _context.SolicitudesMovimiento.FindAsync(id);

            if (solicitud == null)
            {
                return Json(new { success = false, message = "La solicitud no existe." });
            }

            var user = await _userManager.GetUserAsync(User);

            // ✅ REGLA DE NEGOCIO: Si el cliente aprueba, forzamos que el estatus real sea "EnProceso"
            // para que la vista entienda que tiene un formulario técnico pendiente de llenar.
            if (nuevoEstatus == EstatusSolicitud.Aprobado)
            {
                solicitud.Estatus = EstatusSolicitud.EnProceso;
            }
            else
            {
                solicitud.Estatus = nuevoEstatus;
            }

            solicitud.UsuarioAprobadorId = user.Id;
            solicitud.FechaRevision = DateTime.Now;
            solicitud.ComentariosRevision = string.IsNullOrWhiteSpace(comentarios)
                ? "Sin comentarios adicionales"
                : comentarios;

            await _context.SaveChangesAsync();

            // ✅ 🔥 SI ERA APROBACIÓN → REDIRIGIR AL FORMULARIO EXTRA
            // Evaluamos contra 'nuevoEstatus' porque es lo que mandó el cliente originalmente desde la interfaz
            if (nuevoEstatus == EstatusSolicitud.Aprobado)
            {
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("CompletarInventario", "Solicitudes", new { id = solicitud.Id })
                });
            }

            // ✅ SI RECHAZA (O CUALQUIER OTRO) → RESPUESTA NORMAL
            return Json(new
            {
                success = true,
                message = "Solicitud procesada correctamente"
            });
        }

        // MUESTRA LA VISTA DEL FORMULARIO PARA LLENAR LA INFORMACIÓN EXTRA
        [Authorize(Roles = "Aprobador,Administrador")]
        public async Task<IActionResult> CompletarInventario(int id)
        {
            var solicitud = await _context.SolicitudesMovimiento
                .Include(s => s.MovimientosTecnicos)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (solicitud == null)
                return NotFound();

            // 🔒 SOLO SI ESTÁ EN PROCESO
            if (solicitud.Estatus != EstatusSolicitud.EnProceso)
                return RedirectToAction("Index");

            var vm = new SolicitudInventarioViewModel
            {
                SolicitudId = id
            };

            // ✅ SOLO TÉCNICOS
            if (solicitud.MovimientosTecnicos != null)
            {
                var tech = solicitud.MovimientosTecnicos;

                vm.MovimientoRed = tech.MovimientoRed;
                vm.MovimientoIoT = tech.MovimientoIoT;
                vm.MovimientoProgramacion = tech.MovimientoProgramacion;
                vm.MovimientoElectrico = tech.MovimientoElectrico;
                vm.MovimientoEHS = tech.MovimientoEHS;
                vm.CambioNomenclatura = tech.CambioNomenclatura;
                vm.RequierePCR = tech.RequierePCR;
            }

            return View(vm);
        }

        // COMPLETA LA INFORMACIÓN UNA VEZ CONFIRMADA/ACEPTADA LA SOLICITUD DE MOVIMIENTO DE LAYOUT PARA EL FORMULARIO DE INFORMACIÓN
        [HttpPost]
        [Authorize(Roles = "Aprobador,Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletarInventario(SolicitudInventarioViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // =====================================================
            // ✅ 🔥 MOVIMIENTOS TÉCNICOS (ÚNICOQUE SE GUARDA)
            // =====================================================

            var tecnicoExistente = await _context.Set<SolicitudMovimientosTecnicos>()
                .FirstOrDefaultAsync(x => x.SolicitudId == model.SolicitudId);

            if (tecnicoExistente != null)
            {
                // UPDATE
                tecnicoExistente.MovimientoRed = model.MovimientoRed;
                tecnicoExistente.MovimientoIoT = model.MovimientoIoT;
                tecnicoExistente.MovimientoProgramacion = model.MovimientoProgramacion;
                tecnicoExistente.MovimientoElectrico = model.MovimientoElectrico;
                tecnicoExistente.MovimientoEHS = model.MovimientoEHS;
                tecnicoExistente.CambioNomenclatura = model.CambioNomenclatura;
                tecnicoExistente.RequierePCR = model.RequierePCR;
            }
            else
            {
                // INSERT
                var tecnico = new SolicitudMovimientosTecnicos
                {
                    SolicitudId = model.SolicitudId,
                    MovimientoRed = model.MovimientoRed,
                    MovimientoIoT = model.MovimientoIoT,
                    MovimientoProgramacion = model.MovimientoProgramacion,
                    MovimientoElectrico = model.MovimientoElectrico,
                    MovimientoEHS = model.MovimientoEHS,
                    CambioNomenclatura = model.CambioNomenclatura,
                    RequierePCR = model.RequierePCR
                };

                _context.Add(tecnico);
            }

            // =====================================================
            // ✅ 🔥 FINALIZAR SOLICITUD
            // =====================================================

            var solicitudPrincipal = await _context.SolicitudesMovimiento
                .FindAsync(model.SolicitudId);

            if (solicitudPrincipal != null)
            {
                solicitudPrincipal.Estatus = EstatusSolicitud.Finalizado;
            }

            // =====================================================
            // ✅ GUARDAR
            // =====================================================

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}