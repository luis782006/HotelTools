using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelTools.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Empleados");

            migrationBuilder.EnsureSchema(
                name: "Inventarios");

            migrationBuilder.EnsureSchema(
                name: "General");

            migrationBuilder.CreateTable(
                name: "Cargo",
                schema: "Empleados",
                columns: table => new
                {
                    ID_Cargo = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCargo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargo", x => x.ID_Cargo);
                });

            migrationBuilder.CreateTable(
                name: "Categorias",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_Categoria = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoHab = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.ID_Categoria);
                });

            migrationBuilder.CreateTable(
                name: "CategoriasProductos",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_CategoriasPro = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCatProductos = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasProductos", x => x.ID_CategoriasPro);
                });

            migrationBuilder.CreateTable(
                name: "Departamentos",
                schema: "General",
                columns: table => new
                {
                    ID_Departamento = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreDepartamento = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departamentos", x => x.ID_Departamento);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                schema: "Empleados",
                columns: table => new
                {
                    ID_Empleado = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_Cargo = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_Departamento = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NroContacto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_Rol = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaDesde = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaHasta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activo = table.Column<byte>(type: "tinyint", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.ID_Empleado);
                });

            migrationBuilder.CreateTable(
                name: "Habitaciones",
                schema: "General",
                columns: table => new
                {
                    ID_NroHab = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_CategoriasFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MaxPersonas = table.Column<int>(type: "int", nullable: false),
                    DescripcionHab = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Estado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Habitaciones", x => x.ID_NroHab);
                });

            migrationBuilder.CreateTable(
                name: "HabitacionProductos",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_HabProductos = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_HabFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_ProductosFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HabitacionProductos", x => x.ID_HabProductos);
                });

            migrationBuilder.CreateTable(
                name: "Modelos",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_Modelos = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreModelos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modelos", x => x.ID_Modelos);
                });

            migrationBuilder.CreateTable(
                name: "PaqueteProducto",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_Paquete = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ID_CategoriaHabitacionFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaqueteProducto", x => x.ID_Paquete);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                schema: "Empleados",
                columns: table => new
                {
                    ID_Permiso = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.ID_Permiso);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_Productos = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    ID_CategoriaProFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_ModelosFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_NroFacturaFK = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ID_HabitacionFK = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Estado = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Nuevo"),
                    Observaciones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReparadoPor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FechaReparacion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.ID_Productos);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_Proveedor = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_RazonSocialFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_RepresentanteFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedores", x => x.ID_Proveedor);
                });

            migrationBuilder.CreateTable(
                name: "RazonSocial",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_RazonSocial = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Cuil = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DireccionFisica = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    DireccionDigital = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    TelefonoFijo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    TelefonoCelular = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RazonSocial", x => x.ID_RazonSocial);
                });

            migrationBuilder.CreateTable(
                name: "Representante",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_Representante = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TelefonoCelular = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Representante", x => x.ID_Representante);
                });

            migrationBuilder.CreateTable(
                name: "Rol",
                schema: "Empleados",
                columns: table => new
                {
                    ID_Rol = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreRol = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.ID_Rol);
                });

            migrationBuilder.CreateTable(
                name: "RolPermisos",
                schema: "Empleados",
                columns: table => new
                {
                    ID_RolPermiso = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_Rol = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_Permiso = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_Departamento = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolPermisos", x => x.ID_RolPermiso);
                });

            migrationBuilder.CreateTable(
                name: "SesionesActivas",
                schema: "Empleados",
                columns: table => new
                {
                    ID_SesionesActiva = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ID_Empleado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaExpiracion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoSesion = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SesionesActivas", x => x.ID_SesionesActiva);
                });

            migrationBuilder.CreateTable(
                name: "TipoProducto",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_TipoProducto = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ID_CategoriaProFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_ModelosFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoProducto", x => x.ID_TipoProducto);
                });

            migrationBuilder.CreateTable(
                name: "PaqueteProductoDetalle",
                schema: "Inventarios",
                columns: table => new
                {
                    ID_Detalle = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID_PaqueteFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ID_TipoProductoFK = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaqueteProductoDetalle", x => x.ID_Detalle);
                    table.ForeignKey(
                        name: "FK_PaqueteProductoDetalle_PaqueteProducto_ID_PaqueteFK",
                        column: x => x.ID_PaqueteFK,
                        principalSchema: "Inventarios",
                        principalTable: "PaqueteProducto",
                        principalColumn: "ID_Paquete",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaqueteProductoDetalle_TipoProducto_ID_TipoProductoFK",
                        column: x => x.ID_TipoProductoFK,
                        principalSchema: "Inventarios",
                        principalTable: "TipoProducto",
                        principalColumn: "ID_TipoProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaqueteProductoDetalle_ID_PaqueteFK",
                schema: "Inventarios",
                table: "PaqueteProductoDetalle",
                column: "ID_PaqueteFK");

            migrationBuilder.CreateIndex(
                name: "IX_PaqueteProductoDetalle_ID_TipoProductoFK",
                schema: "Inventarios",
                table: "PaqueteProductoDetalle",
                column: "ID_TipoProductoFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cargo",
                schema: "Empleados");

            migrationBuilder.DropTable(
                name: "Categorias",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "CategoriasProductos",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "Departamentos",
                schema: "General");

            migrationBuilder.DropTable(
                name: "Empleados",
                schema: "Empleados");

            migrationBuilder.DropTable(
                name: "Habitaciones",
                schema: "General");

            migrationBuilder.DropTable(
                name: "HabitacionProductos",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "Modelos",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "PaqueteProductoDetalle",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "Permisos",
                schema: "Empleados");

            migrationBuilder.DropTable(
                name: "Productos",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "Proveedores",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "RazonSocial",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "Representante",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "Rol",
                schema: "Empleados");

            migrationBuilder.DropTable(
                name: "RolPermisos",
                schema: "Empleados");

            migrationBuilder.DropTable(
                name: "SesionesActivas",
                schema: "Empleados");

            migrationBuilder.DropTable(
                name: "PaqueteProducto",
                schema: "Inventarios");

            migrationBuilder.DropTable(
                name: "TipoProducto",
                schema: "Inventarios");
        }
    }
}
