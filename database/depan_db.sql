-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         12.0.2-MariaDB - mariadb.org binary distribution
-- SO del servidor:              Win64
-- HeidiSQL Versión:             12.12.0.7122
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para depan_db
CREATE DATABASE IF NOT EXISTS `depan_db` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_uca1400_ai_ci */;
USE `depan_db`;

-- Volcando estructura para tabla depan_db.carrito
CREATE TABLE IF NOT EXISTS `carrito` (
  `id_carrito` int(11) NOT NULL AUTO_INCREMENT,
  `id_usuario` int(11) NOT NULL,
  `fecha_creacion` datetime NOT NULL DEFAULT current_timestamp(),
  `fecha_actualizacion` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `total` decimal(10,2) NOT NULL DEFAULT 0.00 CHECK (`total` >= 0),
  PRIMARY KEY (`id_carrito`),
  UNIQUE KEY `uk_carrito_usuario` (`id_usuario`),
  KEY `idx_usuario` (`id_usuario`),
  CONSTRAINT `carrito_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Volcando datos para la tabla depan_db.carrito: ~1 rows (aproximadamente)
INSERT INTO `carrito` (`id_carrito`, `id_usuario`, `fecha_creacion`, `fecha_actualizacion`, `total`) VALUES
	(1, 5, '2025-11-01 09:59:18', '2025-11-08 13:49:33', 0.00),
	(2, 4, '2025-11-08 13:39:28', '2025-11-08 15:03:55', 0.00);

-- Volcando estructura para tabla depan_db.categoria
CREATE TABLE IF NOT EXISTS `categoria` (
  `id_categoria` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `descripcion` text DEFAULT NULL,
  `activa` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id_categoria`),
  UNIQUE KEY `nombre` (`nombre`),
  KEY `idx_nombre` (`nombre`),
  KEY `idx_activa` (`activa`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Volcando datos para la tabla depan_db.categoria: ~3 rows (aproximadamente)
INSERT INTO `categoria` (`id_categoria`, `nombre`, `descripcion`, `activa`) VALUES
	(1, 'Pan', 'Diferentes tipos de pan artesanal', 1),
	(2, 'Bollería', 'Dulces y bollería variada', 1),
	(3, 'Repostería', 'Tartas y pasteles para ocasiones especiales', 1);

-- Volcando estructura para tabla depan_db.linea_carrito
CREATE TABLE IF NOT EXISTS `linea_carrito` (
  `id_linea_carrito` int(11) NOT NULL AUTO_INCREMENT,
  `id_carrito` int(11) NOT NULL,
  `id_producto` int(11) NOT NULL,
  `cantidad` int(11) NOT NULL DEFAULT 1 CHECK (`cantidad` > 0),
  `precio_unitario` decimal(10,2) NOT NULL CHECK (`precio_unitario` >= 0),
  `subtotal` decimal(10,2) NOT NULL CHECK (`subtotal` >= 0),
  `fecha_reserva` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id_linea_carrito`),
  UNIQUE KEY `uk_carrito_producto` (`id_carrito`,`id_producto`),
  KEY `idx_carrito` (`id_carrito`),
  KEY `idx_producto` (`id_producto`),
  CONSTRAINT `linea_carrito_ibfk_1` FOREIGN KEY (`id_carrito`) REFERENCES `carrito` (`id_carrito`) ON DELETE CASCADE,
  CONSTRAINT `linea_carrito_ibfk_2` FOREIGN KEY (`id_producto`) REFERENCES `producto` (`id_producto`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Volcando datos para la tabla depan_db.linea_carrito: ~0 rows (aproximadamente)

-- Volcando estructura para tabla depan_db.linea_pedido
CREATE TABLE IF NOT EXISTS `linea_pedido` (
  `id_linea_pedido` int(11) NOT NULL AUTO_INCREMENT,
  `id_pedido` int(11) NOT NULL,
  `id_producto` int(11) NOT NULL,
  `nombre_producto` varchar(200) NOT NULL COMMENT 'Snapshot del nombre en el momento del pedido',
  `cantidad` int(11) NOT NULL CHECK (`cantidad` > 0),
  `precio_unitario` decimal(10,2) NOT NULL CHECK (`precio_unitario` >= 0),
  `subtotal` decimal(10,2) NOT NULL CHECK (`subtotal` >= 0),
  PRIMARY KEY (`id_linea_pedido`),
  KEY `idx_pedido` (`id_pedido`),
  KEY `idx_producto` (`id_producto`),
  CONSTRAINT `linea_pedido_ibfk_1` FOREIGN KEY (`id_pedido`) REFERENCES `pedido` (`id_pedido`) ON DELETE CASCADE,
  CONSTRAINT `linea_pedido_ibfk_2` FOREIGN KEY (`id_producto`) REFERENCES `producto` (`id_producto`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Volcando datos para la tabla depan_db.linea_pedido: ~0 rows (aproximadamente)

-- Volcando estructura para tabla depan_db.pedido
CREATE TABLE IF NOT EXISTS `pedido` (
  `id_pedido` int(11) NOT NULL AUTO_INCREMENT,
  `id_usuario_cliente` int(11) NOT NULL,
  `id_repartidor` int(11) DEFAULT NULL,
  `numero_pedido` varchar(50) NOT NULL,
  `fecha_pedido` datetime NOT NULL DEFAULT current_timestamp(),
  `subtotal` decimal(10,2) NOT NULL CHECK (`subtotal` >= 0),
  `gastos_envio` decimal(10,2) NOT NULL DEFAULT 0.00 CHECK (`gastos_envio` >= 0),
  `total` decimal(10,2) NOT NULL CHECK (`total` >= 0),
  `estado` enum('pendiente','preparando','enviado','entregado','cancelado') NOT NULL DEFAULT 'pendiente',
  `direccion_entrega` varchar(255) NOT NULL,
  `ciudad_entrega` varchar(100) NOT NULL,
  `codigo_postal_entrega` varchar(10) NOT NULL,
  `telefono_contacto` varchar(20) NOT NULL,
  `notas` text DEFAULT NULL,
  `fecha_entrega_estimada` datetime DEFAULT NULL,
  `fecha_entrega_real` datetime DEFAULT NULL,
  PRIMARY KEY (`id_pedido`),
  UNIQUE KEY `numero_pedido` (`numero_pedido`),
  KEY `idx_usuario_cliente` (`id_usuario_cliente`),
  KEY `idx_repartidor` (`id_repartidor`),
  KEY `idx_estado` (`estado`),
  KEY `idx_fecha_pedido` (`fecha_pedido`),
  KEY `idx_numero_pedido` (`numero_pedido`),
  CONSTRAINT `pedido_ibfk_1` FOREIGN KEY (`id_usuario_cliente`) REFERENCES `usuario` (`id_usuario`),
  CONSTRAINT `pedido_ibfk_2` FOREIGN KEY (`id_repartidor`) REFERENCES `usuario` (`id_usuario`) ON DELETE SET NULL
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Volcando datos para la tabla depan_db.pedido: ~8 rows (aproximadamente)
INSERT INTO `pedido` (`id_pedido`, `id_usuario_cliente`, `id_repartidor`, `numero_pedido`, `fecha_pedido`, `subtotal`, `gastos_envio`, `total`, `estado`, `direccion_entrega`, `ciudad_entrega`, `codigo_postal_entrega`, `telefono_contacto`, `notas`, `fecha_entrega_estimada`, `fecha_entrega_real`) VALUES
	(1, 5, NULL, 'DP-20251101-6B7745FC', '2025-11-01 09:59:50', 1.50, 2.50, 4.00, 'entregado', 'Lugar Montalvo 18', 'Montalvo', '36960', '606877792', 'Dejar en la puerta y timbrar.', '2025-11-03 09:59:50', '2025-11-08 14:48:36'),
	(2, 5, NULL, 'DP-20251101-F02CB3D2', '2025-11-01 10:00:02', 1.50, 2.50, 4.00, 'cancelado', 'Lugar Montalvo 18', 'Montalvo', '36960', '606877792', 'Dejar en la puerta y timbrar.', '2025-11-03 10:00:02', NULL),
	(3, 5, NULL, 'DP-20251101-B7C45AF5', '2025-11-01 10:00:41', 1.50, 2.50, 4.00, 'enviado', 'Lugar Montalvo 18', 'Montalvo', '36960', '606877792', NULL, '2025-11-03 10:00:41', NULL),
	(4, 5, NULL, 'DP-20251104-DE72F0AB', '2025-11-04 21:42:09', 7.50, 2.50, 10.00, 'entregado', 'Lugar Montalvo 18', 'Montalvo', '36960', '60687792', 'Ayuda en el portal estoy atrapado', '2025-11-06 21:42:09', '2025-11-08 14:48:46'),
	(5, 4, NULL, 'DP-20251108-FCB94D29', '2025-11-08 14:31:54', 6.00, 2.50, 8.50, 'preparando', 'Lugar Montalvo 18', 'Montalvo', '36960', '60687792', NULL, '2025-11-10 14:31:54', NULL),
	(6, 4, NULL, 'DP-20251108-2FD07B7A', '2025-11-08 14:49:18', 6.00, 2.50, 8.50, 'enviado', 'Lugar Montalvo 18', 'Montalvo', '36960', '60687792', NULL, '2025-11-10 14:49:18', NULL),
	(7, 4, NULL, 'DP-20251108-C953F0D4', '2025-11-08 14:53:29', 6.00, 2.50, 8.50, 'entregado', 'Lugar Montalvo 18', 'Montalvo', '36960', '60687792', NULL, '2025-11-10 14:53:29', '2025-11-08 14:58:57'),
	(8, 4, NULL, 'DP-20251108-ED98E818', '2025-11-08 14:59:49', 4.50, 2.50, 7.00, 'pendiente', 'Lugar Montalvo 18', 'Montalvo', '36960', '60687792', NULL, '2025-11-10 14:59:49', NULL);

-- Volcando estructura para tabla depan_db.producto
CREATE TABLE IF NOT EXISTS `producto` (
  `id_producto` int(11) NOT NULL AUTO_INCREMENT,
  `id_categoria` int(11) NOT NULL,
  `nombre` varchar(200) NOT NULL,
  `descripcion` text DEFAULT NULL,
  `precio` decimal(10,2) NOT NULL CHECK (`precio` >= 0),
  `imagen_url` varchar(500) DEFAULT NULL,
  `stock` int(11) NOT NULL DEFAULT 0 CHECK (`stock` >= 0),
  `disponible` tinyint(1) NOT NULL DEFAULT 1,
  `fecha_creacion` datetime NOT NULL DEFAULT current_timestamp(),
  `fecha_actualizacion` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`id_producto`),
  KEY `idx_categoria` (`id_categoria`),
  KEY `idx_disponible` (`disponible`),
  KEY `idx_nombre` (`nombre`),
  FULLTEXT KEY `idx_busqueda` (`nombre`,`descripcion`),
  CONSTRAINT `producto_ibfk_1` FOREIGN KEY (`id_categoria`) REFERENCES `categoria` (`id_categoria`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Volcando datos para la tabla depan_db.producto: ~9 rows (aproximadamente)
INSERT INTO `producto` (`id_producto`, `id_categoria`, `nombre`, `descripcion`, `precio`, `imagen_url`, `stock`, `disponible`, `fecha_creacion`, `fecha_actualizacion`) VALUES
	(1, 1, 'Barra de Pan Gallega', 'Pan tradicional gallego de 500g', 1.20, '/images/pan-gallego.jpg', 50, 1, '2025-10-20 22:19:28', '2025-10-20 22:19:28'),
	(2, 1, 'Pan Integral', 'Pan integral con semillas de 400g', 1.80, '/images/pan-integral.jpg', 30, 1, '2025-10-20 22:19:28', '2025-10-20 22:19:28'),
	(3, 1, 'Hogaza Rústica', 'Hogaza artesanal de masa madre 800g', 3.50, '/images/hogaza.jpg', 20, 1, '2025-10-20 22:19:28', '2025-11-08 13:49:33'),
	(4, 2, 'Croissant', 'Croissant de mantequilla', 1.50, '/images/croissant.jpg', 34, 1, '2025-10-20 22:19:28', '2025-11-08 15:03:55'),
	(5, 2, 'Napolitana de Chocolate', 'Napolitana rellena de chocolate', 1.80, '/images/napolitana.jpg', 35, 1, '2025-10-20 22:19:28', '2025-10-20 22:19:28'),
	(6, 2, 'Ensaimada', 'Ensaimada tradicional mallorquina', 2.20, '/images/ensaimada.jpg', 25, 0, '2025-10-20 22:19:28', '2025-10-27 23:33:33'),
	(7, 3, 'Tarta de Santiago', 'Tarta de almendra tradicional gallega', 15.00, '/images/tarta-santiago.jpg', 10, 1, '2025-10-20 22:19:28', '2025-10-20 22:19:28'),
	(8, 3, 'Tarta de Chocolate', 'Tarta de chocolate con cobertura de cacao', 18.00, '/images/tarta-chocolate.jpg', 8, 1, '2025-10-20 22:19:28', '2025-10-20 22:19:28'),
	(9, 3, 'Pastel de Nata', 'Pastel de nata portugués (6 unidades)', 6.50, '/images/pastel-nata.jpg', 30, 1, '2025-10-20 22:19:28', '2025-10-20 22:19:28');

-- Volcando estructura para tabla depan_db.seguimiento_pedido
CREATE TABLE IF NOT EXISTS `seguimiento_pedido` (
  `id_seguimiento` int(11) NOT NULL AUTO_INCREMENT,
  `id_pedido` int(11) NOT NULL,
  `estado` enum('pendiente','preparando','enviado','entregado','cancelado') NOT NULL,
  `fecha_estado` datetime NOT NULL DEFAULT current_timestamp(),
  `observaciones` text DEFAULT NULL,
  `latitud` decimal(10,8) DEFAULT NULL COMMENT 'Ubicación del repartidor',
  `longitud` decimal(11,8) DEFAULT NULL COMMENT 'Ubicación del repartidor',
  PRIMARY KEY (`id_seguimiento`),
  KEY `idx_pedido` (`id_pedido`),
  KEY `idx_estado` (`estado`),
  KEY `idx_fecha_estado` (`fecha_estado`),
  CONSTRAINT `seguimiento_pedido_ibfk_1` FOREIGN KEY (`id_pedido`) REFERENCES `pedido` (`id_pedido`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Volcando datos para la tabla depan_db.seguimiento_pedido: ~15 rows (aproximadamente)
INSERT INTO `seguimiento_pedido` (`id_seguimiento`, `id_pedido`, `estado`, `fecha_estado`, `observaciones`, `latitud`, `longitud`) VALUES
	(1, 1, 'pendiente', '2025-11-01 09:59:50', 'Pedido creado', NULL, NULL),
	(2, 2, 'pendiente', '2025-11-01 10:00:02', 'Pedido creado', NULL, NULL),
	(3, 3, 'pendiente', '2025-11-01 10:00:41', 'Pedido creado', NULL, NULL),
	(4, 4, 'pendiente', '2025-11-04 21:42:09', 'Pedido creado', NULL, NULL),
	(5, 5, 'pendiente', '2025-11-08 14:31:54', 'Pedido creado', NULL, NULL),
	(6, 5, 'preparando', '2025-11-08 14:48:21', 'Estado cambiado de pendiente a preparando', NULL, NULL),
	(7, 3, 'enviado', '2025-11-08 14:48:31', 'Estado cambiado de pendiente a enviado', NULL, NULL),
	(8, 1, 'entregado', '2025-11-08 14:48:36', 'Estado cambiado de pendiente a entregado', NULL, NULL),
	(9, 2, 'cancelado', '2025-11-08 14:48:40', 'Estado cambiado de pendiente a cancelado', NULL, NULL),
	(10, 4, 'entregado', '2025-11-08 14:48:46', 'Estado cambiado de pendiente a entregado', NULL, NULL),
	(11, 6, 'pendiente', '2025-11-08 14:49:18', 'Pedido creado', NULL, NULL),
	(12, 7, 'pendiente', '2025-11-08 14:53:29', 'Pedido creado', NULL, NULL),
	(13, 6, 'enviado', '2025-11-08 14:58:49', 'Estado cambiado de pendiente a enviado', NULL, NULL),
	(14, 7, 'entregado', '2025-11-08 14:58:57', 'Estado cambiado de pendiente a entregado', NULL, NULL),
	(15, 8, 'pendiente', '2025-11-08 14:59:49', 'Pedido creado', NULL, NULL);

-- Volcando estructura para procedimiento depan_db.sp_crear_pedido_desde_carrito
DELIMITER //
CREATE PROCEDURE `sp_crear_pedido_desde_carrito`(
    IN p_id_usuario INT,
    IN p_direccion VARCHAR(255),
    IN p_ciudad VARCHAR(100),
    IN p_codigo_postal VARCHAR(10),
    IN p_telefono VARCHAR(20),
    IN p_gastos_envio DECIMAL(10,2),
    OUT p_id_pedido_nuevo INT,
    OUT p_numero_pedido VARCHAR(50)
)
BEGIN
    DECLARE v_subtotal DECIMAL(10,2);
    DECLARE v_total DECIMAL(10,2);
    DECLARE v_numero_pedido VARCHAR(50);
    
    -- Generar número de pedido único
    SET v_numero_pedido = CONCAT('PED-', DATE_FORMAT(NOW(), '%Y%m%d'), '-', LPAD(FLOOR(RAND() * 10000), 4, '0'));
    
    -- Obtener subtotal del carrito
    SELECT total INTO v_subtotal
    FROM CARRITO
    WHERE id_usuario = p_id_usuario;
    
    -- Calcular total
    SET v_total = v_subtotal + p_gastos_envio;
    
    -- Crear el pedido
    INSERT INTO PEDIDO (
        id_usuario_cliente, numero_pedido, subtotal, gastos_envio, total,
        direccion_entrega, ciudad_entrega, codigo_postal_entrega, telefono_contacto
    ) VALUES (
        p_id_usuario, v_numero_pedido, v_subtotal, p_gastos_envio, v_total,
        p_direccion, p_ciudad, p_codigo_postal, p_telefono
    );
    
    SET p_id_pedido_nuevo = LAST_INSERT_ID();
    SET p_numero_pedido = v_numero_pedido;
    
    -- Copiar líneas del carrito al pedido
    INSERT INTO LINEA_PEDIDO (id_pedido, id_producto, nombre_producto, cantidad, precio_unitario, subtotal)
    SELECT 
        p_id_pedido_nuevo,
        lc.id_producto,
        pr.nombre,
        lc.cantidad,
        lc.precio_unitario,
        lc.subtotal
    FROM LINEA_CARRITO lc
    INNER JOIN CARRITO c ON lc.id_carrito = c.id_carrito
    INNER JOIN PRODUCTO pr ON lc.id_producto = pr.id_producto
    WHERE c.id_usuario = p_id_usuario;
    
    -- Vaciar el carrito
    DELETE FROM LINEA_CARRITO 
    WHERE id_carrito IN (SELECT id_carrito FROM CARRITO WHERE id_usuario = p_id_usuario);
    
END//
DELIMITER ;

-- Volcando estructura para tabla depan_db.usuario
CREATE TABLE IF NOT EXISTS `usuario` (
  `id_usuario` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(255) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellidos` varchar(150) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `direccion` varchar(255) DEFAULT NULL,
  `ciudad` varchar(100) DEFAULT NULL,
  `codigo_postal` varchar(10) DEFAULT NULL,
  `rol` enum('cliente','repartidor','administrador') NOT NULL DEFAULT 'cliente',
  `fecha_registro` datetime NOT NULL DEFAULT current_timestamp(),
  `activo` tinyint(1) NOT NULL DEFAULT 1,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `email` (`email`),
  KEY `idx_email` (`email`),
  KEY `idx_rol` (`rol`),
  KEY `idx_activo` (`activo`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Volcando datos para la tabla depan_db.usuario: ~5 rows (aproximadamente)
INSERT INTO `usuario` (`id_usuario`, `email`, `password_hash`, `nombre`, `apellidos`, `telefono`, `direccion`, `ciudad`, `codigo_postal`, `rol`, `fecha_registro`, `activo`) VALUES
	(1, 'admin@depan.com', '$2a$11$ejemplo_hash_bcrypt', 'Admin', 'Sistema', NULL, NULL, NULL, NULL, 'administrador', '2025-10-20 22:19:28', 1),
	(2, 'repartidor@depan.com', '$2a$11$ejemplo_hash_bcrypt', 'Juan', 'Repartidor', '666777888', NULL, NULL, NULL, 'repartidor', '2025-10-20 22:19:28', 1),
	(3, 'cliente@ejemplo.com', '$2a$11$ejemplo_hash_bcrypt', 'María', 'García López', '655444333', 'Calle Ejemplo 123', 'Vigo', '36201', 'cliente', '2025-10-20 22:19:28', 1),
	(4, 'denisfernandezcastro12@gmail.com', 'bKE9UspwyIPg8LsQHkJaiehiTeUdstI5JZOvaoQRgJA=', 'Denis', 'Fernández Castro', '606877792', 'Lugar Montalvo 8', 'Pontevedra', '36969', 'administrador', '2025-10-20 22:20:31', 1),
	(5, 'denisfernandezcastro123@gmail.com', 'bKE9UspwyIPg8LsQHkJaiehiTeUdstI5JZOvaoQRgJA=', 'Denis', 'Fernández Castro', '606877792', 'Lugar Montalvo 8', 'Pontevedra', '36969', 'cliente', '2025-10-20 22:20:48', 1),
	(6, 'denisfernandezcastro1234@gmail.com', 'bKE9UspwyIPg8LsQHkJaiehiTeUdstI5JZOvaoQRgJA=', 'Denis', 'Fernández Castro', '606877792', 'Lugar Montalvo 8', 'Pontevedra', '36969', 'cliente', '2025-10-20 22:22:47', 1);

-- Volcando estructura para tabla depan_db.valoracion
CREATE TABLE IF NOT EXISTS `valoracion` (
  `id_valoracion` int(11) NOT NULL AUTO_INCREMENT,
  `id_usuario` int(11) NOT NULL,
  `id_producto` int(11) NOT NULL,
  `puntuacion` int(11) NOT NULL CHECK (`puntuacion` between 1 and 5),
  `comentario` text DEFAULT NULL,
  `fecha_valoracion` datetime NOT NULL DEFAULT current_timestamp(),
  PRIMARY KEY (`id_valoracion`),
  UNIQUE KEY `uk_usuario_producto` (`id_usuario`,`id_producto`),
  KEY `idx_producto` (`id_producto`),
  KEY `idx_puntuacion` (`puntuacion`),
  KEY `idx_fecha` (`fecha_valoracion`),
  CONSTRAINT `valoracion_ibfk_1` FOREIGN KEY (`id_usuario`) REFERENCES `usuario` (`id_usuario`) ON DELETE CASCADE,
  CONSTRAINT `valoracion_ibfk_2` FOREIGN KEY (`id_producto`) REFERENCES `producto` (`id_producto`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Volcando datos para la tabla depan_db.valoracion: ~0 rows (aproximadamente)

-- Volcando estructura para vista depan_db.v_pedidos_completos
-- Creando tabla temporal para superar errores de dependencia de VIEW
CREATE TABLE `v_pedidos_completos` (
	`id_pedido` INT(11) NOT NULL,
	`numero_pedido` VARCHAR(1) NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`fecha_pedido` DATETIME NOT NULL,
	`estado` ENUM('pendiente','preparando','enviado','entregado','cancelado') NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`total` DECIMAL(10,2) NOT NULL,
	`cliente_nombre` VARCHAR(1) NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`cliente_apellidos` VARCHAR(1) NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`cliente_email` VARCHAR(1) NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`cliente_telefono` VARCHAR(1) NULL COLLATE 'utf8mb4_unicode_ci',
	`direccion_entrega` VARCHAR(1) NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`repartidor_nombre` VARCHAR(1) NULL COLLATE 'utf8mb4_unicode_ci',
	`repartidor_apellidos` VARCHAR(1) NULL COLLATE 'utf8mb4_unicode_ci'
);

-- Volcando estructura para vista depan_db.v_pedidos_en_curso
-- Creando tabla temporal para superar errores de dependencia de VIEW
CREATE TABLE `v_pedidos_en_curso` (
	`id_pedido` INT(11) NOT NULL,
	`numero_pedido` VARCHAR(1) NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`estado` ENUM('pendiente','preparando','enviado','entregado','cancelado') NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`fecha_pedido` DATETIME NOT NULL,
	`cliente` VARCHAR(1) NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`repartidor` VARCHAR(1) NULL COLLATE 'utf8mb4_unicode_ci',
	`latitud` DECIMAL(10,8) NULL COMMENT 'Ubicación del repartidor',
	`longitud` DECIMAL(11,8) NULL COMMENT 'Ubicación del repartidor',
	`ultima_actualizacion` DATETIME NULL
);

-- Volcando estructura para vista depan_db.v_productos_mas_vendidos
-- Creando tabla temporal para superar errores de dependencia de VIEW
CREATE TABLE `v_productos_mas_vendidos` (
	`id_producto` INT(11) NOT NULL,
	`nombre` VARCHAR(1) NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`precio` DECIMAL(10,2) NOT NULL,
	`categoria` VARCHAR(1) NOT NULL COLLATE 'utf8mb4_unicode_ci',
	`total_ventas` BIGINT(21) NOT NULL,
	`unidades_vendidas` DECIMAL(32,0) NULL,
	`ingresos_totales` DECIMAL(32,2) NULL
);

-- Volcando estructura para tabla depan_db.__efmigrationshistory
CREATE TABLE IF NOT EXISTS `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_uca1400_ai_ci;

-- Volcando datos para la tabla depan_db.__efmigrationshistory: ~0 rows (aproximadamente)

-- Volcando estructura para disparador depan_db.trg_actualizar_total_carrito_delete
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER trg_actualizar_total_carrito_delete
AFTER DELETE ON LINEA_CARRITO
FOR EACH ROW
BEGIN
    UPDATE CARRITO 
    SET total = (
        SELECT COALESCE(SUM(subtotal), 0) 
        FROM LINEA_CARRITO 
        WHERE id_carrito = OLD.id_carrito
    )
    WHERE id_carrito = OLD.id_carrito;
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

-- Volcando estructura para disparador depan_db.trg_actualizar_total_carrito_insert
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER trg_actualizar_total_carrito_insert
AFTER INSERT ON LINEA_CARRITO
FOR EACH ROW
BEGIN
    UPDATE CARRITO 
    SET total = (
        SELECT COALESCE(SUM(subtotal), 0) 
        FROM LINEA_CARRITO 
        WHERE id_carrito = NEW.id_carrito
    )
    WHERE id_carrito = NEW.id_carrito;
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

-- Volcando estructura para disparador depan_db.trg_actualizar_total_carrito_update
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER trg_actualizar_total_carrito_update
AFTER UPDATE ON LINEA_CARRITO
FOR EACH ROW
BEGIN
    UPDATE CARRITO 
    SET total = (
        SELECT COALESCE(SUM(subtotal), 0) 
        FROM LINEA_CARRITO 
        WHERE id_carrito = NEW.id_carrito
    )
    WHERE id_carrito = NEW.id_carrito;
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

-- Volcando estructura para disparador depan_db.trg_crear_seguimiento_inicial
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER trg_crear_seguimiento_inicial
AFTER INSERT ON PEDIDO
FOR EACH ROW
BEGIN
    INSERT INTO SEGUIMIENTO_PEDIDO (id_pedido, estado, observaciones)
    VALUES (NEW.id_pedido, NEW.estado, 'Pedido creado');
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

-- Volcando estructura para disparador depan_db.trg_registrar_cambio_estado_pedido
SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='STRICT_TRANS_TABLES,ERROR_FOR_DIVISION_BY_ZERO,NO_AUTO_CREATE_USER,NO_ENGINE_SUBSTITUTION';
DELIMITER //
CREATE TRIGGER trg_registrar_cambio_estado_pedido
AFTER UPDATE ON PEDIDO
FOR EACH ROW
BEGIN
    IF OLD.estado != NEW.estado THEN
        INSERT INTO SEGUIMIENTO_PEDIDO (id_pedido, estado, observaciones)
        VALUES (NEW.id_pedido, NEW.estado, CONCAT('Estado cambiado de ', OLD.estado, ' a ', NEW.estado));
    END IF;
END//
DELIMITER ;
SET SQL_MODE=@OLDTMP_SQL_MODE;

-- Eliminando tabla temporal y crear estructura final de VIEW
DROP TABLE IF EXISTS `v_pedidos_completos`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `v_pedidos_completos` AS SELECT 
    p.id_pedido,
    p.numero_pedido,
    p.fecha_pedido,
    p.estado,
    p.total,
    u.nombre AS cliente_nombre,
    u.apellidos AS cliente_apellidos,
    u.email AS cliente_email,
    u.telefono AS cliente_telefono,
    p.direccion_entrega,
    r.nombre AS repartidor_nombre,
    r.apellidos AS repartidor_apellidos
FROM PEDIDO p
INNER JOIN USUARIO u ON p.id_usuario_cliente = u.id_usuario
LEFT JOIN USUARIO r ON p.id_repartidor = r.id_usuario 
;

-- Eliminando tabla temporal y crear estructura final de VIEW
DROP TABLE IF EXISTS `v_pedidos_en_curso`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `v_pedidos_en_curso` AS SELECT 
    p.id_pedido,
    p.numero_pedido,
    p.estado,
    p.fecha_pedido,
    u.nombre AS cliente,
    r.nombre AS repartidor,
    sp.latitud,
    sp.longitud,
    sp.fecha_estado AS ultima_actualizacion
FROM PEDIDO p
INNER JOIN USUARIO u ON p.id_usuario_cliente = u.id_usuario
LEFT JOIN USUARIO r ON p.id_repartidor = r.id_usuario
LEFT JOIN (
    SELECT id_pedido, latitud, longitud, fecha_estado
    FROM SEGUIMIENTO_PEDIDO sp1
    WHERE fecha_estado = (
        SELECT MAX(fecha_estado)
        FROM SEGUIMIENTO_PEDIDO sp2
        WHERE sp1.id_pedido = sp2.id_pedido
    )
) sp ON p.id_pedido = sp.id_pedido
WHERE p.estado IN ('preparando', 'enviado') 
;

-- Eliminando tabla temporal y crear estructura final de VIEW
DROP TABLE IF EXISTS `v_productos_mas_vendidos`;
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `v_productos_mas_vendidos` AS SELECT 
    pr.id_producto,
    pr.nombre,
    pr.precio,
    c.nombre AS categoria,
    COUNT(lp.id_linea_pedido) AS total_ventas,
    SUM(lp.cantidad) AS unidades_vendidas,
    SUM(lp.subtotal) AS ingresos_totales
FROM PRODUCTO pr
INNER JOIN CATEGORIA c ON pr.id_categoria = c.id_categoria
LEFT JOIN LINEA_PEDIDO lp ON pr.id_producto = lp.id_producto
GROUP BY pr.id_producto, pr.nombre, pr.precio, c.nombre
ORDER BY unidades_vendidas DESC 
;

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
