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
        public virtual DbSet<ProductosMovimientos> ProductosMovimientos { get; set; }
        public virtual DbSet<TipoProducto> TipoProductos { get; set; }
        public virtual DbSet<PaqueteProducto> PaqueteProductos { get; set; }
        public virtual DbSet<PaqueteProductoDetalle> PaqueteProductoDetalles { get; set; }

        public virtual DbSet<Queja> Quejas { get; set; }
        public virtual DbSet<HistorialQueja> HistorialQuejas { get; set; }
        public virtual DbSet<Huesped> Huespedes { get; set; }
        public virtual DbSet<CategoriaQueja> CategoriasQueja { get; set; }
        public virtual DbSet<EstadoQueja> EstadosQueja { get; set; }
        public virtual DbSet<PrioridadQueja> PrioridadesQueja { get; set; }
        public virtual DbSet<ImagenQueja> ImagenesQueja { get; set; }
        public virtual DbSet<QuejaImagen> QuejasImagenes { get; set; }

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
                entity.Property(e => e.ID_HabitacionOrigenFK);
                entity.Property(e => e.Prestamo).HasDefaultValue(false);
                entity.Property(e => e.Prestado).HasDefaultValue(false);
                entity.ToTable("Productos", "Inventarios");
            });

            modelBuilder.Entity<HabitacionProductos>(entity =>
            {
                entity.HasKey(c => c.ID_HabProductos);
                entity.Property(e => e.ID_HabProductos).ValueGeneratedOnAdd();
                entity.Property(e => e.EsNativo).HasDefaultValue(true);
                entity.Property(e => e.EsPrestamo).HasDefaultValue(false);
                entity.Property(e => e.Activo).HasDefaultValue(true);
                entity.Property(e => e.PrestadoFuera).HasDefaultValue(false);
                entity.Property(e => e.FechaAsignacion);
                entity.Property(e => e.FechaRetiro);
                entity.ToTable("HabitacionProductos", "Inventarios");
            });

            modelBuilder.Entity<ProductosMovimientos>(entity =>
            {
                entity.HasKey(c => c.ID_ProductosMovimientos);
                entity.Property(e => e.ID_ProductosMovimientos).ValueGeneratedOnAdd();
                entity.Property(e => e.ID_Productos);
                entity.Property(e => e.ID_HabOrigen);
                entity.Property(e => e.ID_HabDestino);
                entity.Property(e => e.FechaMov);
                entity.Property(e => e.ID_Empleado);
                entity.Property(e => e.Observaciones);
                entity.Property(e => e.ID_EmpleadoMov);
                entity.Property(e => e.TipoMovimiento).HasMaxLength(20);
                entity.Property(e => e.ID_HabitacionCasa);
                entity.ToTable("ProductosMovimientos", "Inventarios");
            });

            modelBuilder.Entity<TipoProducto>(entity =>
            {
                entity.HasKey(e => e.ID_TipoProducto);
                entity.Property(e => e.ID_TipoProducto).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).HasMaxLength(60);
                entity.Property(e => e.Descripcion).HasMaxLength(200);
                entity.ToTable("TipoProducto", "Inventarios");
            });

            modelBuilder.Entity<PaqueteProducto>(entity =>
            {
                entity.HasKey(e => e.ID_Paquete);
                entity.Property(e => e.ID_Paquete).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).HasMaxLength(60);
                entity.ToTable("PaqueteProducto", "Inventarios");
            });

            modelBuilder.Entity<PaqueteProductoDetalle>(entity =>
            {
                entity.HasKey(e => e.ID_Detalle);
                entity.Property(e => e.ID_Detalle).ValueGeneratedOnAdd();
                entity.Property(e => e.Cantidad);
                entity.HasOne(e => e.Paquete).WithMany().HasForeignKey(e => e.ID_PaqueteFK)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.TipoProducto).WithMany().HasForeignKey(e => e.ID_TipoProductoFK);
                entity.ToTable("PaqueteProductoDetalle", "Inventarios");
            });

            modelBuilder.Entity<Queja>(entity =>
            {
                entity.HasKey(e => e.ID_Quejas);
                entity.Property(e => e.ID_Quejas).ValueGeneratedOnAdd();
                entity.Property(e => e.Quejas).HasColumnType("nvarchar(max)");
                entity.ToTable("Quejas", "Quejas");
            });

            modelBuilder.Entity<HistorialQueja>(entity =>
            {
                entity.HasKey(e => e.ID_Orden);
                entity.Property(e => e.ID_Orden).ValueGeneratedOnAdd();
                entity.Property(e => e.observaciones).HasMaxLength(10);
                entity.Property(e => e.FechaRegistro).HasDefaultValueSql("GETDATE()");
                entity.ToTable("Historial", "Quejas");
            });

            modelBuilder.Entity<Huesped>(entity =>
            {
                entity.HasKey(e => e.ID_Huesped);
                entity.Property(e => e.ID_Huesped).ValueGeneratedOnAdd();
                entity.Property(e => e.Nombre).HasMaxLength(40);
                entity.Property(e => e.Apellido).HasMaxLength(50);
                entity.ToTable("Huespedes", "Quejas");
            });

            modelBuilder.Entity<CategoriaQueja>(entity =>
            {
                entity.HasKey(e => e.ID_CategoriaQueja);
                entity.Property(e => e.ID_CategoriaQueja).ValueGeneratedOnAdd();
                entity.Property(e => e.NombreCategoria).HasMaxLength(60);
                entity.Property(e => e.Descripcion).HasMaxLength(250);
                entity.Property(e => e.Activo).HasDefaultValue(true);
                entity.ToTable("CategoriasQueja", "Quejas");
            });

            modelBuilder.Entity<EstadoQueja>(entity =>
            {
                entity.HasKey(e => e.ID_Estado);
                entity.Property(e => e.ID_Estado).ValueGeneratedOnAdd();
                entity.Property(e => e.NombreEstado).HasColumnType("nvarchar(max)");
                entity.ToTable("Estados", "Quejas");
            });

            modelBuilder.Entity<PrioridadQueja>(entity =>
            {
                entity.HasKey(e => e.ID_Prioridad);
                entity.Property(e => e.ID_Prioridad).ValueGeneratedOnAdd();
                entity.Property(e => e.Descripcion).HasMaxLength(100);
                entity.Property(e => e.Color).HasMaxLength(20);
                entity.ToTable("Prioridad", "Quejas");
            });

            modelBuilder.Entity<ImagenQueja>(entity =>
            {
                entity.HasKey(e => e.idImagen);
                entity.Property(e => e.idImagen).ValueGeneratedOnAdd();
                entity.Property(e => e.imagen).HasColumnType("varbinary(max)");
                entity.ToTable("Imagen", "Quejas");
            });

            modelBuilder.Entity<QuejaImagen>(entity =>
            {
                entity.HasKey(e => e.ID_QuejaImagen);
                entity.Property(e => e.ID_QuejaImagen).ValueGeneratedOnAdd();
                entity.Property(e => e.FechaAdjunto).HasDefaultValueSql("GETDATE()");
                entity.ToTable("QuejaImagen", "Quejas");
            });
        }
    }
}