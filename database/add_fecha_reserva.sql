-- Script para añadir la columna FechaReserva a la tabla linea_carrito
-- Ejecuta este script en tu cliente MySQL (phpMyAdmin, MySQL Workbench, etc.)

USE depan_db;

-- Añadir columna fecha_reserva con valor por defecto CURRENT_TIMESTAMP
ALTER TABLE linea_carrito 
ADD COLUMN fecha_reserva DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP;

-- Verificar que se añadió correctamente
DESCRIBE linea_carrito;

-- Opcional: Actualizar registros existentes para que tengan la fecha actual
UPDATE linea_carrito 
SET fecha_reserva = NOW() 
WHERE fecha_reserva IS NULL OR fecha_reserva = '0000-00-00 00:00:00';

SELECT 'Columna fecha_reserva añadida exitosamente' AS resultado;
