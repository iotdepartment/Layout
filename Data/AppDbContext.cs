using Layout.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Layout.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Area> Areas { get; set; }

        public DbSet<AreaOrganizacional> AreasOrganizacionales { get; set; }

        public DbSet<ResponsableFirma> ResponsablesFirma { get; set; }

        public DbSet<UsuarioArea> UsuarioAreas { get; set; }
        public DbSet<UsuarioTipoFirma> UsuarioTiposFirma { get; set; }

        public DbSet<SolicitudMovimiento> SolicitudesMovimiento { get; set; }

        public DbSet<SolicitudAprobacionDetalle> SolicitudesAprobacion { get; set; }

        public DbSet<SolicitudHistorial> SolicitudesHistorial { get; set; }

        public DbSet<SolicitudInventarioTemporal> SolicitudesInventario { get; set; }

        public DbSet<TipoFirma> TiposFirma { get; set; }

        public DbSet<SolicitudFirma> SolicitudesFirma { get; set; }

        public DbSet<SolicitudMovimientosTecnicos> SolicitudesMovimientosTecnicos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // =====================================================
            // USUARIO - TIPO FIRMA
            // =====================================================

            builder.Entity<UsuarioTipoFirma>()
                .HasOne(x => x.Usuario)
                .WithMany(x => x.TiposFirma)
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<UsuarioTipoFirma>()
                .HasOne(x => x.TipoFirma)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.TipoFirmaId)
                .OnDelete(DeleteBehavior.Cascade);

            // =====================================================
            // USUARIO - AREA (M:N)
            // =====================================================

            builder.Entity<UsuarioArea>()
                .HasKey(x => new
                {
                    x.UsuarioId,
                    x.AreaId
                });

            builder.Entity<UsuarioArea>()
                .HasOne(x => x.Usuario)
                .WithMany(x => x.Areas)
                .HasForeignKey(x => x.UsuarioId);

            builder.Entity<UsuarioArea>()
                .HasOne(x => x.Area)
                .WithMany(x => x.Usuarios)
                .HasForeignKey(x => x.AreaId);

            // =====================================================
            // SOLICITUD - USUARIO SOLICITANTE
            // =====================================================

            builder.Entity<SolicitudMovimiento>()
                .HasOne(s => s.UsuarioSolicitante)
                .WithMany()
                .HasForeignKey(s => s.UsuarioSolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            // =====================================================
            // SOLICITUD - USUARIO APROBADOR
            // =====================================================

            builder.Entity<SolicitudMovimiento>()
                .HasOne(s => s.UsuarioAprobador)
                .WithMany()
                .HasForeignKey(s => s.UsuarioAprobadorId)
                .OnDelete(DeleteBehavior.Restrict);

            // =====================================================
            // DETALLE APROBACIÓN 1:1
            // =====================================================

            builder.Entity<SolicitudAprobacionDetalle>()
                .HasOne(d => d.SolicitudMovimiento)
                .WithOne(s => s.DetalleAprobacion)
                .HasForeignKey<SolicitudAprobacionDetalle>(
                    d => d.SolicitudMovimientoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SolicitudAprobacionDetalle>()
                .HasOne(d => d.UsuarioEjecutor)
                .WithMany()
                .HasForeignKey(d => d.UsuarioEjecutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // =====================================================
            // INVENTARIO TEMPORAL
            // =====================================================

            builder.Entity<SolicitudInventarioTemporal>()
                .HasOne(i => i.Solicitud)
                .WithOne(s => s.InventarioTemporal)
                .HasForeignKey<SolicitudInventarioTemporal>(
                    i => i.SolicitudId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SolicitudInventarioTemporal>()
                .HasIndex(i => i.SolicitudId)
                .IsUnique();

            // =====================================================
            // FOLIO ÚNICO
            // =====================================================

            builder.Entity<SolicitudMovimiento>()
                .HasIndex(s => s.Folio)
                .IsUnique();

            // =====================================================
            // MOVIMIENTOS TÉCNICOS 1:1
            // =====================================================

            builder.Entity<SolicitudMovimientosTecnicos>()
                .HasOne(t => t.Solicitud)
                .WithOne(s => s.MovimientosTecnicos)
                .HasForeignKey<SolicitudMovimientosTecnicos>(
                    t => t.SolicitudId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SolicitudMovimientosTecnicos>()
                .HasIndex(t => t.SolicitudId)
                .IsUnique();

            // =====================================================
            // SOLICITUDES FIRMA
            // =====================================================

            builder.Entity<SolicitudFirma>()
                .HasOne(s => s.Solicitud)
                .WithMany()
                .HasForeignKey(s => s.SolicitudId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SolicitudFirma>()
                .HasOne(x => x.UsuarioRequerido)
                .WithMany()
                .HasForeignKey(x => x.UsuarioRequeridoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SolicitudFirma>()
                .HasOne(s => s.TipoFirma)
                .WithMany()
                .HasForeignKey(s => s.TipoFirmaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SolicitudFirma>()
                .HasOne(s => s.UsuarioFirmante)
                .WithMany()
                .HasForeignKey(s => s.UsuarioFirmanteId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}