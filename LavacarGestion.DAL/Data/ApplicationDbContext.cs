using LavacarGestion.Entities;
using Microsoft.EntityFrameworkCore;

namespace LavacarGestion.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets representan las tablas en la base de datos
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Cita> Citas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Cliente
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.ClienteId);
                entity.Property(e => e.Identificacion).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Apellidos).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Telefono).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);

                // Índice único para Identificacion
                entity.HasIndex(e => e.Identificacion).IsUnique();
            });

            // Configuración de Vehiculo
            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(e => e.VehiculoId);
                entity.Property(e => e.Placa).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Marca).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Modelo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(30);

                // Relación: Un vehículo pertenece a un cliente
                entity.HasOne(e => e.Cliente)
                      .WithMany(c => c.Vehiculos)
                      .HasForeignKey(e => e.ClienteId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configuración de Cita
            modelBuilder.Entity<Cita>(entity =>
            {
                entity.HasKey(e => e.CitaId);
                entity.Property(e => e.Observaciones).HasMaxLength(500);

                // Relación: Una cita pertenece a un cliente
                entity.HasOne(e => e.Cliente)
                      .WithMany()
                      .HasForeignKey(e => e.ClienteId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Relación: Una cita pertenece a un vehículo
                entity.HasOne(e => e.Vehiculo)
                      .WithMany(v => v.Citas)
                      .HasForeignKey(e => e.VehiculoId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}