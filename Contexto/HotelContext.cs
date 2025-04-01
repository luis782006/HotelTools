using Microsoft.EntityFrameworkCore;

namespace HotelTools.Models
{
    public class HotelContext : DbContext
    {
        private readonly IConfiguration _config;

        public HotelContext(DbContextOptions<HotelContext> options, IConfiguration config)
            : base(options)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("Hotel_Tools"));
            }
        }

        public virtual DbSet<Empleado> Empleados { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<SesionActiva> SesionActiva { get; set; }
        public virtual DbSet<Cargo> Cargo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(c => c.ID_Cargo);
                entity.ToTable("Cargo", "Empleados");
            });

           modelBuilder.Entity<Empleado>(entity =>
           {
               entity.HasKey(c => c.ID_Empleado);
               entity.Property(e => e.ID_Empleado).ValueGeneratedOnAdd(); // Autoincremental
               entity.ToTable("Empleados", "Empleados"); 
           });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(c => c.ID_Rol);
                entity.ToTable("Rol", "Empleados");
            });

            modelBuilder.Entity<SesionActiva>(entity =>
            {
                entity.HasKey(c => c.TokenValue);
                entity.ToTable("SesionActiva", "Empleados");
            });
        }
    }
}