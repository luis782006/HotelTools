USE HotelTools;
GO

-- =====================================================================
-- Cambios de esquema para Carga de Productos
-- 1) Agregar FK a Habitaciones en Productos
-- 2) Agregar campos de estado, observaciones, reparacion
-- 3) Corregir Descripcion NOT NULL
-- =====================================================================

BEGIN TRANSACTION;

-- FK a Habitaciones (nullable - no todos los productos tienen destino)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Inventarios.Productos') AND name = 'ID_HabitacionFK')
    ALTER TABLE [Inventarios].[Productos] ADD ID_HabitacionFK decimal(18, 0) NULL;

-- Estado del producto
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Inventarios.Productos') AND name = 'Estado')
    ALTER TABLE [Inventarios].[Productos] ADD Estado varchar(20) NOT NULL CONSTRAINT DF_Productos_Estado DEFAULT 'Nuevo';

-- Observaciones
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Inventarios.Productos') AND name = 'Observaciones')
    ALTER TABLE [Inventarios].[Productos] ADD Observaciones varchar(200) NULL;

-- Quien reparo
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Inventarios.Productos') AND name = 'ReparadoPor')
    ALTER TABLE [Inventarios].[Productos] ADD ReparadoPor decimal(18, 0) NULL;

-- Fecha de reparacion
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Inventarios.Productos') AND name = 'FechaReparacion')
    ALTER TABLE [Inventarios].[Productos] ADD FechaReparacion datetime NULL;

-- Corregir Descripcion NOT NULL (solo si tiene datos existentes con NULL)
UPDATE [Inventarios].[Productos] SET Descripcion = 'Sin descripcion' WHERE Descripcion IS NULL;

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('Inventarios.Productos') AND name = 'Descripcion' AND is_nullable = 1)
    ALTER TABLE [Inventarios].[Productos] ALTER COLUMN Descripcion varchar(60) NOT NULL;

COMMIT TRANSACTION;
GO

-- =====================================================================
-- FK a Habitaciones
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Productos_Habitaciones')
    ALTER TABLE [Inventarios].[Productos] WITH CHECK ADD CONSTRAINT FK_Productos_Habitaciones
        FOREIGN KEY (ID_HabitacionFK) REFERENCES [General].[Habitaciones] (ID_NroHab);
GO

-- =====================================================================
-- Verificacion
-- =====================================================================
SELECT c.name, c.is_nullable, t.name AS tipo, c.max_length
FROM sys.columns c
JOIN sys.types t ON c.user_type_id = t.user_type_id
WHERE c.object_id = OBJECT_ID('Inventarios.Productos')
ORDER BY c.column_id;
GO
