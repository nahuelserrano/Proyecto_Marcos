-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 05-06-2025 a las 18:21:56
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `truck_manager_project_db`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `camion`
--

CREATE TABLE `camion` (
  `idcamion` int(11) NOT NULL,
  `patente` varchar(45) NOT NULL,
  `nombre_chofer` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `camion`
--

INSERT INTO `camion` (`idcamion`, `patente`, `nombre_chofer`) VALUES
(2, 'HIJ429', 'x'),
(5, 'WWW123', 'carlos'),
(11, 'PUC111', 'JUAN'),
(12, 'MLA126', 'Pepito'),
(14, 'NCS234', 'Mili'),
(17, 'KJH921', 'Vicky'),
(21, 'pruebaagregar1', 'nuevoPrueba1'),
(22, 'pruebaagregar2', 'nuevoPrueba2'),
(24, 'HOLA1234', 'Vicky'),
(25, 'JJJ999', 'inventado'),
(26, 'HHH888', 'inventado2');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cheque`
--

CREATE TABLE `cheque` (
  `idcheque` int(11) NOT NULL,
  `fecha_ingreso` date NOT NULL,
  `nro_cheque` int(11) NOT NULL,
  `monto` float NOT NULL,
  `banco` varchar(45) NOT NULL,
  `fecha_cobro` date NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `numero_personalizado` int(11) DEFAULT NULL,
  `fecha_vencimieto` date DEFAULT NULL,
  `entregado_a` varchar(45) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `cheque`
--

INSERT INTO `cheque` (`idcheque`, `fecha_ingreso`, `nro_cheque`, `monto`, `banco`, `fecha_cobro`, `nombre`, `numero_personalizado`, `fecha_vencimieto`, `entregado_a`) VALUES
(1, '2025-05-19', 54321, 2000, 'Banco Control', '2025-06-18', '', NULL, '2025-06-18', NULL),
(2, '2025-05-19', 12345, 6000, 'Banco Nación', '2025-06-18', '', NULL, '2025-06-18', NULL),
(3, '2021-12-12', 1234, 150000, 'Banco Nacion', '2021-12-12', 'Marcos', 1234, '2022-12-12', 'cristian');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `chofer`
--

CREATE TABLE `chofer` (
  `idChofer` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `chofer`
--

INSERT INTO `chofer` (`idChofer`, `nombre`) VALUES
(1, 'Mili'),
(4, 'nuevo chofer'),
(5, 'Vicky'),
(6, 'nuevoPrueba1'),
(10, 'nuevoPrueba2'),
(12, 'Vicky'),
(13, 'inventado'),
(14, 'inventado2');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cliente`
--

CREATE TABLE `cliente` (
  `idCliente` int(11) NOT NULL,
  `nombre` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `cliente`
--

INSERT INTO `cliente` (`idCliente`, `nombre`) VALUES
(2, 'Cliente1'),
(3, 'COOPERATIVA'),
(4, 'MACHACA'),
(5, 'TRANSPORTES DEL SUR'),
(6, 'CARGA PESADA SA'),
(7, 'DISTRIBUIDORA TANDIL'),
(8, 'PRUEBACLIENTE1'),
(20, 'CLIENTEPRUEBA'),
(21, 'PRUEBACORRECCION'),
(22, 'PRUEBACORRECCION2');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `cuenta_corriente`
--

CREATE TABLE `cuenta_corriente` (
  `idcuenta_corriente` int(11) NOT NULL,
  `fecha_factura` date NOT NULL,
  `nro_factura` int(11) NOT NULL,
  `adeuda` float NOT NULL,
  `importe_pagado` float NOT NULL,
  `idCliente` int(11) DEFAULT NULL,
  `saldo` float NOT NULL,
  `idfletero` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `cuenta_corriente`
--

INSERT INTO `cuenta_corriente` (`idcuenta_corriente`, `fecha_factura`, `nro_factura`, `adeuda`, `importe_pagado`, `idCliente`, `saldo`, `idfletero`) VALUES
(1, '2025-04-07', 3333, 2345, 2344, 2, 0, NULL),
(4, '2025-05-13', 8888, 10000, 0, 2, 10000, NULL),
(5, '2025-05-13', 9999, 14000, 2000, 2, 22000, NULL),
(6, '2025-05-13', 10000, 35000, 1000, 2, 56000, NULL),
(9, '2025-05-11', 92, 1000, 10000, NULL, -9000, 6);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `fletero`
--

CREATE TABLE `fletero` (
  `idFletero` int(11) NOT NULL,
  `nombre` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `fletero`
--

INSERT INTO `fletero` (`idFletero`, `nombre`) VALUES
(1, 'Bernabé'),
(2, 'Marcelo'),
(4, 'JUAN'),
(6, 'CARLOS'),
(7, 'ANITA'),
(9, 'FLETEROPRUEBA'),
(12, 'FLETERONUEVOO'),
(13, 'MARCELO');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

CREATE TABLE `pago` (
  `idpago` int(11) NOT NULL,
  `monto` float NOT NULL,
  `idChofer` int(11) DEFAULT NULL,
  `pagado` tinyint(4) DEFAULT NULL,
  `idViaje` int(11) NOT NULL,
  `idSueldo` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pago`
--

INSERT INTO `pago` (`idpago`, `monto`, `idChofer`, `pagado`, `idViaje`, `idSueldo`) VALUES
(1, 0, 4, 0, 9, NULL),
(2, 4000000000, 5, 0, 11, NULL),
(3, 4000000000, 5, 0, 12, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `sueldo`
--

CREATE TABLE `sueldo` (
  `idsueldo` int(11) NOT NULL,
  `idchofer` int(11) DEFAULT NULL,
  `fecha_desde` date DEFAULT NULL,
  `fecha_hasta` date DEFAULT NULL,
  `fecha_pago` date DEFAULT NULL,
  `monto_total` float NOT NULL,
  `idCamion` int(11) DEFAULT NULL,
  `pagado` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `sueldo`
--

INSERT INTO `sueldo` (`idsueldo`, `idchofer`, `fecha_desde`, `fecha_hasta`, `fecha_pago`, `monto_total`, `idCamion`, `pagado`) VALUES
(5, 1, '2025-05-01', '2025-05-31', '2025-05-29', 22222, 2, 1),
(6, 4, '2025-04-01', '2025-04-30', NULL, 84848, 14, 0),
(9, 4, '2025-05-01', '2025-05-31', NULL, 45000, NULL, 0),
(10, NULL, '2025-05-01', '2025-05-31', '2025-05-29', 100000, 2, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `viaje`
--

CREATE TABLE `viaje` (
  `idviaje` int(11) NOT NULL,
  `partida` date NOT NULL,
  `origen` varchar(45) NOT NULL,
  `destino` varchar(45) NOT NULL,
  `remito` int(11) DEFAULT NULL,
  `kg` float NOT NULL,
  `carga` varchar(180) DEFAULT NULL,
  `idcliente` int(11) NOT NULL,
  `idcamion` int(11) NOT NULL,
  `km` float DEFAULT NULL,
  `tarifa` float NOT NULL,
  `nombre_chofer` varchar(45) NOT NULL,
  `comision_chofer` float NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `viaje`
--

INSERT INTO `viaje` (`idviaje`, `partida`, `origen`, `destino`, `remito`, `kg`, `carga`, `idcliente`, `idcamion`, `km`, `tarifa`, `nombre_chofer`, `comision_chofer`) VALUES
(1, '2025-04-05', 'Tandil', 'Buenos Aires', 10001, 5000, 'Cereales', 3, 2, 370, 25, 'x', 0.18),
(4, '2025-04-12', 'Córdoba', 'Mendoza', 10004, 8000, 'Maquinaria', 4, 5, 700, 35, 'carlos', 0.18),
(7, '2025-04-20', 'Buenos Aires', 'Córdoba', 10007, 9000, 'Electrónicos', 7, 11, 700, 36, 'roman', 0.18),
(8, '2025-04-22', 'Rosario', 'Santa Fe', 10008, 6700, 'Papel', 7, 12, 160, 15, 'Pepito', 0.18),
(9, '2025-04-28', 'Tandil', 'Azul', 123, 30, 'trigo', 2, 11, 350.5, 5000, 'JUAN', 0),
(11, '2025-05-29', 'Tandil', 'Necochea', 8888, 500, 'trigo', 3, 17, 789, 40000, 'Vicky', 20),
(12, '2025-05-30', 'Tandil', 'Azul', 9999, 500, 'maiz', 4, 17, 789, 40000, 'Vicky', 20);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `viaje_flete`
--

CREATE TABLE `viaje_flete` (
  `idviaje_flete` int(11) NOT NULL,
  `origen` varchar(45) NOT NULL,
  `destino` varchar(45) NOT NULL,
  `remito` float DEFAULT NULL,
  `carga` varchar(45) DEFAULT NULL,
  `km` float DEFAULT NULL,
  `kg` float DEFAULT NULL,
  `tarifa` float DEFAULT NULL,
  `factura` int(11) DEFAULT NULL,
  `idCliente` int(11) DEFAULT NULL,
  `fletero` int(11) DEFAULT NULL,
  `nombre_chofer` varchar(45) NOT NULL,
  `comision` float DEFAULT NULL,
  `fecha_salida` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `viaje_flete`
--

INSERT INTO `viaje_flete` (`idviaje_flete`, `origen`, `destino`, `remito`, `carga`, `km`, `kg`, `tarifa`, `factura`, `idCliente`, `fletero`, `nombre_chofer`, `comision`, `fecha_salida`) VALUES
(1, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(13, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 6, 'Chofer de Carlos', 10, '2025-04-11'),
(15, 'Tandil', 'Ayacucho', 19292, 'trigo', 150, 300, 19000, 12244, 3, 1, 'Justo', 10, '2025-04-15'),
(16, 'Pilar', 'Tandil', 2000, 'soja', 650, 880, 35000, 1235, 3, 2, 'Patricio', 15, '2025-04-10'),
(17, 'Tandil', 'Azul', 40, 'trigo', 120, 130, 19000, 9998, 3, 6, 'Chofer de Carlos', 10, '2025-04-11'),
(18, 'Tandil', 'MDP', 1245, 'trigo', 200, 500, 20000, 382882, 4, 2, 'lauty', 10, '2025-01-12');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `camion`
--
ALTER TABLE `camion`
  ADD PRIMARY KEY (`idcamion`);

--
-- Indices de la tabla `cheque`
--
ALTER TABLE `cheque`
  ADD PRIMARY KEY (`idcheque`);

--
-- Indices de la tabla `chofer`
--
ALTER TABLE `chofer`
  ADD PRIMARY KEY (`idChofer`);

--
-- Indices de la tabla `cliente`
--
ALTER TABLE `cliente`
  ADD PRIMARY KEY (`idCliente`);

--
-- Indices de la tabla `cuenta_corriente`
--
ALTER TABLE `cuenta_corriente`
  ADD PRIMARY KEY (`idcuenta_corriente`),
  ADD KEY `cc_cliente_fk_idx` (`idCliente`),
  ADD KEY `fk_cc_fletero_idx` (`idfletero`);

--
-- Indices de la tabla `fletero`
--
ALTER TABLE `fletero`
  ADD PRIMARY KEY (`idFletero`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`idpago`),
  ADD KEY `fk_pago_chofer_idx` (`idChofer`),
  ADD KEY `fk_pago_viaje_idx` (`idViaje`),
  ADD KEY `fk_pago_sueldo_idx` (`idSueldo`);

--
-- Indices de la tabla `sueldo`
--
ALTER TABLE `sueldo`
  ADD PRIMARY KEY (`idsueldo`),
  ADD KEY `fk_sueldo_chofer_idx` (`idchofer`),
  ADD KEY `fk_sueldo_camion_idx` (`idCamion`);

--
-- Indices de la tabla `viaje`
--
ALTER TABLE `viaje`
  ADD PRIMARY KEY (`idviaje`),
  ADD KEY `viaje_camion_fk_idx` (`idcamion`),
  ADD KEY `viaje_cliente_fk_idx` (`idcliente`);

--
-- Indices de la tabla `viaje_flete`
--
ALTER TABLE `viaje_flete`
  ADD PRIMARY KEY (`idviaje_flete`),
  ADD KEY `fk_flete_cliente_idx` (`idCliente`),
  ADD KEY `fk_flete_fletero_idx` (`fletero`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `camion`
--
ALTER TABLE `camion`
  MODIFY `idcamion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT de la tabla `cheque`
--
ALTER TABLE `cheque`
  MODIFY `idcheque` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `chofer`
--
ALTER TABLE `chofer`
  MODIFY `idChofer` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- AUTO_INCREMENT de la tabla `cliente`
--
ALTER TABLE `cliente`
  MODIFY `idCliente` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT de la tabla `cuenta_corriente`
--
ALTER TABLE `cuenta_corriente`
  MODIFY `idcuenta_corriente` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `fletero`
--
ALTER TABLE `fletero`
  MODIFY `idFletero` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `idpago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `sueldo`
--
ALTER TABLE `sueldo`
  MODIFY `idsueldo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `viaje`
--
ALTER TABLE `viaje`
  MODIFY `idviaje` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `viaje_flete`
--
ALTER TABLE `viaje_flete`
  MODIFY `idviaje_flete` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `cuenta_corriente`
--
ALTER TABLE `cuenta_corriente`
  ADD CONSTRAINT `cc_cliente_fk` FOREIGN KEY (`idCliente`) REFERENCES `cliente` (`idCliente`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_cc_fletero` FOREIGN KEY (`idfletero`) REFERENCES `fletero` (`idFletero`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_pago_chofer` FOREIGN KEY (`idChofer`) REFERENCES `chofer` (`idChofer`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_pago_sueldo` FOREIGN KEY (`idSueldo`) REFERENCES `sueldo` (`idsueldo`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_pago_viaje` FOREIGN KEY (`idViaje`) REFERENCES `viaje` (`idviaje`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `sueldo`
--
ALTER TABLE `sueldo`
  ADD CONSTRAINT `fk_sueldo_camion` FOREIGN KEY (`idCamion`) REFERENCES `camion` (`idcamion`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_sueldo_chofer` FOREIGN KEY (`idchofer`) REFERENCES `chofer` (`idChofer`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `viaje`
--
ALTER TABLE `viaje`
  ADD CONSTRAINT `viaje_camion_fk` FOREIGN KEY (`idcamion`) REFERENCES `camion` (`idcamion`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `viaje_cliente_fk` FOREIGN KEY (`idcliente`) REFERENCES `cliente` (`idCliente`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `viaje_flete`
--
ALTER TABLE `viaje_flete`
  ADD CONSTRAINT `fk_flete_cliente` FOREIGN KEY (`idCliente`) REFERENCES `cliente` (`idCliente`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_flete_fletero` FOREIGN KEY (`fletero`) REFERENCES `fletero` (`idFletero`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
