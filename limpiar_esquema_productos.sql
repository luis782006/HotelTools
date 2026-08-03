USE HotelTools;
GO

-- =====================================================================
-- Limpieza de esquema para el ABM de Productos
-- 1) Eliminar FK_Productos_ProductosMovimientos (relacion circular: el
--    producto no apunta a "un" movimiento, el movimiento apunta al producto)
-- 2) ID_NroFacturaFK pasa a NULL (factura opcional)
-- =====================================================================

BEGIN TRANSACTION;

IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Productos_ProductosMovimientos')
    ALTER TABLE [Inventarios].[Productos] DROP CONSTRAINT [FK_Productos_ProductosMovimientos];

ALTER TABLE [Inventarios].[Productos] ALTER COLUMN ID_NroFacturaFK decimal(18, 0) NULL;

COMMIT TRANSACTION;
GO

-- Verificacion: la FK no debe existir y la columna debe ser nullable (is_nullable = 1)
SELECT name, is_nullable FROM sys.columns
WHERE object_id = OBJECT_ID('Inventarios.Productos') AND name = 'ID_NroFacturaFK';
SELECT name FROM sys.foreign_keys WHERE name = 'FK_Productos_ProductosMovimientos';
GO
