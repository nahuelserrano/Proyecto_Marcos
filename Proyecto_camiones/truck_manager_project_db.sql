-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 21-05-2025 a las 17:14:22
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.0.30

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
(3, 'WWW123', 'juan'),
(4, 'WWW123', 'juan'),
(5, 'WWW123', 'carlos'),
(6, 'WWW123', 'mili'),
(7, 'WWW123', 'tobias'),
(10, 'HJK092', 'lauti'),
(11, 'PUC111', 'JUAN'),
(12, 'MLA126', 'Pepito'),
(13, 'MLA126', 'Pepito'),
(14, 'NCS234', 'Mili'),
(15, 'NCS234', 'Mili'),
(16, 'NCS234', 'Mili');

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
  `fecha_vencimieto` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `cheque`
--

INSERT INTO `cheque` (`idcheque`, `fecha_ingreso`, `nro_cheque`, `monto`, `banco`, `fecha_cobro`, `nombre`, `numero_personalizado`, `fecha_vencimieto`) VALUES
(1, '2025-05-19', 54321, 2000, 'Banco Control', '2025-06-18', '', NULL, '2025-06-18'),
(2, '2025-05-19', 12345, 6000, 'Banco Nación', '2025-06-18', '', NULL, '2025-06-18');

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
(2, 'Mili'),
(3, 'Mili'),
(4, 'nuevo chofer');

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
(7, 'DISTRIBUIDORA TANDIL');

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
(6, '2025-05-13', 10000, 35000, 1000, 2, 56000, NULL);

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
(3, 'Marcelo'),
(4, 'JUAN'),
(5, 'JUAN'),
(6, 'CARLOS');

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
(1, 0, 4, 0, 10, NULL);

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
(6, 4, '2025-04-01', '2025-04-30', NULL, 84848, 15, 0),
(7, 1, '2025-05-01', '2025-05-31', '2025-05-29', 23456, 2, 1),
(8, 4, '2025-04-01', '2025-04-30', NULL, 84848, 15, 0),
(9, 4, '2025-05-01', '2025-05-31', NULL, 45000, NULL, 0),
(10, NULL, '2025-05-01', '2025-05-31', '2025-05-29', 100000, 2, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

CREATE TABLE `usuario` (
  `idUsuario` int(11) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `contrasenia` varchar(45) NOT NULL,
  `username` varchar(45) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

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
(2, '2025-04-07', 'Mar del Plata', 'Bahía Blanca', 10002, 7500, 'Frutas', 3, 3, 420, 28, 'juan', 0.1),
(3, '2025-04-10', 'La Plata', 'Rosario', 10003, 6200, 'Muebles', 3, 4, 390, 26, 'juan', 0.18),
(4, '2025-04-12', 'Córdoba', 'Mendoza', 10004, 8000, 'Maquinaria', 4, 5, 700, 35, 'carlos', 0.18),
(5, '2025-04-15', 'Santa Fe', 'Tucumán', 10005, 5800, 'Alimentos', 5, 6, 780, 38, 'mili', 0.18),
(6, '2025-04-18', 'Salta', 'Jujuy', 10006, 4500, 'Textiles', 6, 10, 150, 18, 'lauti', 0.18),
(7, '2025-04-20', 'Buenos Aires', 'Córdoba', 10007, 9000, 'Electrónicos', 7, 11, 700, 36, 'roman', 0.18),
(8, '2025-04-22', 'Rosario', 'Santa Fe', 10008, 6700, 'Papel', 7, 12, 160, 15, 'Pepito', 0.18),
(9, '2025-04-28', 'Tandil', 'Azul', 123, 30, 'trigo', 2, 11, 350.5, 5000, 'JUAN', 0),
(10, '2025-04-28', 'Tandil', 'Azul', 123, 30, 'trigo', 2, 11, 350.5, 5000, 'nuevo chofer', 0);

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
(2, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(3, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(4, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(5, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(6, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(7, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(8, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(9, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(10, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(11, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(12, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 1, 'Chofer del Flete X', 10, '2025-04-11'),
(13, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 6, 'Chofer de Carlos', 10, '2025-04-11'),
(14, 'Tandil', 'Necochea', 40, 'trigo', 120, 130, 19000, 12345, 5, 6, 'Chofer de Carlos', 10, '2025-04-11'),
(15, 'Tandil', 'Ayacucho', 19292, 'trigo', 150, 300, 19000, 12244, 3, 1, 'Justo', 10, '2025-04-15'),
(16, 'Pilar', 'Tandil', 2000, 'soja', 650, 880, 35000, 1235, 3, 3, 'Patricio', 15, '2025-04-10');

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
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`idUsuario`);

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
  MODIFY `idcamion` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT de la tabla `cheque`
--
ALTER TABLE `cheque`
  MODIFY `idcheque` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `chofer`
--
ALTER TABLE `chofer`
  MODIFY `idChofer` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT de la tabla `cliente`
--
ALTER TABLE `cliente`
  MODIFY `idCliente` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `cuenta_corriente`
--
ALTER TABLE `cuenta_corriente`
  MODIFY `idcuenta_corriente` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT de la tabla `fletero`
--
ALTER TABLE `fletero`
  MODIFY `idFletero` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `idpago` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `sueldo`
--
ALTER TABLE `sueldo`
  MODIFY `idsueldo` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `idUsuario` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `viaje`
--
ALTER TABLE `viaje`
  MODIFY `idviaje` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `viaje_flete`
--
ALTER TABLE `viaje_flete`
  MODIFY `idviaje_flete` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `cuenta_corriente`
--
ALTER TABLE `cuenta_corriente`
  ADD CONSTRAINT `cc_cliente_fk` FOREIGN KEY (`idCliente`) REFERENCES `cliente` (`idCliente`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_cc_fletero` FOREIGN KEY (`idfletero`) REFERENCES `fletero` (`idFletero`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `fk_pago_chofer` FOREIGN KEY (`idChofer`) REFERENCES `chofer` (`idChofer`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_pago_sueldo` FOREIGN KEY (`idSueldo`) REFERENCES `sueldo` (`idsueldo`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_pago_viaje` FOREIGN KEY (`idViaje`) REFERENCES `viaje` (`idviaje`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `sueldo`
--
ALTER TABLE `sueldo`
  ADD CONSTRAINT `fk_sueldo_camion` FOREIGN KEY (`idCamion`) REFERENCES `camion` (`idcamion`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_sueldo_chofer` FOREIGN KEY (`idchofer`) REFERENCES `chofer` (`idChofer`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `viaje`
--
ALTER TABLE `viaje`
  ADD CONSTRAINT `viaje_camion_fk` FOREIGN KEY (`idcamion`) REFERENCES `camion` (`idcamion`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `viaje_cliente_fk` FOREIGN KEY (`idcliente`) REFERENCES `cliente` (`idCliente`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Filtros para la tabla `viaje_flete`
--
ALTER TABLE `viaje_flete`
  ADD CONSTRAINT `fk_flete_cliente` FOREIGN KEY (`idCliente`) REFERENCES `cliente` (`idCliente`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_flete_fletero` FOREIGN KEY (`fletero`) REFERENCES `fletero` (`idFletero`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
