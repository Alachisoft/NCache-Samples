-- MySQL dump 10.13  Distrib 5.1.53, for Win32 (ia32)
--
-- Host: localhost    Database: hibernatesample
-- ------------------------------------------------------
-- Server version	5.1.53-community

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `customer`
--

DROP TABLE IF EXISTS `customers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `customers` (
  `CustomerID` varchar(255) NOT NULL,
  `Address` varchar(255) DEFAULT NULL,
  `City` varchar(255) DEFAULT NULL,
  `CompanyName` varchar(255) DEFAULT NULL,
  `ContactName` varchar(255) DEFAULT NULL,
  `Country` varchar(255) DEFAULT NULL,
  `PostalCode` varchar(255) DEFAULT NULL,
  `Region` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`CustomerID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `customer`
--

LOCK TABLES `customers` WRITE;
/*!40000 ALTER TABLE `customer` DISABLE KEYS */;
INSERT INTO `customers` VALUES ('ALFKI','Obere Str. 57','Berlin','Alfreds Futterkiste','Maria Anders','Germany','12209',NULL),('ANATR','Avda. de la Constituci¢n 2222','M‚xico D.F.','Ana Trujillo Emparedados y helados','Ana Trujillo','Mexico','05021',NULL),('ANTON','Mataderos  2312','M‚xico D.F.','Antonio Moreno Taquer¡a','Antonio Moreno','Mexico','05023',NULL),('AROUT','120 Hanover Sq.','London','Around the Horn','Thomas Hardy','UK','WA1 1DP',NULL),('BERGS','Berguvsv„gen  8','Lule†','Berglunds snabbk”p','Christina Berglund','Sweden','S-958 22',NULL),('BLAUS','Forsterstr. 57','Mannheim','Blauer See Delikatessen','Hanna Moos','Germany','68306',NULL),('BLONP','24, place Kl‚ber','Strasbourg','Blondesddsl pŠre et fils','Fr‚d‚rique Citeaux','France','67000',NULL),('BOLID','C/ Araquil, 67','Madrid','B¢lido Comidas preparadas','Mart¡n Sommer','Spain','28023',NULL),('BONAP','C/ Araquil, 67','Marseille','Bon app','Laurence Lebihan','France','13008',NULL),('BOTTM','23 Tsawassen Blvd.','Tsawassen','Bottom-Dollar Markets','Elizabeth Lincoln','Canada','T2F 8M4',NULL);
/*!40000 ALTER TABLE `customer` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orderdetails`
--

DROP TABLE IF EXISTS `orderdetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `orderdetails` (
  `ProductID` int(11) NOT NULL,
  `OrderID` int(11) DEFAULT NULL,
  PRIMARY KEY (`ProductID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orderdetails`
--

LOCK TABLES `orderdetails` WRITE;
/*!40000 ALTER TABLE `orderdetails` DISABLE KEYS */;
/*!40000 ALTER TABLE `orderdetails` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `orders`
--

DROP TABLE IF EXISTS `orders`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `orders` (
  `OrderID` int(11) NOT NULL,
  `CustomerID` varchar(255) DEFAULT NULL,
  `ShipAddress` int(11) DEFAULT NULL,
  `ShipCity` int(11) DEFAULT NULL,
  `ShipPostalCode` int(11) DEFAULT NULL,
  `ShipRegion` int(11) DEFAULT NULL,
  `ShippedDate` datetime DEFAULT NULL,
  `OrderDate` datetime DEFAULT NULL,
  `ShippedName` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`OrderID`),
  KEY `FKC3DF62E56ACEE8AE` (`CustomerID`),
  CONSTRAINT `FKC3DF62E56ACEE8AE` FOREIGN KEY (`CustomerID`) REFERENCES `customer` (`CustomerID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `orders`
--

LOCK TABLES `orders` WRITE;
/*!40000 ALTER TABLE `orders` DISABLE KEYS */;
INSERT INTO `orders` VALUES (10248,'ALFKI',1551,22,46310,5,'1996-07-16 00:00:00','1996-07-04 00:00:00','Vins et alcools Chevalier'),(10249,'ANATR',4567,42,98100,11,'1996-08-01 00:00:00','1996-07-03 00:00:00','Toms Spezialit„ten'),(10250,'ANTON',3238,13,4260,9,'1996-07-12 00:00:00','1996-07-08 00:00:00','Hanari Carnes'),(10251,'AROUT',5509,42,46000,21,'1996-07-29 00:00:00','1996-07-19 00:00:00','Ottilies K„seladen'),(10261,'ALFKI',5509,22,46310,5,'1996-07-19 00:00:00','1996-07-30 00:00:00','Que Del¡cia'),(10279,'ALFKI',5156,22,46310,5,'1996-08-13 00:00:00','1996-08-16 00:00:00','Lehmanns Marktstand');
/*!40000 ALTER TABLE `orders` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `product`
--

DROP TABLE IF EXISTS `product`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `product` (
  `ProductID` int(11) NOT NULL,
  `ProductName` varchar(255) DEFAULT NULL,
  `OrderID` int(11) DEFAULT NULL,
  `UnitPrice` double DEFAULT NULL,
  PRIMARY KEY (`ProductID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `product`
--

LOCK TABLES `product` WRITE;
/*!40000 ALTER TABLE `product` DISABLE KEYS */;
/*!40000 ALTER TABLE `product` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2011-06-06 16:12:03
