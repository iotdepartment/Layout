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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SolicitudMovimiento>()
                .HasOne(s => s.UsuarioSolicitante)
                .WithMany()
                .HasForeignKey(s => s.UsuarioSolicitanteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SolicitudMovimiento>()
                .HasOne(s => s.UsuarioAprobador)
                .WithMany()
                .HasForeignKey(s => s.UsuarioAprobadorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SolicitudAprobacionDetalle>()
                .HasOne(d => d.SolicitudMovimiento)
                .WithOne(s => s.DetalleAprobacion)
                .HasForeignKey<SolicitudAprobacionDetalle>(d => d.SolicitudMovimientoId);

            builder.Entity<SolicitudAprobacionDetalle>()
                .HasOne(d => d.UsuarioEjecutor)
                .WithMany()
                .HasForeignKey(d => d.UsuarioEjecutorId)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}