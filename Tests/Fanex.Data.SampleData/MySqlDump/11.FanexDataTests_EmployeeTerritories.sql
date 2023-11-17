
USE `fanexdatatests`;
-- MySQL dump 10.13  Distrib 8.0.15, for Win64 (x86_64)
--
-- Host: 10.18.200.110    Database: FanexDataTests
-- ------------------------------------------------------
-- Server version	8.0.15

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `EmployeeTerritories`
--

DROP TABLE IF EXISTS `EmployeeTerritories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `EmployeeTerritories` (
  `EmployeeID` int(11) NOT NULL,
  `TerritoryID` varchar(20) CHARACTER SET utf8 COLLATE utf8_general_ci NOT NULL,
  PRIMARY KEY (`EmployeeID`,`TerritoryID`),
  KEY `FK_EmployeeTerritories_Territories_idx` (`TerritoryID`),
  CONSTRAINT `FK_EmployeeTerritories_Employees` FOREIGN KEY (`EmployeeID`) REFERENCES `Employees` (`EmployeeID`),
  CONSTRAINT `FK_EmployeeTerritories_Territories` FOREIGN KEY (`TerritoryID`) REFERENCES `Territories` (`TerritoryID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `EmployeeTerritories`
--

LOCK TABLES `EmployeeTerritories` WRITE;
/*!40000 ALTER TABLE `EmployeeTerritories` DISABLE KEYS */;
INSERT INTO `EmployeeTerritories` VALUES (2,'01581'),(2,'01730'),(2,'01833'),(2,'02116'),(2,'02139'),(2,'02184'),(5,'02903'),(9,'03049'),(9,'03801'),(1,'06897'),(5,'07960'),(5,'08837'),(5,'10019'),(5,'10038'),(5,'11747'),(5,'14450'),(8,'19428'),(1,'19713'),(4,'20852'),(4,'27403'),(4,'27511'),(3,'30346'),(3,'31406'),(3,'32859'),(3,'33607'),(1,'40222'),(2,'40222'),(8,'44122'),(8,'45839'),(9,'48075'),(9,'48084'),(9,'48304'),(8,'53404'),(9,'55113'),(9,'55439'),(7,'60179'),(7,'60601'),(7,'80202'),(7,'80909'),(6,'85014'),(6,'85251'),(7,'90405'),(7,'94025'),(7,'94105'),(7,'95008'),(7,'95054'),(7,'95060'),(6,'98004'),(6,'98052'),(1,'98104'),(6,'98104');
/*!40000 ALTER TABLE `EmployeeTerritories` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-04-22 10:02:23
