using Layout.Data;
using Layout.Models;
using Layout.Models.Enums;
using Layout.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Layout.Controllers
{
    [Authorize]
    public class FirmasController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FirmasController(
            AppDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        [Authorize]
        public async Task<IActionResult> Pendientes()
        {
            var usuario = await _userManager.GetUserAsync(User);

            var vm = new MisFirmasViewModel();

            var tiposPA = await _context.FirmasPA
                .Where(x =>
                    x.UsuarioPAId == usuario.Id &&
                    x.Activo)
                .Select(x => x.TipoFirmaId)
                .ToListAsync();

            var pendientes = await _context.SolicitudesFirma
                 .Include(x => x.Solicitud)
                     .ThenInclude(x => x.Area)
                 .Include(x => x.TipoFirma)
                 .Where(x =>
                     !x.Firmada &&
                     (
                         x.UsuarioRequeridoId == usuario.Id
                         ||
                         tiposPA.Contains(x.TipoFirmaId)
                     ))
                 .ToListAsync();

            vm.PuedeAsignarPA = await _context.UsuarioTiposFirma
                .AnyAsync(x => x.UsuarioId == usuario.Id);


            vm.SoyPADe = await _context.FirmasPA
                .Include(x => x.TipoFirma)
                .Include(x => x.UsuarioTitular)
                .Where(x =>
                    x.UsuarioPAId == usuario.Id &&
                    x.Activo)
                .Select(x => new FirmaPAViewModel
                {
                    TipoFirma = x.TipoFirma.Nombre,
                    UsuarioPA = x.UsuarioTitular.NombreCompleto,
                    Activo = x.Activo
                })
                .ToListAsync();

            vm.PendientesAgrupadas = pendientes
            .GroupBy(x => x.SolicitudId)
            .Select(g => new PendienteFirmaAgrupadaViewModel
            {
                FirmaIdReferencia = g.First().Id,

                SolicitudId = g.Key,

                Folio = g.First().Solicitud.Folio,

                NumeroValidacion =
                    g.First().Solicitud.NumeroValidacion,

                Area = g.First().Solicitud.Area.Nombre,

                FechaCreacion = g.First().Solicitud.FechaCreacion,

                TiposFirma = g
                    .Select(x => x.TipoFirma.Nombre)
                    .ToList()
            })
            .ToList();


            var realizadas = await _context.SolicitudesFirma
                .Include(x => x.Solicitud)
                    .ThenInclude(x => x.Area)
                .Include(x => x.TipoFirma)
                .Where(x =>

                    x.Firmada &&

                    (
                        x.UsuarioFirmanteId == usuario.Id
                        ||
                        x.UsuarioPAId == usuario.Id
                    )
                )
                .ToListAsync();

            vm.RealizadasAgrupadas = realizadas
                .GroupBy(x => x.SolicitudId)
                .Select(g => new FirmaRealizadaAgrupadaViewModel
                {
                    FirmaIdReferencia = g.First().Id,

                    SolicitudId = g.Key,

                    Folio = g.First().Solicitud.Folio,

                    Area = g.First().Solicitud.Area.Nombre,

                    FechaFirma = g.Max(x => x.FechaFirma),

                    TiposFirma = g
                        .Select(x => x.TipoFirma.Nombre)
                        .Distinct()
                        .ToList()
                })
                .OrderByDescending(x => x.FechaFirma)
                .ToList();

            vm.FirmasPA = await _context.FirmasPA
                .Include(x => x.TipoFirma)
                .Include(x => x.UsuarioPA)
                .Where(x => x.UsuarioTitularId == usuario.Id)
                .Select(x => new FirmaPAViewModel
                {
                    TipoFirmaId = x.TipoFirmaId,
                    TipoFirma = x.TipoFirma.Nombre,
                    UsuarioPAId = x.UsuarioPAId,
                    UsuarioPA = x.UsuarioPA.UserName,
                    Activo = x.Activo
                })
                .ToListAsync(); 

            return View(vm);
        }

        [Authorize]
        public async Task<IActionResult> Detalle(int id)
        {
            var usuario = await _userManager.GetUserAsync(User);

            var firma = await _context.SolicitudesFirma
                .Include(x => x.TipoFirma)
                .Include(x => x.UsuarioFirmante)
                .Include(x => x.UsuarioRequerido)
                .Include(x => x.Solicitud)
                    .ThenInclude(x => x.Area)
                .Include(x => x.Solicitud)
                    .ThenInclude(x => x.MovimientosTecnicos)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (firma == null)
                return NotFound();

            bool esPA = await _context.FirmasPA
                .AnyAsync(x =>
                    x.UsuarioPAId == usuario.Id &&
                    x.TipoFirmaId == firma.TipoFirmaId &&
                    x.Activo);

            if (firma.UsuarioRequeridoId != usuario.Id && !esPA)
                return Forbid();

            ViewBag.EsPA = esPA;

            FirmaPA? registroPA = null;

            if (esPA)
            {
                registroPA = await _context.FirmasPA
                    .Include(x => x.UsuarioTitular)
                    .FirstOrDefaultAsync(x =>
                        x.UsuarioPAId == usuario.Id &&
                        x.TipoFirmaId == firma.TipoFirmaId &&
                        x.Activo);

                ViewBag.NombreTitular =
                    registroPA?.UsuarioTitular?.NombreCompleto;

                ViewBag.MotivoAsignacionPA =
                    registroPA?.MotivoAsignacion;
            }

            ViewBag.Firmas = await _context.SolicitudesFirma
                .Include(x => x.TipoFirma)
                .Include(x => x.UsuarioRequerido)
                .Include(x => x.UsuarioFirmante)
                .Where(x => x.SolicitudId == firma.SolicitudId)
                .ToListAsync();

            return View(firma);
        }

        private async Task ValidarFinalizacionSolicitud(int solicitudId)
        {
            bool existenPendientes =
                await _context.SolicitudesFirma
                    .AnyAsync(x =>
                        x.SolicitudId == solicitudId &&
                        !x.Firmada);

            if (!existenPendientes)
            {
                var solicitud =
                    await _context.SolicitudesMovimiento
                        .FirstOrDefaultAsync(x =>
                            x.Id == solicitudId);

                if (solicitud != null)
                {
                    solicitud.Estatus =
                        EstatusSolicitud.Aprobado;

                    await _context.SaveChangesAsync();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> FirmarSolicitud(int id, string? motivoPA)
        {
            var usuario =
                await _userManager.GetUserAsync(User);

            var firmaBase =
                await _context.SolicitudesFirma
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (firmaBase == null)
                return NotFound();

            bool esPA = await _context.FirmasPA
                .AnyAsync(x =>
                    x.UsuarioPAId == usuario.Id &&
                    x.TipoFirmaId == firmaBase.TipoFirmaId &&
                    x.Activo);

            if (firmaBase.UsuarioRequeridoId != usuario.Id &&
                !esPA)
            {
                return Forbid();
            }

            var firmasPendientes =
                await _context.SolicitudesFirma
                    .Where(x =>
                        x.SolicitudId == firmaBase.SolicitudId &&
                        x.TipoFirmaId == firmaBase.TipoFirmaId &&
                        !x.Firmada)
                    .ToListAsync();

            foreach (var firma in firmasPendientes)
            {
                firma.Firmada = true;

                firma.UsuarioFirmanteId =
                    usuario.Id;

                firma.FechaFirma =
                    DateTime.Now;

                if (esPA)
                {
                    firma.EsFirmaPA = true;

                    firma.UsuarioPAId =
                        usuario.Id;

                    firma.MotivoFirmaPA =
                        motivoPA;
                }
            }

            await _context.SaveChangesAsync();

            await ValidarFinalizacionSolicitud(
                firmaBase.SolicitudId);

            TempData["Success"] =
                esPA
                ? $"Se registraron {firmasPendientes.Count} firma(s) como PA."
                : $"Se registraron {firmasPendientes.Count} firma(s).";

            return RedirectToAction(nameof(Pendientes));
        }

        [HttpPost]
        public async Task<IActionResult> AsignarPA(int tipoFirmaId, string usuarioPAId, string motivo)
        {
            var usuarioActual =
                await _userManager.GetUserAsync(User);

            var existente = await _context.FirmasPA
                .FirstOrDefaultAsync(x =>
                    x.UsuarioTitularId == usuarioActual.Id &&
                    x.TipoFirmaId == tipoFirmaId);

            if (existente != null)
            {
                existente.UsuarioPAId = usuarioPAId;
                existente.MotivoAsignacion = motivo;
                existente.Activo = true;
                existente.FechaAsignacion = DateTime.Now;
            }
            else
            {
                _context.FirmasPA.Add(
                    new FirmaPA
                    {
                        UsuarioTitularId = usuarioActual.Id,
                        UsuarioPAId = usuarioPAId,
                        TipoFirmaId = tipoFirmaId,
                        MotivoAsignacion = motivo,
                        Activo = true,
                        FechaAsignacion = DateTime.Now
                    });
            }

            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true
            });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerDatosPA()
        {
            var usuario =
                await _userManager.GetUserAsync(User);

            var tiposFirma =
                await _context.UsuarioTiposFirma
                .Include(x => x.TipoFirma)
                .Where(x => x.UsuarioId == usuario.Id)
                .Select(x => new
                {
                    id = x.TipoFirmaId,
                    texto = x.TipoFirma.Nombre
                })
                .ToListAsync();

            var usuarios =
                await _userManager.Users
                .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    texto = x.UserName
                })
                .ToListAsync();

            return Json(new
            {
                tiposFirma,
                usuarios
            });
        }

    }
}
