-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: truck_manager_project_db
-- ------------------------------------------------------
-- Server version	5.5.5-10.4.32-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `camion`
--

DROP TABLE IF EXISTS `camion`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `camion` (
  `idcamion` int(11) NOT NULL AUTO_INCREMENT,
  `patente` varchar(45) NOT NULL,
  `nombre_chofer` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idcamion`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `camion`
--

LOCK TABLES `camion` WRITE;
/*!40000 ALTER TABLE `camion` DISABLE KEYS */;
INSERT INTO `camion` VALUES (2,'HIJ429','x'),(5,'WWW123','carlos'),(12,'MLA126','Pepito'),(14,'NCS234','Mili'),(17,'KJH921','Vicky'),(21,'pruebaagregar1','nuevoPrueba1'),(22,'pruebaagregar2','nuevoPrueba2'),(25,'JJJ999','inventado'),(26,'HHH888','inventado2'),(27,'AAA333','Peter');
/*!40000 ALTER TABLE `camion` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cheque`
--

DROP TABLE IF EXISTS `cheque`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cheque` (
  `idcheque` int(11) NOT NULL AUTO_INCREMENT,
  `fecha_ingreso` date NOT NULL,
  `nro_cheque` int(11) NOT NULL,
  `monto` float NOT NULL,
  `banco` varchar(45) NOT NULL,
  `fecha_cobro` date NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `numero_personalizado` int(11) DEFAULT NULL,
  `fecha_vencimieto` date DEFAULT NULL,
  `entregado_a` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idcheque`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cheque`
--

LOCK TABLES `cheque` WRITE;
/*!40000 ALTER TABLE `cheque` DISABLE KEYS */;
INSERT INTO `cheque` VALUES (1,'2025-05-19',54321,2000,'Banco Control','2025-06-18','',NULL,'2025-06-18',NULL),(2,'2025-05-19',12345,6000,'Banco Nación','2025-06-18','',NULL,'2025-06-18',NULL),(3,'2021-12-12',1234,150000,'Banco Nacion','2021-12-12','Marcos',1234,'2022-12-12','cristian');
/*!40000 ALTER TABLE `cheque` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `chofer`
--

DROP TABLE IF EXISTS `chofer`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `chofer` (
  `idChofer` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  PRIMARY KEY (`idChofer`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chofer`
--

LOCK TABLES `chofer` WRITE;
/*!40000 ALTER TABLE `chofer` DISABLE KEYS */;
INSERT INTO `chofer` VALUES (1,'Mili'),(4,'nuevo chofer'),(6,'nuevoPrueba1'),(10,'nuevoPrueba2'),(12,'Vicky'),(13,'inventado'),(14,'inventado2'),(15,'Peter'),(16,'carlos');
/*!40000 ALTER TABLE `chofer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cliente`
--

DROP TABLE IF EXISTS `cliente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cliente` (
  `idCliente` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(45) NOT NULL,
  PRIMARY KEY (`idCliente`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cliente`
--

LOCK TABLES `cliente` WRITE;
/*!40000 ALTER TABLE `cliente` DISABLE KEYS */;
INSERT INTO `cliente` VALUES (2,'Cliente1'),(3,'COOPERATIVA'),(4,'MACHACA'),(5,'TRANSPORTES DEL SUR'),(6,'CARGA PESADA SA'),(7,'DISTRIBUIDORA TANDIL'),(8,'PRUEBACLIENTE1'),(20,'CLIENTEPRUEBA'),(21,'PRUEBACORRECCION'),(22,'PRUEBACORRECCION2'),(23,'BHJ 099');
/*!40000 ALTER TABLE `cliente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuenta_corriente`
--

DROP TABLE IF EXISTS `cuenta_corriente`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuenta_corriente` (
  `idcuenta_corriente` int(11) NOT NULL AUTO_INCREMENT,
  `fecha_factura` date NOT NULL,
  `nro_factura` int(11) NOT NULL,
  `adeuda` float NOT NULL,
  `importe_pagado` float NOT NULL,
  `idCliente` int(11) DEFAULT NULL,
  `saldo` float NOT NULL,
  `idfletero` int(11) DEFAULT NULL,
  PRIMARY KEY (`idcuenta_corriente`),
  KEY `cc_cliente_fk_idx` (`idCliente`),
  KEY `fk_cc_fletero_idx` (`idfletero`),
  CONSTRAINT `cc_cliente_fk` FOREIGN KEY (`idCliente`) REFERENCES `cliente` (`idCliente`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_cc_fletero` FOREIGN KEY (`idfletero`) REFERENCES `fletero` (`idFletero`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuenta_corriente`
--

LOCK TABLES `cuenta_corriente` WRITE;
/*!40000 ALTER TABLE `cuenta_corriente` DISABLE KEYS */;
INSERT INTO `cuenta_corriente` VALUES (9,'2025-05-11',92,1000,10000,NULL,-9000,6);
/*!40000 ALTER TABLE `cuenta_corriente` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `fletero`
--

DROP TABLE IF EXISTS `fletero`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `fletero` (
  `idFletero` int(11) NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  PRIMARY KEY (`idFletero`)
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `fletero`
--

LOCK TABLES `fletero` WRITE;
/*!40000 ALTER TABLE `fletero` DISABLE KEYS */;
INSERT INTO `fletero` VALUES (1,'Bernabé'),(2,'Marcelo'),(4,'JUAN'),(6,'CARLOS'),(7,'ANITA'),(9,'FLETEROPRUEBA'),(12,'FLETERONUEVOO'),(13,'MARCELO');
/*!40000 ALTER TABLE `fletero` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pago`
--

DROP TABLE IF EXISTS `pago`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pago` (
  `idpago` int(11) NOT NULL AUTO_INCREMENT,
  `monto` float NOT NULL,
  `idChofer` int(11) DEFAULT NULL,
  `pagado` tinyint(4) DEFAULT NULL,
  `idViaje` int(11) NOT NULL,
  `idSueldo` int(11) DEFAULT NULL,
  PRIMARY KEY (`idpago`),
  KEY `fk_pago_chofer_idx` (`idChofer`),
  KEY `fk_pago_viaje_idx` (`idViaje`),
  KEY `fk_pago_sueldo_idx` (`idSueldo`),
  CONSTRAINT `fk_pago_chofer` FOREIGN KEY (`idChofer`) REFERENCES `chofer` (`idChofer`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_pago_sueldo` FOREIGN KEY (`idSueldo`) REFERENCES `sueldo` (`idsueldo`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_pago_viaje` FOREIGN KEY (`idViaje`) REFERENCES `viaje` (`idviaje`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pago`
--

LOCK TABLES `pago` WRITE;
/*!40000 ALTER TABLE `pago` DISABLE KEYS */;
/*!40000 ALTER TABLE `pago` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `sueldo`
--

DROP TABLE IF EXISTS `sueldo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `sueldo` (
  `idsueldo` int(11) NOT NULL AUTO_INCREMENT,
  `idchofer` int(11) DEFAULT NULL,
  `fecha_desde` date DEFAULT NULL,
  `fecha_hasta` date DEFAULT NULL,
  `fecha_pago` date DEFAULT NULL,
  `monto_total` float NOT NULL,
  `idCamion` int(11) DEFAULT NULL,
  `pagado` tinyint(1) NOT NULL,
  PRIMARY KEY (`idsueldo`),
  KEY `fk_sueldo_chofer_idx` (`idchofer`),
  KEY `fk_sueldo_camion_idx` (`idCamion`),
  CONSTRAINT `fk_sueldo_camion` FOREIGN KEY (`idCamion`) REFERENCES `camion` (`idcamion`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_sueldo_chofer` FOREIGN KEY (`idchofer`) REFERENCES `chofer` (`idChofer`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `sueldo`
--

LOCK TABLES `sueldo` WRITE;
/*!40000 ALTER TABLE `sueldo` DISABLE KEYS */;
INSERT INTO `sueldo` VALUES (5,1,'2025-05-01','2025-05-31','2025-05-29',22222,2,1),(6,4,'2025-04-01','2025-04-30',NULL,84848,14,0),(9,4,'2025-05-01','2025-05-31',NULL,45000,NULL,0),(10,NULL,'2025-05-01','2025-05-31','2025-05-29',100000,2,1);
/*!40000 ALTER TABLE `sueldo` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `viaje`
--

DROP TABLE IF EXISTS `viaje`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `viaje` (
  `idviaje` int(11) NOT NULL AUTO_INCREMENT,
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
  `comision_chofer` float NOT NULL,
  PRIMARY KEY (`idviaje`),
  KEY `viaje_camion_fk_idx` (`idcamion`),
  KEY `viaje_cliente_fk_idx` (`idcliente`),
  CONSTRAINT `viaje_camion_fk` FOREIGN KEY (`idcamion`) REFERENCES `camion` (`idcamion`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `viaje_cliente_fk` FOREIGN KEY (`idcliente`) REFERENCES `cliente` (`idCliente`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=14 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `viaje`
--

LOCK TABLES `viaje` WRITE;
/*!40000 ALTER TABLE `viaje` DISABLE KEYS */;
INSERT INTO `viaje` VALUES (1,'2025-04-05','Tandil','Buenos Aires',10001,5000,'Cereales',3,2,370,25,'x',0.18),(4,'2025-04-12','Córdoba','Mendoza',10004,8000,'Maquinaria',4,5,700,35,'carlos',0.18),(8,'2025-04-22','Rosario','Santa Fe',10008,6700,'Papel',7,12,160,15,'Pepito',0.18),(11,'2025-05-29','Tandil','Necochea',8888,500,'trigo',3,17,789,40000,'Vicky',20),(12,'2025-05-30','Tandil','Azul',9999,500,'maiz',4,17,789,40000,'Vicky',20);
/*!40000 ALTER TABLE `viaje` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `viaje_flete`
--

DROP TABLE IF EXISTS `viaje_flete`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `viaje_flete` (
  `idviaje_flete` int(11) NOT NULL AUTO_INCREMENT,
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
  `fecha_salida` date DEFAULT NULL,
  PRIMARY KEY (`idviaje_flete`),
  KEY `fk_flete_cliente_idx` (`idCliente`),
  KEY `fk_flete_fletero_idx` (`fletero`),
  CONSTRAINT `fk_flete_cliente` FOREIGN KEY (`idCliente`) REFERENCES `cliente` (`idCliente`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `fk_flete_fletero` FOREIGN KEY (`fletero`) REFERENCES `fletero` (`idFletero`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `viaje_flete`
--

LOCK TABLES `viaje_flete` WRITE;
/*!40000 ALTER TABLE `viaje_flete` DISABLE KEYS */;
INSERT INTO `viaje_flete` VALUES (1,'Tandil','Necochea',40,'trigo',120,130,19000,12345,5,1,'Chofer del Flete X',10,'2025-04-11'),(13,'Tandil','Necochea',40,'trigo',120,130,19000,12345,5,6,'Chofer de Carlos',10,'2025-04-11'),(15,'Tandil','Ayacucho',19292,'trigo',150,300,19000,12244,3,1,'Justo',10,'2025-04-15'),(16,'Pilar','Tandil',2000,'soja',650,880,35000,1235,3,2,'Patricio',15,'2025-04-10'),(17,'Tandil','Azul',40,'trigo',120,130,19000,9998,3,6,'Chofer de Carlos',10,'2025-04-11'),(18,'Tandil','MDP',1245,'trigo',200,500,20000,382882,4,2,'lauty',10,'2025-01-12');
/*!40000 ALTER TABLE `viaje_flete` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-06-13 15:40:17
