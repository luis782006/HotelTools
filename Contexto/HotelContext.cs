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
        public virtual DbSet<Rol> Rol { get; set; }
        public virtual DbSet<SesionActiva> SesionesActivas { get; set; }
        public virtual DbSet<Cargo> Cargo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(c => c.ID_Cargo);
                entity.Property(e => e.ID_Cargo).ValueGeneratedOnAdd(); // Autoincremental
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
                entity.Property(e => e.ID_Rol).ValueGeneratedOnAdd(); // Autoincremental
                entity.ToTable("Rol", "Empleados");
            });

            modelBuilder.Entity<SesionActiva>(entity =>
            {
                entity.HasKey(c => c.ID_SesionesActiva);
                entity.Property(e => e.ID_SesionesActiva).ValueGeneratedOnAdd(); // Autoincremental
                entity.ToTable("SesionesActivas", "Empleados");
            });
        }
    }
}