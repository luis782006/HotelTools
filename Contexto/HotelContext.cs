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
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<RolPermiso> RolPermisos { get; set; }
        public virtual DbSet<Departamento> Departamentos { get; set; }
        public virtual DbSet<CategoriaHabitacion> CategoriaHabitaciones { get; set; }
        public virtual DbSet<Habitacion> Habitaciones { get; set; }
        public virtual DbSet<Proveedor> Proveedores { get; set; }
        public virtual DbSet<RazonSocial> RazonSociales { get; set; }
        public virtual DbSet<Representante> Representantes { get; set; }
        public virtual DbSet<CategoriaProducto> CategoriasProductos { get; set; }
        public virtual DbSet<Modelo> Modelos { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }
        public virtual DbSet<HabitacionProductos> HabitacionProductos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cargo>(entity =>
            {
                entity.HasKey(c => c.ID_Cargo);
                entity.Property(e => e.ID_Cargo).ValueGeneratedOnAdd(); 
                entity.ToTable("Cargo", "Empleados");
            });

           modelBuilder.Entity<Empleado>(entity =>
           {
               entity.HasKey(c => c.ID_Empleado);
               entity.Property(e => e.ID_Empleado).ValueGeneratedOnAdd(); 
               entity.ToTable("Empleados", "Empleados"); 
           });

            modelBuilder.Entity<Rol>(entity =>
            {
                entity.HasKey(c => c.ID_Rol);
                entity.Property(e => e.ID_Rol).ValueGeneratedOnAdd(); 
                entity.ToTable("Rol", "Empleados");
            });

            modelBuilder.Entity<SesionActiva>(entity =>
            {
                entity.HasKey(c => c.ID_SesionesActiva);
                entity.Property(e => e.ID_SesionesActiva).ValueGeneratedOnAdd(); 
                entity.ToTable("SesionesActivas", "Empleados");
            });

            modelBuilder.Entity<Permiso>(entity =>
            {
                entity.HasKey(c => c.ID_Permiso);
                entity.Property(e => e.ID_Permiso).ValueGeneratedOnAdd();
                entity.Property(e => e.Codigo).HasMaxLength(120);
                entity.Property(e => e.Descripcion).HasMaxLength(250);
                entity.ToTable("Permisos", "Empleados");
            });

            modelBuilder.Entity<RolPermiso>(entity =>
            {
                entity.HasKey(c => c.ID_RolPermiso);
                entity.Property(e => e.ID_RolPermiso).ValueGeneratedOnAdd();
                entity.ToTable("RolPermisos", "Empleados");
            });

            modelBuilder.Entity<Departamento>(entity =>
            {
                entity.HasKey(c => c.ID_Departamento);
                entity.Property(e => e.ID_Departamento).ValueGeneratedOnAdd();
                entity.Property(e => e.NombreDepartamento).HasMaxLength(40);
                entity.ToTable("Departamentos", "General");
            });

            modelBuilder.Entity<CategoriaHabitacion>(entity =>
            {
                entity.HasKey(c => c.ID_Categoria);
                entity.Property(e => e.ID_Categoria).ValueGeneratedOnAdd();
                entity.Property(e => e.TipoHab).HasMaxLength(20);
                entity.ToTable("Categorias", "Inventarios");
            });

            modelBuilder.Entity<Habitacion>(entity =>
            {
                entity.HasKey(c => c.ID_NroHab);
                entity.Property(e => e.ID_NroHab).ValueGeneratedOnAdd();
                entity.Property(e => e.DescripcionHab).HasMaxLength(20);
                entity.ToTable("Habitaciones", "General");
            });

            modelBuilder.Entity<Proveedor>(entity =>
            {
                entity.HasKey(c => c.ID_Proveedor);
                entity.Property(e => e.ID_Proveedor).ValueGeneratedOnAdd();
                entity.ToTable("Proveedores", "Inventarios");
            });

            modelBuilder.Entity<RazonSocial>(entity =>
            {
                entity.HasKey(c => c.ID_RazonSocial);
                entity.Property(e => e.ID_RazonSocial).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.DireccionFisica).HasMaxLength(150);
                entity.Property(e => e.DireccionDigital).HasMaxLength(150);
                entity.Property(e => e.TelefonoFijo).HasMaxLength(30);
                entity.Property(e => e.TelefonoCelular).HasMaxLength(30);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.ToTable("RazonSocial", "Inventarios");
            });

            modelBuilder.Entity<Representante>(entity =>
            {
                entity.HasKey(c => c.ID_Representante);
                entity.Property(e => e.ID_Representante).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).HasMaxLength(100);
                entity.Property(e => e.TelefonoCelular).HasMaxLength(30);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.ToTable("Representante", "Inventarios");
            });

            modelBuilder.Entity<CategoriaProducto>(entity =>
            {
                entity.HasKey(c => c.ID_CategoriasPro);
                entity.Property(e => e.ID_CategoriasPro).ValueGeneratedOnAdd();
                entity.Property(e => e.NombreCatProductos).HasMaxLength(60);
                entity.ToTable("CategoriasProductos", "Inventarios");
            });

            modelBuilder.Entity<Modelo>(entity =>
            {
                entity.HasKey(c => c.ID_Modelos);
                entity.Property(e => e.ID_Modelos).ValueGeneratedOnAdd();
                entity.Property(e => e.NombreModelos).HasMaxLength(50);
                entity.ToTable("Modelos", "Inventarios");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(c => c.ID_Productos);
                entity.Property(e => e.ID_Productos).ValueGeneratedOnAdd();
                entity.Property(e => e.Codigo).HasMaxLength(80);
                entity.Property(e => e.Descripcion).HasMaxLength(60);
                entity.Property(e => e.ID_NroFacturaFK);
                entity.Property(e => e.ID_HabitacionFK);
                entity.Property(e => e.Estado).HasMaxLength(20).HasDefaultValue("Nuevo");
                entity.Property(e => e.Observaciones).HasMaxLength(200);
                entity.Property(e => e.ReparadoPor);
                entity.Property(e => e.FechaReparacion);
                entity.ToTable("Productos", "Inventarios");
            });

            modelBuilder.Entity<HabitacionProductos>(entity =>
            {
                entity.HasKey(c => c.ID_HabProductos);
                entity.Property(e => e.ID_HabProductos).ValueGeneratedOnAdd();
                entity.ToTable("HabitacionProductos", "Inventarios");
            });
        }
    }
}