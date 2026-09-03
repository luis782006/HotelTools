-- Insertar permisos base
INSERT INTO [Empleados].[Permisos] ([ID_Permiso], [Codigo], [Descripcion])
VALUES 
    (1, 'ViewHabitacionesStatus', 'Ver panel de habitaciones'),
    (2, 'ManageUsers', 'Gestionar usuarios del sistema'),
    (3, 'ManageRoles', 'Gestionar roles y permisos'),
    (4, 'ViewQuejas', 'Ver quejas'),
    (5, 'ManageInventario', 'Gestionar inventario'),
    (6, 'ManageProveedores', 'Gestionar proveedores'),
    (7, 'ManageConfiguraciones', 'Acceder a configuraciones del sistema'),
    (8, 'Quejas.Admin', 'Administrar quejas'),
    (9, 'Quejas.Asignar', 'Asignar quejas a empleados'),
    (10, 'Quejas.Cerrar', 'Cerrar quejas'),
    (11, 'Quejas.Aprobar', 'Aprobar compras vinculadas a quejas'),
    (12, 'ManageQuejasConfig', 'Gestionar configuración de quejas')
GO

-- Asignar todos los permisos al rol Admin (ID 1), sin restriccion de departamento
INSERT INTO [Empleados].[RolPermisos] ([ID_RolPermiso], [ID_Rol], [ID_Permiso], [ID_Departamento])
VALUES 
    (1, 1, 1, NULL),
    (2, 1, 2, NULL),
    (3, 1, 3, NULL),
    (4, 1, 4, NULL),
    (5, 1, 5, NULL),
    (6, 1, 6, NULL),
    (7, 1, 7, NULL),
    (8, 1, 8, NULL),
    (9, 1, 9, NULL),
    (10, 1, 10, NULL),
    (11, 1, 11, NULL),
    (12, 1, 12, NULL)
GO

-- Asignar permisos limitados al rol Empleado (ID 2), segun departamento
-- Empleado en Direccion (ID 1): solo puede ver habitaciones y quejas
INSERT INTO [Empleados].[RolPermisos] ([ID_RolPermiso], [ID_Rol], [ID_Permiso], [ID_Departamento])
VALUES 
    (8, 2, 1, 1),
    (9, 2, 4, 1)
GO
