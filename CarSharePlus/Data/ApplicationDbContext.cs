using Microsoft.EntityFrameworkCore;
using CarSharePlus.Shared.Models;

namespace CarSharePlus.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

<<<<<<< HEAD
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Evaluacion> Evaluaciones { get; set; }
=======
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
>>>>>>> 875207af3e982a6adcdb3d4de98f46b58b45ec4a

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Placa única en Vehiculo
            modelBuilder.Entity<Vehiculo>()
                .HasIndex(v => v.Placa)
                .IsUnique();

            // Correo único en Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Correo)
                .IsUnique();

<<<<<<< HEAD
            // Usuario → Vehiculos
=======
            // Relación Usuario-Vehiculo (uno a muchos)
>>>>>>> 875207af3e982a6adcdb3d4de98f46b58b45ec4a
            modelBuilder.Entity<Vehiculo>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Vehiculos)
                .HasForeignKey(v => v.UsuarioId)
<<<<<<< HEAD
                .OnDelete(DeleteBehavior.Restrict);

            // Usuario → Reservas
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Usuario)
                .WithMany(u => u.Reservas)
                .HasForeignKey(r => r.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vehiculo → Reservas
            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Vehiculo)
                .WithMany(v => v.Reservas)
                .HasForeignKey(r => r.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);

            // Reserva → Pagos
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Reserva)
                .WithMany(r => r.Pagos)
                .HasForeignKey(p => p.ReservaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.Usuario)
                .WithMany(u => u.Evaluaciones)
                .HasForeignKey(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict); 
            
            modelBuilder.Entity<Evaluacion>()
                .HasOne(e => e.Vehiculo)
                .WithMany(v => v.Evaluaciones)
                .HasForeignKey(e => e.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);
=======
                .OnDelete(DeleteBehavior.Restrict); // evita borrado en cascada
>>>>>>> 875207af3e982a6adcdb3d4de98f46b58b45ec4a
        }
    }
}
