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
        public DbSet<SolicitudMovimiento> SolicitudesMovimiento { get; set; }
        public DbSet<SolicitudAprobacionDetalle> SolicitudesAprobacion { get; set; }
        public DbSet<SolicitudHistorial> SolicitudesHistorial { get; set; }
        public DbSet<SolicitudInventarioTemporal> SolicitudesInventario { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ✅ Usuario solicitante (sin cascade delete)
            builder.Entity<SolicitudMovimiento>()
                .HasOne(s => s.UsuarioSolicitante)
                .WithMany()
                .HasForeignKey(s => s.UsuarioSolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Usuario aprobador (sin cascade delete)
            builder.Entity<SolicitudMovimiento>()
                .HasOne(s => s.UsuarioAprobador)
                .WithMany()
                .HasForeignKey(s => s.UsuarioAprobadorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ Relación 1:1 AprobaciónDetalle
            builder.Entity<SolicitudAprobacionDetalle>()
                .HasOne(d => d.SolicitudMovimiento)
                .WithOne(s => s.DetalleAprobacion)
                .HasForeignKey<SolicitudAprobacionDetalle>(d => d.SolicitudMovimientoId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SolicitudAprobacionDetalle>()
                .HasOne(d => d.UsuarioEjecutor)
                .WithMany()
                .HasForeignKey(d => d.UsuarioEjecutorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ✅ ✅ RELACIÓN 1:1 InventarioTemporal (CLAVE)
            builder.Entity<SolicitudInventarioTemporal>()
                .HasOne(i => i.Solicitud)
                .WithOne(s => s.InventarioTemporal)
                .HasForeignKey<SolicitudInventarioTemporal>(i => i.SolicitudId)
                .OnDelete(DeleteBehavior.Cascade);

            // ✅ ✅ RESTRICCIÓN: SOLO UNA POR SOLICITUD
            builder.Entity<SolicitudInventarioTemporal>()
                .HasIndex(i => i.SolicitudId)
                .IsUnique();

            // ✅ ✅ FOLIO ÚNICO
            builder.Entity<SolicitudMovimiento>()
                .HasIndex(s => s.Folio)
                .IsUnique();

            builder.Entity<SolicitudMovimientosTecnicos>()
                .HasOne(t => t.Solicitud)
                .WithOne(s => s.MovimientosTecnicos)
                .HasForeignKey<SolicitudMovimientosTecnicos>(t => t.SolicitudId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<SolicitudMovimientosTecnicos>()
                .HasIndex(t => t.SolicitudId)
                .IsUnique();
        }
    }
}