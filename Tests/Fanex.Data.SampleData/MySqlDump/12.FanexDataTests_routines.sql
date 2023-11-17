
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
-- Dumping routines for database 'FanexDataTests'
--
DROP PROCEDURE IF EXISTS `CustomerByID`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `CustomerByID`(
    IN CustomerID CHAR(5))
BEGIN
    SELECT
       `Cust`.`CustomerID`,
       `Cust`.`CompanyName`,
       `Cust`.`ContactName`,
       `Cust`.`ContactTitle`,
       `Cust`.`Address`,
       `Cust`.`City`,
       `Cust`.`Region`,
       `Cust`.`PostalCode`,
       `Cust`.`Country`,
       `Cust`.`Phone`,
       `Cust`.`Fax`
    FROM `Customers` AS `Cust`
    WHERE `Cust`.`CustomerID` = CustomerID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `DeleteEmployeeTerritory`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `DeleteEmployeeTerritory`(
    IN EmployeeID  INT,
    IN TerritoryID VARCHAR(20))
BEGIN
    DELETE FROM `EmployeeTerritories`
    WHERE (`EmployeeTerritories`.`EmployeeID` = EmployeeID) AND (`EmployeeTerritories`.`TerritoryID` = TerritoryID);
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `EmployeesByBirthYear`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `EmployeesByBirthYear`(
    IN BirthYear INT,
    IN InTime    TIME,
    OUT OutTime   TIME)
BEGIN
    SET OutTime = InTime;

    IF (BirthYear > 0) THEN
        SELECT
           `EmployeeID`,
           `FirstName`,
           `LastName`,
           `Title`,
           `TitleOfCourtesy`,
           `BirthDate`,
           `HireDate`,
           `Address`,
           `City`,
           `Region`,
           `PostalCode`,
           `Country`,
           `HomePhone`,
           `Extension`,
           `PhotoPath`,
           `Notes`
        FROM `Employees`
        WHERE YEAR(`BirthDate`) = BirthYear;
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `FetchMultipleTables`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `FetchMultipleTables`()
BEGIN
    SELECT
        `CategoryID`,
        `CategoryName`,
        `Description`,
        `Picture`
    FROM `Categories`;

    SELECT
        `CustomerID`,
        `CompanyName`,
        `ContactName`,
        `ContactTitle`,
        `Address`,
        `City`,
        `Region`,
        `PostalCode`,
        `Country`,
        `Phone`,
        `Fax`
    FROM `Customers`
    LIMIT 10;

    SELECT
        `EmployeeID`,
        `LastName`,
        `FirstName`,
        `Title`,
        `TitleOfCourtesy`,
        `BirthDate`,
        `HireDate`,
        `Address`,
        `City`,
        `Region`,
        `PostalCode`,
        `Country`,
        `HomePhone`,
        `Extension`,
        `Photo`,
        `Notes`,
        `ReportsTo`,
        `PhotoPath`
    FROM `Employees`
    LIMIT 10;

    SELECT
        `ProductID`,
        `ProductName`,
        `QuantityPerUnit`,
        `UnitPrice`,
        `UnitsInStock`,
        `UnitsOnOrder`,
        `ReorderLevel`,
        `Discontinued`
    FROM `Products`
    LIMIT 10;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `GetEmployeeTerritory`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `GetEmployeeTerritory`(
    IN EmployeeID  INT,
    IN TerritoryID VARCHAR(20))
BEGIN
    SELECT
        `ET`.`EmployeeID`,
        `ET`.`TerritoryID`
    FROM `EmployeeTerritories` AS `ET`
    WHERE `ET`.`EmployeeID` = EmployeeID AND `ET`.`TerritoryID` = TerritoryID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `GetProductByCategoryID`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `GetProductByCategoryID`(
    IN CategoryID INT,
    OUT CategoryName VARCHAR(15))
BEGIN
    SELECT `Cate`.`CategoryName` INTO CategoryName
    FROM `Categories` AS `Cate`
    WHERE `Cate`.`CategoryID` = CategoryID;

    IF (CategoryID > 0) THEN
        SELECT
           `Cate`.`CategoryID`,
           `Cate`.`CategoryName`,
           `Cate`.`Description`
        FROM `Categories` AS `Cate`
        WHERE `Cate`.`CategoryID` = CategoryID;

        SELECT
           `Prod`.`ProductID`,
           `Prod`.`ProductName`,
           `Prod`.`QuantityPerUnit`,
           `Prod`.`UnitPrice`,
           `Prod`.`UnitsInStock`,
           `Prod`.`UnitsOnOrder`,
           `Prod`.`ReorderLevel`,
           `Prod`.`Discontinued`
        FROM `Products` AS `Prod`
            LEFT JOIN `Categories` AS `Cate` ON `Prod`.`CategoryID` = `Cate`.CategoryID
        WHERE `Cate`.`CategoryID` = CategoryID;
    END IF;    
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `InsertEmployeeTerritory`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `InsertEmployeeTerritory`(
    IN EmployeeID  INT,
    IN TerritoryID VARCHAR(20))
BEGIN
    INSERT `EmployeeTerritories`
    (
        `EmployeeID`,
        `TerritoryID`
    )
    VALUES
    (
        EmployeeID,
        TerritoryID
    );
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `OrderByID`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `OrderByID`(
    IN OrderID INT)
BEGIN
    SELECT
        `Ord`.`OrderID`,
        `Ord`.`OrderDate`,
        `Ord`.`RequiredDate`,
        `Ord`.`ShippedDate`,
        `Ord`.`Freight`,
        `Ord`.`ShipName`,
        `Ord`.`ShipAddress`,
        `Ord`.`ShipCity`,
        `Ord`.`ShipRegion`,
        `Ord`.`ShipPostalCode`,
        `Ord`.`ShipCountry`,
        `Cust`.`CustomerID`,
        `Cust`.`CompanyName`,
        `Cust`.`ContactName`,
        `Cust`.`ContactTitle`,
        `Cust`.`Address` AS `CustomerAddress`,
        `Cust`.`City` AS `CustomerCity`,
        `Cust`.`Region` AS `CustomerRegion`,
        `Cust`.`PostalCode` AS `CustomerPostalCode`,
        `Cust`.`Country` AS `CustomerCountry`,
        `Cust`.`Phone` AS `CustomerPhone`,
        `Cust`.`Fax` AS `CustomerFax`,
        `Empl`.`EmployeeID`,
        `Empl`.`FirstName`,
        `Empl`.`LastName`,
        `Empl`.`Title`,
        `Empl`.`TitleOfCourtesy`,
        `Empl`.`BirthDate`,
        `Empl`.`HireDate`,
        `Empl`.`Address` AS `EmployeeAddress`,
        `Empl`.`City` AS `EmployeeCity`,
        `Empl`.`Region` AS `EmployeeRegion`,
        `Empl`.`PostalCode` AS `EmployeePostalCode`,
        `Empl`.`Country` AS `EmployeeCountry`,
        `Empl`.`HomePhone` AS `EmployeePhone`,
        `Empl`.`Extension`,
        `Empl`.`PhotoPath`,
        `Empl`.`Notes`,
        `Shp`.`ShipperID`,
        `Shp`.`CompanyName` AS ShipperCompany,
        `Shp`.`Phone` AS ShipperPhone
    FROM `Orders` AS `Ord`
        LEFT JOIN `Customers` AS `Cust` ON `Cust`.`CustomerID` = `Ord`.`CustomerID`
        LEFT JOIN `Employees` AS `Empl` ON `Empl`.`EmployeeID` = `Ord`.`EmployeeID`
        LEFT JOIN `Shippers` AS `Shp` ON `Shp`.`ShipperID` = `Ord`.`ShipVia`
    WHERE `Ord`.`OrderID` = OrderID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `OrderCountByProduct`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `OrderCountByProduct`(
    IN  ProductID   INT,
    OUT ProductName VARCHAR(40))
BEGIN
    SELECT `Prod`.`ProductName` INTO ProductName
    FROM `Products` AS `Prod`
    WHERE `Prod`.`ProductID` = ProductID;

    SELECT COUNT(`OrdDtl`.`OrderID`) AS OrderCount
    FROM `Order Details` AS `OrdDtl`
    WHERE `OrdDtl`.`ProductID` = ProductID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `OrderDetailsByOrderID`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `OrderDetailsByOrderID`(
    IN OrderID INT)
BEGIN
    SELECT
        `Ord`.`OrderID`,
        `Ord`.`OrderDate`,
        `Ord`.`RequiredDate`,
        `Ord`.`ShippedDate`,
        `Ord`.`Freight`,
        `Ord`.`ShipName`,
        `Ord`.`ShipAddress`,
        `Ord`.`ShipCity`,
        `Ord`.`ShipRegion`,
        `Ord`.`ShipPostalCode`,
        `Ord`.`ShipCountry`,
        `Cust`.`CustomerID`,
        `Cust`.`CompanyName`,
        `Cust`.`ContactName`,
        `Cust`.`ContactTitle`,
        `Cust`.`Address` AS `CustomerAddress`,
        `Cust`.`City` AS `CustomerCity`,
        `Cust`.`Region` AS `CustomerRegion`,
        `Cust`.`PostalCode` AS `CustomerPostalCode`,
        `Cust`.`Country` AS `CustomerCountry`,
        `Cust`.`Phone` AS `CustomerPhone`,
        `Cust`.`Fax` AS `CustomerFax`,
        `Empl`.`EmployeeID`,
        `Empl`.`FirstName`,
        `Empl`.`LastName`,
        `Empl`.`Title`,
        `Empl`.`TitleOfCourtesy`,
        `Empl`.`BirthDate`,
        `Empl`.`HireDate`,
        `Empl`.`Address` AS `EmployeeAddress`,
        `Empl`.`City` AS `EmployeeCity`,
        `Empl`.`Region` AS `EmployeeRegion`,
        `Empl`.`PostalCode` AS `EmployeePostalCode`,
        `Empl`.`Country` AS `EmployeeCountry`,
        `Empl`.`HomePhone` AS `EmployeePhone`,
        `Empl`.`Extension`,
        `Empl`.`PhotoPath`,
        `Empl`.`Notes`,
        `Shp`.`ShipperID`,
        `Shp`.`CompanyName` AS `ShipperCompany`,
        `Shp`.`Phone` AS `ShipperPhone`
    FROM `Orders` AS `Ord`
        LEFT JOIN `Customers` AS `Cust` ON `Cust`.`CustomerID` = `Ord`.`CustomerID`
        LEFT JOIN `Employees` AS `Empl` ON `Empl`.`EmployeeID` = `Ord`.`EmployeeID`
        LEFT JOIN `Shippers` AS `Shp` ON `Shp`.`ShipperID` = `Ord`.`ShipVia`
    WHERE `Ord`.`OrderID` = OrderID;
    
    SELECT
        `OrdDtl`.`UnitPrice`,
        `OrdDtl`.`Quantity`,
        `OrdDtl`.`Discount`,
        `Prod`.`ProductID`,
        `Prod`.`ProductName`,
        `Prod`.`QuantityPerUnit`,
        `Prod`.`UnitPrice` AS `OriginalUnitPrice`,
        `Prod`.`UnitsInStock`,
        `Prod`.`UnitsOnOrder`,
        `Prod`.`ReorderLevel`,
        `Prod`.`Discontinued`,
        `Sup`.`SupplierID`,
        `Sup`.`CompanyName`,
        `Sup`.`ContactName`,
        `Sup`.`ContactTitle`,
        `Sup`.`Address`,
        `Sup`.`City`,
        `Sup`.`Region`,
        `Sup`.`PostalCode`,
        `Sup`.`Country`,
        `Sup`.`Phone`,
        `Sup`.`Fax`,
        `Sup`.`HomePage`,
        `Cat`.`CategoryID`,
        `Cat`.`CategoryName`,
        `Cat`.`Description`
    FROM `Order Details` AS `OrdDtl`
        LEFT JOIN `Products` AS `Prod` ON `OrdDtl`.`ProductID` = `Prod`.`ProductID`
        LEFT JOIN `Suppliers` AS `Sup` ON `Prod`.`SupplierID` = `Sup`.`SupplierID`
        LEFT JOIN `Categories` AS `Cat` ON `Prod`.`CategoryID` = `Cat`.`CategoryID`
    WHERE `OrdDtl`.`OrderID` = OrderID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `OrdersByCustomerId`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `OrdersByCustomerId`(
    IN  CustomerID   CHAR(5),
    OUT NumberOfRows INT)
BEGIN
    SELECT
        `Ord`.`OrderID`,
        `Ord`.`OrderDate`,
        `Ord`.`RequiredDate`,
        `Ord`.`ShippedDate`,
        `Ord`.`Freight`,
        `Ord`.`ShipName`,
        `Ord`.`ShipAddress`,
        `Ord`.`ShipCity`,
        `Ord`.`ShipRegion`,
        `Ord`.`ShipPostalCode`,
        `Ord`.`ShipCountry`,
        `Cust`.`CustomerID`,
        `Cust`.`CompanyName`,
        `Cust`.`ContactName`,
        `Cust`.`ContactTitle`,
        `Cust`.`Address` AS `CustomerAddress`,
        `Cust`.`City` AS `CustomerCity`,
        `Cust`.`Region` AS `CustomerRegion`,
        `Cust`.`PostalCode` AS `CustomerPostalCode`,
        `Cust`.`Country` AS `CustomerCountry`,
        `Cust`.`Phone` AS `CustomerPhone`,
        `Cust`.`Fax` AS `CustomerFax`,
        `Empl`.`EmployeeID`,
        `Empl`.`FirstName`,
        `Empl`.`LastName`,
        `Empl`.`Title`,
        `Empl`.`TitleOfCourtesy`,
        `Empl`.`BirthDate`,
        `Empl`.`HireDate`,
        `Empl`.`Address` AS `EmployeeAddress`,
        `Empl`.`City` AS `EmployeeCity`,
        `Empl`.`Region` AS `EmployeeRegion`,
        `Empl`.`PostalCode` AS `EmployeePostalCode`,
        `Empl`.`Country` AS `EmployeeCountry`,
        `Empl`.`HomePhone` AS `EmployeePhone`,
        `Empl`.`Extension`,
        `Empl`.`PhotoPath`,
        `Empl`.`Notes`,
        `Shp`.`ShipperID`,
        `Shp`.`CompanyName` AS `ShipperCompany`,
        `Shp`.`Phone` AS `ShipperPhone`
    FROM `Orders` AS `Ord`
        LEFT JOIN `Customers` AS `Cust` ON `Cust`.`CustomerID` = `Ord`.`CustomerID`
        LEFT JOIN `Employees` AS `Empl` ON `Empl`.`EmployeeID` = `Ord`.`EmployeeID`
        LEFT JOIN `Shippers` AS `Shp` ON `Shp`.`ShipperID` = `Ord`.`ShipVia`
    WHERE `Ord`.`CustomerID` = CustomerID
    ORDER BY `Ord`.`OrderID`;
    
    SELECT COUNT(`Ord`.`OrderID`) INTO NumberOfRows
    FROM `Orders` AS `Ord`
    WHERE `Ord`.`CustomerID` = CustomerID;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `QuickOrderByID`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `QuickOrderByID`(
    IN OrderID INT)
BEGIN
    SELECT
        `Ord`.`OrderID`,
        `Ord`.`OrderDate`,
        `Ord`.`RequiredDate`,
        `Ord`.`ShippedDate`,
        `Ord`.`Freight`,
        `Ord`.`ShipName`,
        `Ord`.`ShipAddress`,
        `Ord`.`ShipCity`,
        `Ord`.`ShipRegion`,
        `Ord`.`ShipPostalCode`,
        `Ord`.`ShipCountry`,
        `Cust`.`CustomerID`,
        `Cust`.`CompanyName`,
        `Cust`.`ContactName`,
        `Cust`.`ContactTitle`,
        `Cust`.`Address` AS `CustomerAddress`,
        `Cust`.`City` AS `CustomerCity`,
        `Cust`.`Region` AS `CustomerRegion`,
        `Cust`.`PostalCode` AS `CustomerPostalCode`,
        `Cust`.`Country` AS `CustomerCountry`,
        `Cust`.`Phone` AS `CustomerPhone`,
        `Cust`.`Fax` AS `CustomerFax`,
        `Empl`.`EmployeeID`,
        `Empl`.`FirstName`,
        `Empl`.`LastName`,
        `Empl`.`Title`,
        `Empl`.`TitleOfCourtesy`,
        `Empl`.`BirthDate`,
        `Empl`.`HireDate`,
        `Empl`.`Address` AS `EmployeeAddress`,
        `Empl`.`City` AS `EmployeeCity`,
        `Empl`.`Region` AS `EmployeeRegion`,
        `Empl`.`PostalCode` AS `EmployeePostalCode`,
        `Empl`.`Country` AS `EmployeeCountry`,
        `Empl`.`HomePhone` AS `EmployeePhone`,
        `Empl`.`Extension`,
        `Empl`.`PhotoPath`,
        `Empl`.`Notes`,
        `Shp`.`ShipperID`,
        `Shp`.`CompanyName` AS `ShipperCompany`,
        `Shp`.`Phone` AS `ShipperPhone`,
        `OrdDtl`.`UnitPrice`,
        `OrdDtl`.`Quantity`,
        `OrdDtl`.`Discount`,
        `Prod`.`ProductID`,
        `Prod`.`ProductName`,
        `Prod`.`QuantityPerUnit`,
        `Prod`.`UnitPrice` AS `OriginalUnitPrice`,
        `Prod`.`UnitsInStock`,
        `Prod`.`UnitsOnOrder`,
        `Prod`.`ReorderLevel`,
        `Prod`.`Discontinued`,
        `Sup`.`SupplierID`,
        `Sup`.`CompanyName` AS `SupplierCompany`,
        `Sup`.`ContactName` AS `SupplierContactName`,
        `Sup`.`ContactTitle` AS `SupplierContactTitle`,
        `Sup`.`Address` AS `SupplierAddress`,
        `Sup`.`City` AS `SupplierCity`,
        `Sup`.`Region` AS `SupplierRegion`,
        `Sup`.`PostalCode` AS `SupplierPostalCode`,
        `Sup`.`Country` AS `SupplierCountry`,
        `Sup`.`Phone` AS `SupplierPhone`,
        `Sup`.`Fax` AS `SupplierFax`,
        `Sup`.`HomePage` AS `SupplierHomePage`,
        `Cat`.`CategoryID`,
        `Cat`.`CategoryName`,
        `Cat`.`Description`
    FROM `Orders` AS `Ord`
        LEFT JOIN `Customers` AS `Cust` ON `Cust`.`CustomerID` = `Ord`.`CustomerID`
        LEFT JOIN `Employees` AS `Empl` ON `Empl`.`EmployeeID` = `Ord`.`EmployeeID`
        LEFT JOIN `Shippers` AS `Shp` ON `Shp`.`ShipperID` = `Ord`.`ShipVia`
        LEFT JOIN `Order Details` AS `OrdDtl` ON `OrdDtl`.`OrderID` = `Ord`.`OrderID`
        LEFT JOIN `Products` AS `Prod` ON `OrdDtl`.`ProductID` = `Prod`.`ProductID`
        LEFT JOIN `Suppliers` AS `Sup` ON `Prod`.`SupplierID` = `Sup`.`SupplierID`
        LEFT JOIN `Categories` AS `Cat` ON `Prod`.`CategoryID` = `Cat`.`CategoryID`
    WHERE `Ord`.`OrderID` = OrderID
    LIMIT 1;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
DROP PROCEDURE IF EXISTS `TenMostExpensiveProducts`;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_unicode_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`fanexuser`@`%` PROCEDURE `TenMostExpensiveProducts`()
BEGIN
    SELECT
        `ProductName` AS `TenMostExpensiveProducts`,
        `UnitPrice`
    FROM `Products`
    ORDER BY `UnitPrice` DESC
    LIMIT 10;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-04-22 10:02:24
