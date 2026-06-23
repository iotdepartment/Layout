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

        // ✅ GET
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

        // ✅ POST (Modificado para responder peticiones AJAX / FETCH)
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

                string imageBase64 = null;

                // 💡 LA CLAVE: Convertimos la imagen a texto en memoria sin tocar el disco del servidor
                if (model.Imagen != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        await model.Imagen.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();

                        // Obtenemos el tipo de formato (ej: image/png, image/jpeg)
                        string mimeType = model.Imagen.ContentType;

                        // Creamos la cadena Base64 que se guardará en la base de datos
                        imageBase64 = $"data:{mimeType};base64,{Convert.ToBase64String(fileBytes)}";
                    }
                }

                var solicitud = new SolicitudMovimiento
                {
                    AreaId = model.AreaId,
                    TipoMovimiento = model.TipoMovimiento,
                    Descripcion = model.Descripcion,
                    Razon = model.Razon,
                    ImagenLayout = imageBase64, // 👈 Guardamos el texto en lugar de la ruta física
                    UsuarioSolicitanteId = user.Id,
                    Estatus = EstatusSolicitud.Pendiente,
                    FechaCreacion = DateTime.Now
                };

                _context.SolicitudesMovimiento.Add(solicitud);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Solicitud guardada con éxito", redirectUrl = Url.Action("Index", "Home") });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error en el servidor: " + ex.Message });
            }

        }


        // ✅ GET: Historial de Solicitudes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Obtenemos las solicitudes incluyendo los datos del Área relacionada
            // Puedes ordenar por FechaCreacion para ver las más recientes primero
            var solicitudes = await _context.SolicitudesMovimiento
                .Include(s => s.Area)
                .OrderByDescending(s => s.FechaCreacion)
                .ToListAsync();

            return View(solicitudes);
        }

        
        // ✅ POST: Solicitudes/Evaluar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Evaluar(int id, EstatusSolicitud nuevoEstatus, string comentarios)
        {
            var solicitud = await _context.SolicitudesMovimiento.FindAsync(id);
            if (solicitud == null)
            {
                return Json(new { success = false, message = "La solicitud no existe." });
            }

            // Obtener el usuario administrador que está logueado
            var user = await _userManager.GetUserAsync(User);

            // Actualizar campos de la entidad
            solicitud.Estatus = nuevoEstatus;
            solicitud.UsuarioAprobadorId = user.Id;
            solicitud.FechaRevision = DateTime.Now;
            solicitud.ComentariosRevision = string.IsNullOrWhiteSpace(comentarios) ? "Sin comentarios adicionales" : comentarios;

            _context.SolicitudesMovimiento.Update(solicitud);
            await _context.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = $"Solicitud {(nuevoEstatus == EstatusSolicitud.Aprobado ? "aprobada" : "rechazada")} con éxito."
            });
        }


    }
}