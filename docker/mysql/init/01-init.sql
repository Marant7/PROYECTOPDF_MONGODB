-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Versión del servidor:         10.4.32-MariaDB - mariadb.org binary distribution
-- SO del servidor:              Win64
-- HeidiSQL Versión:             12.8.0.6908
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


-- Volcando estructura de base de datos para pdfsolutions_test
CREATE DATABASE IF NOT EXISTS `pdfsolutions_test` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci */;
USE `pdfsolutions_test`;

-- Volcando estructura para tabla pdfsolutions_test.detalles_suscripciones
CREATE TABLE IF NOT EXISTS `detalles_suscripciones` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `tipo_suscripcion` varchar(50) DEFAULT NULL,
  `fecha_inicio` datetime DEFAULT NULL,
  `fecha_final` datetime DEFAULT NULL,
  `precio` decimal(20,6) DEFAULT NULL,
  `operaciones_realizadas` int(11) DEFAULT NULL,
  `usuario_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_detalles_suscripciones_usuarios` (`usuario_id`),
  CONSTRAINT `FK_detalles_suscripciones_usuarios` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla pdfsolutions_test.detalles_suscripciones: ~1 rows (aproximadamente)
INSERT INTO `detalles_suscripciones` (`id`, `tipo_suscripcion`, `fecha_inicio`, `fecha_final`, `precio`, `operaciones_realizadas`, `usuario_id`) VALUES
	(1, 'basico', '2024-11-23 04:26:20', '2025-11-23 04:26:20', 0.000000, 5, 5);

-- Volcando estructura para tabla pdfsolutions_test.operaciones_pdf
CREATE TABLE IF NOT EXISTS `operaciones_pdf` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `usuario_id` int(11) DEFAULT NULL,
  `TipoOperacion` varchar(50) DEFAULT NULL,
  `fechaOperacion` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `FK_operaciones_pdf_usuarios` (`usuario_id`),
  CONSTRAINT `FK_operaciones_pdf_usuarios` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla pdfsolutions_test.operaciones_pdf: ~1 rows (aproximadamente)
INSERT INTO `operaciones_pdf` (`id`, `usuario_id`, `TipoOperacion`, `fechaOperacion`) VALUES
	(1, 5, 'Cortar', '2024-11-23 04:49:50');

-- Volcando estructura para tabla pdfsolutions_test.usuarios
CREATE TABLE IF NOT EXISTS `usuarios` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) DEFAULT NULL,
  `correo` varchar(50) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Volcando datos para la tabla pdfsolutions_test.usuarios: ~5 rows (aproximadamente)
INSERT INTO `usuarios` (`id`, `nombre`, `correo`, `password`) VALUES
	(1, 'Usuario Test', 'test638679205450426115@test.com', 'password123'),
	(2, 'Usuario Lista', 'lista638679205452145661@test.com', 'password123'),
	(3, 'Usuario 1', 'test638679205453015913@test.com', 'password123'),
	(4, 'Nuevo Usuario', 'nuevo638679205454206188@test.com', 'password123'),
	(5, 'mario', 'mario@gmail.com', '123');

/*!40103 SET TIME_ZONE=IFNULL(@OLD_TIME_ZONE, 'system') */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IFNULL(@OLD_FOREIGN_KEY_CHECKS, 1) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40111 SET SQL_NOTES=IFNULL(@OLD_SQL_NOTES, 1) */;
