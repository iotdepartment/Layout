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

            var pendientes = await _context.SolicitudesFirma
             .Include(x => x.Solicitud)
                .ThenInclude(x => x.Area)
             .Include(x => x.TipoFirma)
             .Where(x =>
                 x.UsuarioRequeridoId == usuario.Id &&
                 !x.Firmada)
             .ToListAsync();

            vm.PendientesAgrupadas = pendientes
               .GroupBy(x => x.SolicitudId)
               .Select(g => new PendienteFirmaAgrupadaViewModel
               {
                   FirmaIdReferencia = g.First().Id,

                   SolicitudId = g.Key,

                   Folio = g.First().Solicitud.Folio,

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
                    x.UsuarioRequeridoId == usuario.Id &&
                    x.Firmada)
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

            if (firma.UsuarioRequeridoId != usuario.Id)
                return Forbid();

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
        public async Task<IActionResult> Firmar(
       int firmaId,
       string? comentarios)
        {
            var usuario = await _userManager.GetUserAsync(User);

            var firmaBase = await _context.SolicitudesFirma
                .FirstOrDefaultAsync(x => x.Id == firmaId);

            if (firmaBase == null)
                return NotFound();

            if (firmaBase.UsuarioRequeridoId != usuario.Id)
                return Forbid();

            var firmasPendientes = await _context.SolicitudesFirma
                .Where(x =>
                    x.SolicitudId == firmaBase.SolicitudId &&
                    x.UsuarioRequeridoId == usuario.Id &&
                    !x.Firmada)
                .ToListAsync();

            foreach (var firma in firmasPendientes)
            {
                firma.Firmada = true;

                firma.UsuarioFirmanteId = usuario.Id;

                firma.FechaFirma = DateTime.Now;

                firma.Comentarios = comentarios;
            }

            await _context.SaveChangesAsync();

            await ValidarFinalizacionSolicitud(
                firmaBase.SolicitudId);

            TempData["Success"] =
                $"Se registraron {firmasPendientes.Count} firma(s).";

            return RedirectToAction(nameof(Pendientes));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> FirmarSolicitud(int id)
        {
            var usuario = await _userManager.GetUserAsync(User);

            var firmaBase = await _context.SolicitudesFirma
                .FirstOrDefaultAsync(x => x.Id == id);

            if (firmaBase == null)
                return NotFound();

            if (firmaBase.UsuarioRequeridoId != usuario.Id)
                return Forbid();

            var firmasPendientes = await _context.SolicitudesFirma
                .Where(x =>
                    x.SolicitudId == firmaBase.SolicitudId &&
                    x.UsuarioRequeridoId == usuario.Id &&
                    !x.Firmada)
                .ToListAsync();

            foreach (var firma in firmasPendientes)
            {
                firma.Firmada = true;
                firma.UsuarioFirmanteId = usuario.Id;
                firma.FechaFirma = DateTime.Now;
            }

            await _context.SaveChangesAsync();

            await ValidarFinalizacionSolicitud(
                firmaBase.SolicitudId);

            TempData["Success"] =
                $"Se registraron {firmasPendientes.Count} firma(s).";

            return RedirectToAction(nameof(Pendientes));
        }
    }
}
