-- ============================================================
-- Esquema de Paquetes de Productos
-- Tablas: TipoProducto (catalogo), PaqueteProducto, PaqueteProductoDetalle
-- Esquema: Inventarios
-- ============================================================

IF OBJECT_ID('Inventarios.PaqueteProductoDetalle', 'U') IS NOT NULL
    DROP TABLE Inventarios.PaqueteProductoDetalle;
GO

IF OBJECT_ID('Inventarios.PaqueteProducto', 'U') IS NOT NULL
    DROP TABLE Inventarios.PaqueteProducto;
GO

IF OBJECT_ID('Inventarios.TipoProducto', 'U') IS NOT NULL
    DROP TABLE Inventarios.TipoProducto;
GO

-- Catalogo de productos para la carga
CREATE TABLE Inventarios.TipoProducto (
    ID_TipoProducto DECIMAL(18,0) IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(60) NOT NULL,
    Descripcion VARCHAR(200) NULL,
    ID_CategoriaProFK DECIMAL(18,0) NOT NULL,
    ID_ModelosFK DECIMAL(18,0) NOT NULL,
    CONSTRAINT FK_TipoProducto_CategoriasProductos FOREIGN KEY (ID_CategoriaProFK)
        REFERENCES Inventarios.CategoriasProductos(ID_CategoriasPro),
    CONSTRAINT FK_TipoProducto_Modelos FOREIGN KEY (ID_ModelosFK)
        REFERENCES Inventarios.Modelos(ID_Modelos)
);
GO

-- Encabezado de paquete por categoria de habitacion
CREATE TABLE Inventarios.PaqueteProducto (
    ID_Paquete DECIMAL(18,0) IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(60) NOT NULL,
    ID_CategoriaHabitacionFK DECIMAL(18,0) NOT NULL,
    CONSTRAINT FK_PaqueteProducto_CategoriasHab FOREIGN KEY (ID_CategoriaHabitacionFK)
        REFERENCES Inventarios.Categorias(ID_Categoria)
);
GO

-- Detalle: productos y cantidades del paquete
CREATE TABLE Inventarios.PaqueteProductoDetalle (
    ID_Detalle DECIMAL(18,0) IDENTITY(1,1) PRIMARY KEY,
    ID_PaqueteFK DECIMAL(18,0) NOT NULL,
    ID_TipoProductoFK DECIMAL(18,0) NOT NULL,
    Cantidad INT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Detalle_Paquete FOREIGN KEY (ID_PaqueteFK)
        REFERENCES Inventarios.PaqueteProducto(ID_Paquete) ON DELETE CASCADE,
    CONSTRAINT FK_Detalle_TipoProducto FOREIGN KEY (ID_TipoProductoFK)
        REFERENCES Inventarios.TipoProducto(ID_TipoProducto)
);
GO

SELECT 'OK' AS Resultado;
