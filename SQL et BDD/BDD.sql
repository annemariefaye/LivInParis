CREATE DATABASE  IF NOT EXISTS `livinparis` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `livinparis`;
-- MySQL dump 10.13  Distrib 8.0.41, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: livinparis
-- ------------------------------------------------------
-- Server version	8.0.41

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
-- Table structure for table `categorieambiance`
--

DROP TABLE IF EXISTS `categorieambiance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `categorieambiance` (
  `IdCategorie` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(100) NOT NULL,
  PRIMARY KEY (`IdCategorie`),
  UNIQUE KEY `Nom` (`Nom`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `categorieambiance`
--

LOCK TABLES `categorieambiance` WRITE;
/*!40000 ALTER TABLE `categorieambiance` DISABLE KEYS */;
INSERT INTO `categorieambiance` VALUES (5,'Chic'),(6,'Cosy'),(10,'Épicée'),(8,'Exotique'),(2,'Familiale'),(3,'Festive'),(1,'Romantique'),(4,'Soleil'),(9,'Spécial Été'),(7,'Traditionnelle');
/*!40000 ALTER TABLE `categorieambiance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `client`
--

DROP TABLE IF EXISTS `client`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `client` (
  `IdClient` int NOT NULL AUTO_INCREMENT,
  `NomEntreprise` varchar(255) DEFAULT NULL,
  `MotDePasse` varchar(255) NOT NULL,
  PRIMARY KEY (`IdClient`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `client`
--

LOCK TABLES `client` WRITE;
/*!40000 ALTER TABLE `client` DISABLE KEYS */;
INSERT INTO `client` VALUES (1,'Boulangerie Dupont','mdp123'),(2,'Entreprise XYZ','xyz789'),(3,'Perrot','&nRZm3de2R'),(4,'Mary','#6D5HzeU6P'),(5,'Lacroix S.A.S.','+R7ULaXQnl'),(6,'Marchal','&oUUYcHso0'),(7,'Fontaine','k4IWu$!@&!'),(8,'Jacob','^e3WMfk_!W'),(9,'Joseph','E%4f0IqtyK'),(10,'Imbert','@n66BccFW0'),(11,'Fischer Dupont SA','H@xhE4AqQb'),(12,'Barthelemy','L_0H9nmrv_'),(13,'Thibault Torres S.A.R.L.','4O)aEdDs$G'),(14,'Pons','$m4zQp9h+s'),(15,'Denis S.A.','fYaDXag&+7'),(16,'Pascal Imbert S.A.S.','+m1uP^NlWx'),(17,'Étienne Perret S.A.R.L.','(+B7TnOsf6'),(18,'Le Roux','o+8Q&sxc77'),(19,'Ruiz S.A.','!1RJB*n28p'),(20,'Ribeiro Baron S.A.','&9Z4CHOy!_'),(21,'Rolland SA','$F*8R^Ybp^'),(22,'Loiseau','$1uFXjGgb^'),(23,'Guillet Grenier SA','&!3AA$fSnE'),(24,'Diallo Le Gall S.A.S.','%n&#2ZyQrH'),(25,'Muller S.A.R.L.','^75kIbNn(F'),(26,'Perrier Prévost et Fils','$pZ*W7rpS2'),(27,'Allard','71^4wW+t)!'),(28,'Chauveau Guyon SARL','s*rL7RgQkR'),(29,'Delmas','^0ZfkmTWRM'),(30,'Étienne Robert SA','^)BS#2Va#U'),(31,'Morel Renault S.A.','92!3d@Mi!o'),(32,'Jourdan','%iQKP&gx82'),(33,'Charrier','#mx5)Du7YW'),(34,'Allain SARL','%z5$EgwriV'),(35,'Robert Gomes et Fils','@3X4HH6kei'),(36,'Vincent','c1bR4OUh4@'),(37,'Jourdan S.A.',')2AY4ohcBJ'),(38,'Guérin Aubry S.A.','+2DRZjJ80z'),(39,'Albert','!e_2FAdkQ3'),(40,'Gallet','@#q5YNvrJy'),(41,'Perret Guillou S.A.R.L.','gnggZOzV*0'),(42,'Moulin SA','sER3rXim%y'),(43,'Mathieu','y1p+3CyBcN'),(44,'Benard','r$6qT9Ul2('),(45,'Perrot Guillot SA','D_gw8psJES'),(46,'Meyer','&J$IAtNK$1'),(47,'Lecoq','mL5+HmDw%F'),(48,NULL,'boul2025'),(49,NULL,'gerard10');
/*!40000 ALTER TABLE `client` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `commande`
--

DROP TABLE IF EXISTS `commande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `commande` (
  `IdCommande` int NOT NULL AUTO_INCREMENT,
  `IdClient` int NOT NULL,
  `DateCommande` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `Statut` enum('En attente','Validée','Livrée','Annulée') NOT NULL,
  PRIMARY KEY (`IdCommande`),
  KEY `IdClient` (`IdClient`),
  CONSTRAINT `commande_ibfk_1` FOREIGN KEY (`IdClient`) REFERENCES `utilisateur` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `commande`
--

LOCK TABLES `commande` WRITE;
/*!40000 ALTER TABLE `commande` DISABLE KEYS */;
INSERT INTO `commande` VALUES (1,1,'2025-04-04 17:41:20','En attente'),(2,2,'2025-04-04 17:41:20','Validée'),(3,3,'2025-04-04 17:41:20','Validée'),(4,2,'2025-04-04 17:41:20','Validée'),(5,3,'2025-03-23 02:18:38','En attente'),(6,1,'2025-03-13 11:47:25','Livrée'),(7,3,'2025-03-22 15:20:20','Validée'),(8,4,'2025-03-28 23:37:15','En attente'),(9,9,'2025-03-13 17:50:44','Livrée'),(10,2,'2025-03-14 13:59:06','Validée'),(11,4,'2025-03-16 03:15:32','En attente'),(12,4,'2025-03-25 05:12:38','Livrée'),(13,2,'2025-04-03 07:55:16','En attente'),(14,1,'2025-03-10 03:32:11','Validée'),(15,2,'2025-03-15 03:57:55','Validée'),(16,2,'2025-04-01 13:40:57','En attente'),(17,9,'2025-03-16 01:54:51','En attente'),(18,9,'2025-03-14 03:02:51','Validée'),(19,3,'2025-03-23 14:42:04','Validée'),(20,1,'2025-03-16 17:22:11','En attente'),(21,2,'2025-03-27 07:41:45','Livrée'),(22,3,'2025-03-08 16:58:44','Validée'),(23,7,'2025-03-31 03:31:56','Livrée'),(24,9,'2025-03-13 07:03:52','En attente'),(25,1,'2025-04-03 18:14:32','Livrée'),(26,3,'2025-03-07 17:00:29','Livrée'),(27,3,'2025-03-20 21:01:16','Validée'),(28,4,'2025-03-12 10:15:29','Validée'),(29,2,'2025-03-11 21:06:07','Validée'),(30,7,'2025-03-14 11:05:32','Annulée'),(31,6,'2025-03-28 02:12:16','Validée'),(32,1,'2025-03-20 08:43:06','Livrée'),(33,1,'2025-03-27 03:06:40','Validée'),(34,8,'2025-03-10 02:56:20','Livrée'),(35,6,'2025-03-23 01:05:07','Validée'),(36,3,'2025-03-19 20:08:31','Livrée'),(37,4,'2025-04-01 00:34:14','En attente'),(38,7,'2025-03-20 22:02:12','En attente'),(39,5,'2025-03-17 02:29:33','Validée'),(40,5,'2025-03-09 11:25:16','En attente'),(41,9,'2025-03-27 22:41:06','Livrée'),(42,2,'2025-03-28 13:59:07','Annulée'),(43,5,'2025-03-30 03:25:58','En attente'),(44,4,'2025-03-09 17:09:06','Validée'),(45,3,'2025-03-15 13:06:45','Validée'),(46,1,'2025-03-24 23:51:39','Validée'),(47,7,'2025-03-18 22:57:24','Validée'),(48,4,'2025-03-25 22:28:49','En attente'),(49,4,'2025-03-18 03:10:55','Livrée'),(50,6,'2025-03-20 09:17:44','Validée'),(51,9,'2025-03-08 00:24:37','Livrée'),(52,10,'2025-03-13 15:28:30','Validée'),(53,2,'2025-03-09 19:54:33','Livrée'),(54,1,'2025-04-02 10:26:04','Livrée'),(55,1,'2025-04-04 17:41:20','Validée');
/*!40000 ALTER TABLE `commande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `correspondance`
--

DROP TABLE IF EXISTS `correspondance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `correspondance` (
  `IdStation` int NOT NULL,
  `IdLigne` int NOT NULL,
  PRIMARY KEY (`IdLigne`,`IdStation`),
  KEY `IdStation` (`IdStation`),
  CONSTRAINT `correspondance_ibfk_1` FOREIGN KEY (`IdStation`) REFERENCES `station` (`IdStation`) ON DELETE CASCADE,
  CONSTRAINT `correspondance_ibfk_2` FOREIGN KEY (`IdLigne`) REFERENCES `ligne` (`IdLigne`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `correspondance`
--

LOCK TABLES `correspondance` WRITE;
/*!40000 ALTER TABLE `correspondance` DISABLE KEYS */;
INSERT INTO `correspondance` VALUES (1,6),(2,2),(3,1),(3,4),(4,1),(4,7),(5,7),(5,14),(6,12),(6,14),(7,8),(7,12),(8,1),(8,8),(9,9),(9,13),(10,3),(10,9),(10,14),(11,2),(11,3),(12,2),(12,7),(13,4),(13,5);
/*!40000 ALTER TABLE `correspondance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cuisinier`
--

DROP TABLE IF EXISTS `cuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cuisinier` (
  `IdCuisinier` int NOT NULL AUTO_INCREMENT,
  `MotDePasse` varchar(255) NOT NULL,
  `PlatDuJour` varchar(255) NOT NULL,
  PRIMARY KEY (`IdCuisinier`)
) ENGINE=InnoDB AUTO_INCREMENT=53 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cuisinier`
--

LOCK TABLES `cuisinier` WRITE;
/*!40000 ALTER TABLE `cuisinier` DISABLE KEYS */;
INSERT INTO `cuisinier` VALUES (1,'cuisine123','Ratatouille'),(2,'de6U6fk%!E','Tajine d’agneau'),(3,'_MDP)owZ8y','Couscous'),(4,'LpB1@0Tp$a','Spaghetti bolognaise'),(5,'gNJ2XufT^C','Poulet basquaise'),(6,'YvbK42Hh+!','Gratin dauphinois'),(7,'1^6wuT1yD!','Pizza Margherita'),(8,'!)IS%js*8O','Pad thaï'),(9,'*KSAowl2m5','Falafel maison'),(10,'(mth5Lc$E7','Quiche lorraine'),(11,'jeZ9!Osx+2','Soupe pho'),(12,'78ZcZD9v&z','Biryani au poulet'),(13,'%gHB$upXn8','Bouillon asiatique'),(14,'T8KMpGSO&A','Lasagnes végétariennes'),(15,'&2KVfw)ySt','Wok de légumes'),(16,'fawZUVGz&1','Poulet curry coco'),(17,'6QCdMNCh)n','Gnocchis à la crème'),(18,'%h7VNfmeSn','Croque-monsieur'),(19,'H7w)xQUb(y','Riz cantonais'),(20,'+$m70_TdJq','Tartiflette'),(21,'NV_e3Fk^#(','Gratin de pâtes'),(22,'*SqBB+gmx3','Chili con carne'),(23,'0(f6CeA5ft','Paella'),(24,'(86SOIXa%!','Burger végétarien'),(25,'Dw7@7OPHx_','Clafoutis cerise'),(26,')SZyciB688','Crème brûlée'),(27,'%GLVvchF5c','Mousse au chocolat'),(28,'5Z6%7bQc$^','Pizza quatre fromages'),(29,'&9QnEsnpFK','Soupe à l’oignon'),(30,'NLI@kkz#$1','Flan pâtissier'),(31,')&1G*!Em57','Tiramisu'),(32,'l9Dix%Mq+J','Salade niçoise'),(33,'d#4%H2cNdm','Sushis saumon avocat'),(34,'(1B_XjojN2','Rouleaux de printemps'),(35,'))79EoqC1)','Gaspacho'),(36,'(fIMQDMu*4','Burger au bœuf'),(37,'+AXNC)zD50','Poulet rôti'),(38,'Bd$@1Yruh5','Tarte aux pommes'),(39,'0v1uI)y*@X','Nuggets de poulet'),(40,'*48cDLx9m!','Steak frites'),(41,'55)Jnyrm#L','Quiche végétarienne'),(42,'w!BHXwcy@0','Poisson pané maison'),(43,'*A@c!X1na6','Pizza végétarienne'),(44,'!A24Temms@','Gratin de courgettes'),(45,'p9hEsIq4_i','Burger classique'),(46,'$Q2IzwmfqP','Spaghetti carbonara'),(47,')1ExGoh!%2','Omelette aux fines herbes'),(48,'Oqvv5L*@%(','Wrap au poulet'),(49,'(_Ra1RnCy4','Tarte tatin'),(50,'(R2qTMgt#3','Raviolis ricotta-épinards'),(51,'^r7hnWxVe1','Soupe courge-coco'),(52,'chef789','Lasagnes');
/*!40000 ALTER TABLE `cuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ingredient`
--

DROP TABLE IF EXISTS `ingredient`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ingredient` (
  `IdIngredient` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(100) NOT NULL,
  `Prix` decimal(10,2) NOT NULL,
  PRIMARY KEY (`IdIngredient`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ingredient`
--

LOCK TABLES `ingredient` WRITE;
/*!40000 ALTER TABLE `ingredient` DISABLE KEYS */;
INSERT INTO `ingredient` VALUES (1,'Tomates',5.78),(2,'Courgettes',3.04),(3,'Pommes de terre',5.78),(4,'Oignons',4.69),(5,'Carottes',5.93),(6,'Poivrons rouges',5.46),(7,'Poivrons verts',4.96),(8,'Aubergines',2.24),(9,'Poulet',5.42),(10,'Agneau',4.92),(11,'Semoule',4.08),(12,'Riz basmati',1.04),(13,'Lait de coco',3.02),(14,'Pâtes',2.73),(15,'Boeuf haché',4.23),(16,'Thon',2.20),(17,'Jambon',0.78),(18,'Fromage râpé',4.36),(19,'Mozzarella',3.99),(20,'Parmesan',4.70),(21,'Lardons',4.31),(22,'Crème fraîche',4.09),(23,'Beurre',0.75),(24,'Œufs',2.10),(25,'Farine',2.52),(26,'Sucre',5.00),(27,'Chocolat noir',5.96),(28,'Levure',1.52),(29,'Vanille',4.08),(30,'Citron',3.57),(31,'Fraises',3.70),(32,'Pommes',1.88),(33,'Cerises',4.00),(34,'Bananes',3.62),(35,'Lait',2.30),(36,'Chapelure',1.90),(37,'Pain de mie',1.61),(38,'Salade verte',2.20),(39,'Épinards',5.86),(40,'Pois chiches',4.46),(41,'Lentilles corail',2.30),(42,'Tofu',2.90),(43,'Crevettes',4.00),(44,'Saumon',1.15),(45,'Basilic',2.98),(46,'Coriandre',2.95),(47,'Persil',4.43),(48,'Ail',5.14),(49,'Gingembre',4.35),(50,'Huile d’olive',2.72);
/*!40000 ALTER TABLE `ingredient` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `ligne`
--

DROP TABLE IF EXISTS `ligne`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ligne` (
  `IdLigne` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`IdLigne`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `ligne`
--

LOCK TABLES `ligne` WRITE;
/*!40000 ALTER TABLE `ligne` DISABLE KEYS */;
INSERT INTO `ligne` VALUES (1,'Ligne 1'),(2,'Ligne 2'),(3,'Ligne 3'),(4,'Ligne 3bis'),(5,'Ligne 4'),(6,'Ligne 5'),(7,'Ligne 6'),(8,'Ligne 7'),(9,'Ligne 7bis'),(10,'Ligne 8'),(11,'Ligne 9'),(12,'Ligne 10'),(13,'Ligne 11'),(14,'Ligne 12'),(15,'Ligne 13'),(16,'Ligne 14');
/*!40000 ALTER TABLE `ligne` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `lignedecommande`
--

DROP TABLE IF EXISTS `lignedecommande`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `lignedecommande` (
  `IdLigneCommande` int NOT NULL AUTO_INCREMENT,
  `IdCommande` int NOT NULL,
  `IdPlat` int NOT NULL,
  `Quantite` int NOT NULL,
  `DateLivraison` date NOT NULL,
  `LieuLivraison` varchar(255) NOT NULL,
  PRIMARY KEY (`IdLigneCommande`),
  KEY `IdCommande` (`IdCommande`),
  KEY `IdPlat` (`IdPlat`),
  CONSTRAINT `lignedecommande_ibfk_1` FOREIGN KEY (`IdCommande`) REFERENCES `commande` (`IdCommande`) ON DELETE CASCADE,
  CONSTRAINT `lignedecommande_ibfk_2` FOREIGN KEY (`IdPlat`) REFERENCES `plat` (`IdPlat`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `lignedecommande`
--

LOCK TABLES `lignedecommande` WRITE;
/*!40000 ALTER TABLE `lignedecommande` DISABLE KEYS */;
INSERT INTO `lignedecommande` VALUES (1,1,1,2,'2025-03-02','10 rue de Paris'),(2,3,3,4,'2025-03-04','22 boulevard Haussmann'),(3,4,4,2,'2025-03-05','5 place Bellecour'),(4,5,5,3,'2025-03-06','18 rue de Marseille'),(5,6,2,1,'2025-03-06','31 rue du Faubourg Saint-Antoine'),(6,7,1,2,'2025-03-07','12 avenue des Champs-Élysées'),(7,8,3,5,'2025-03-08','8 rue de la République'),(8,9,4,1,'2025-03-09','3 rue Nationale'),(9,10,5,2,'2025-03-10','40 rue de Rennes'),(10,2,2,1,'2025-03-03','15 avenue de Lyon');
/*!40000 ALTER TABLE `lignedecommande` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `listeingredients`
--

DROP TABLE IF EXISTS `listeingredients`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `listeingredients` (
  `IdIngredient` int NOT NULL,
  `IdRecette` int NOT NULL,
  `Quantite` int DEFAULT '1',
  PRIMARY KEY (`IdIngredient`,`IdRecette`),
  KEY `IdRecette` (`IdRecette`),
  CONSTRAINT `listeingredients_ibfk_1` FOREIGN KEY (`IdRecette`) REFERENCES `recette` (`IdRecette`) ON DELETE CASCADE,
  CONSTRAINT `listeingredients_ibfk_2` FOREIGN KEY (`IdIngredient`) REFERENCES `ingredient` (`IdIngredient`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `listeingredients`
--

LOCK TABLES `listeingredients` WRITE;
/*!40000 ALTER TABLE `listeingredients` DISABLE KEYS */;
INSERT INTO `listeingredients` VALUES (1,1,3),(2,2,2),(2,4,5),(5,50,1),(7,2,3),(7,4,2),(8,1,3),(9,50,4),(10,1,5),(12,50,3),(13,2,2),(14,3,1),(16,5,5),(23,6,4),(24,1,2),(25,7,4),(28,1,4),(31,50,4),(34,1,5),(35,2,5),(39,2,5),(39,6,1),(42,2,1),(44,3,3),(45,5,4),(47,6,1);
/*!40000 ALTER TABLE `listeingredients` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `livraison`
--

DROP TABLE IF EXISTS `livraison`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `livraison` (
  `IdLivraison` int NOT NULL AUTO_INCREMENT,
  `IdLigneCommande` int NOT NULL,
  `IdLivreur` int NOT NULL,
  `IdStationDepart` int DEFAULT NULL,
  `IdStationArrivee` int DEFAULT NULL,
  `Statut` enum('En attente','En cours','Livrée') DEFAULT 'En attente',
  PRIMARY KEY (`IdLivraison`),
  KEY `IdLigneCommande` (`IdLigneCommande`),
  KEY `IdLivreur` (`IdLivreur`),
  KEY `IdStationDepart` (`IdStationDepart`),
  KEY `IdStationArrivee` (`IdStationArrivee`),
  CONSTRAINT `livraison_ibfk_1` FOREIGN KEY (`IdLigneCommande`) REFERENCES `lignedecommande` (`IdLigneCommande`) ON DELETE CASCADE,
  CONSTRAINT `livraison_ibfk_2` FOREIGN KEY (`IdLivreur`) REFERENCES `utilisateur` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `livraison_ibfk_3` FOREIGN KEY (`IdStationDepart`) REFERENCES `station` (`IdStation`) ON DELETE CASCADE,
  CONSTRAINT `livraison_ibfk_4` FOREIGN KEY (`IdStationArrivee`) REFERENCES `station` (`IdStation`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `livraison`
--

LOCK TABLES `livraison` WRITE;
/*!40000 ALTER TABLE `livraison` DISABLE KEYS */;
INSERT INTO `livraison` VALUES (1,1,1,1,2,'En attente'),(2,1,10,47,11,'En cours'),(3,2,4,43,14,'En attente'),(4,1,3,8,16,'En attente'),(5,2,8,27,50,'En attente'),(6,1,5,50,36,'Livrée'),(7,2,4,29,48,'En attente'),(8,1,5,30,22,'En attente'),(9,2,4,16,21,'En cours'),(10,1,6,19,3,'En cours'),(11,1,8,42,2,'En cours'),(12,1,4,34,40,'En cours'),(13,2,2,34,30,'En cours'),(14,1,7,25,36,'Livrée'),(15,1,3,31,31,'Livrée'),(16,1,6,48,22,'En attente'),(17,2,2,2,1,'En cours');
/*!40000 ALTER TABLE `livraison` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `musique`
--

DROP TABLE IF EXISTS `musique`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `musique` (
  `IdMusique` int NOT NULL AUTO_INCREMENT,
  `Titre` varchar(255) NOT NULL,
  `Nationalite` varchar(250) NOT NULL,
  `IdPlat` int DEFAULT NULL,
  PRIMARY KEY (`IdMusique`),
  KEY `IdPlat` (`IdPlat`),
  CONSTRAINT `musique_ibfk_1` FOREIGN KEY (`IdPlat`) REFERENCES `plat` (`IdPlat`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `musique`
--

LOCK TABLES `musique` WRITE;
/*!40000 ALTER TABLE `musique` DISABLE KEYS */;
INSERT INTO `musique` VALUES (1,'Bella Ciao','Italienne',1),(2,'Sakura','Japonaise',5),(3,'Habibi Funk','Marocaine',6),(4,'Pad Thai Song','Thaïlandaise',2),(5,'Smooth Jazz Dinner','Américaine',3),(6,'Bollywood Beats','Indienne',NULL),(7,'Accordéon Parisien','Française',NULL),(8,'Flamenco Andalou','Espagnole',NULL),(9,'Samba Brasileira','Brésilienne',NULL),(10,'Soukous Congo','Congolaise',NULL),(11,'Fado de Lisbonne','Portugaise',NULL),(12,'Jazz New Orleans','Américaine',4),(13,'Tango Argentino','Argentine',NULL),(14,'Bossa Nova Breeze','Brésilienne',NULL),(15,'Gnawa Spirit','Marocaine',NULL),(16,'Oriental Lounge','Libanaise',NULL),(17,'Afrobeat Chill','Nigériane',NULL),(18,'Lo-Fi Chill Beats','Internationale',NULL),(19,'Classique Français','Française',NULL),(20,'Musette à Montmartre','Française',NULL);
/*!40000 ALTER TABLE `musique` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `notationcuisinier`
--

DROP TABLE IF EXISTS `notationcuisinier`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `notationcuisinier` (
  `IdNotation` int NOT NULL AUTO_INCREMENT,
  `IdCuisinier` int NOT NULL,
  `Note` int DEFAULT NULL,
  `Commentaire` text,
  `DateNotation` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`IdNotation`),
  KEY `IdCuisinier` (`IdCuisinier`),
  CONSTRAINT `notationcuisinier_ibfk_1` FOREIGN KEY (`IdCuisinier`) REFERENCES `cuisinier` (`IdCuisinier`) ON DELETE CASCADE,
  CONSTRAINT `notationcuisinier_chk_1` CHECK (((`Note` >= 1) and (`Note` <= 5)))
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `notationcuisinier`
--

LOCK TABLES `notationcuisinier` WRITE;
/*!40000 ALTER TABLE `notationcuisinier` DISABLE KEYS */;
INSERT INTO `notationcuisinier` VALUES (1,1,1,'Commentaire pour cuisinier 1','2025-04-02 09:08:26'),(2,2,4,'Commentaire pour cuisinier 2','2025-03-31 15:10:36'),(3,3,2,'Commentaire pour cuisinier 3','2025-03-27 19:45:00'),(4,4,2,'Commentaire pour cuisinier 4','2025-03-19 01:51:02'),(5,5,4,'Commentaire pour cuisinier 5','2025-03-17 13:47:50'),(6,6,1,'Commentaire pour cuisinier 6','2025-03-26 00:40:35'),(7,7,4,'Commentaire pour cuisinier 7','2025-03-15 19:44:11'),(8,8,4,'Commentaire pour cuisinier 8','2025-04-02 03:51:33'),(9,9,1,'Commentaire pour cuisinier 9','2025-03-17 09:19:18'),(10,10,5,'Commentaire pour cuisinier 10','2025-03-28 09:56:26');
/*!40000 ALTER TABLE `notationcuisinier` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plat`
--

DROP TABLE IF EXISTS `plat`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `plat` (
  `IdPlat` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(100) NOT NULL,
  `Prix` decimal(10,2) NOT NULL,
  `IdCuisinier` int NOT NULL,
  `Type` enum('Entrée','Plat Principal','Dessert') NOT NULL,
  `Personnes` int NOT NULL,
  `DateFabrication` date NOT NULL,
  `DatePeremption` date NOT NULL,
  `Regime` varchar(50) NOT NULL,
  `IdRecette` int NOT NULL,
  `CheminAccesPhoto` varchar(255) DEFAULT NULL,
  `Nationalite` varchar(255) NOT NULL,
  `Proteines` decimal(10,2) NOT NULL,
  PRIMARY KEY (`IdPlat`),
  KEY `IdRecette` (`IdRecette`),
  KEY `IdCuisinier` (`IdCuisinier`),
  CONSTRAINT `plat_ibfk_1` FOREIGN KEY (`IdRecette`) REFERENCES `recette` (`IdRecette`) ON DELETE CASCADE,
  CONSTRAINT `plat_ibfk_2` FOREIGN KEY (`IdCuisinier`) REFERENCES `utilisateur` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plat`
--

LOCK TABLES `plat` WRITE;
/*!40000 ALTER TABLE `plat` DISABLE KEYS */;
INSERT INTO `plat` VALUES (1,'Lasagnes',12.50,8,'Plat Principal',8,'2025-03-01','2025-03-03','Végétarien',1,'lasagnes.jpg','Italienne',19.00),(2,'Pad Thaï au poulet',14.08,10,'Plat Principal',8,'2025-04-06','2025-04-08','Classique',1,'pad_thai_au_poulet.jpg','Thaïlandaise',26.70),(3,'Burger végétarien',17.48,9,'Plat Principal',9,'2025-04-06','2025-04-09','Végétarien',2,'burger_vegetarien.jpg','Américaine',32.10),(4,'Pizza Margherita',11.70,10,'Plat Principal',10,'2025-04-06','2025-04-09','Végétarien',3,'pizza_margherita.jpg','Italienne',27.70),(5,'Sushi saumon avocat',16.41,9,'Plat Principal',9,'2025-04-04','2025-04-07','Sans gluten',4,'sushi_saumon_avocat.jpg','Japonaise',16.00),(6,'Couscous royal',19.98,8,'Plat Principal',10,'2025-04-04','2025-04-05','Halal',5,'couscous_royal.jpg','Marocaine',21.40),(7,'Soupe courge coco',18.98,10,'Entrée',8,'2025-04-03','2025-04-05','Vegan',50,'soupe_courge_coco.jpg','Fusion',16.60),(8,'Couscous',15.00,2,'Plat Principal',9,'2025-03-02','2025-03-04','Halal',2,'couscous.jpg','Marocaine',19.80);
/*!40000 ALTER TABLE `plat` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `platcategorieambiance`
--

DROP TABLE IF EXISTS `platcategorieambiance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `platcategorieambiance` (
  `IdPlat` int NOT NULL,
  `IdCategorie` int NOT NULL,
  PRIMARY KEY (`IdPlat`,`IdCategorie`),
  KEY `IdCategorie` (`IdCategorie`),
  CONSTRAINT `platcategorieambiance_ibfk_1` FOREIGN KEY (`IdPlat`) REFERENCES `plat` (`IdPlat`) ON DELETE CASCADE,
  CONSTRAINT `platcategorieambiance_ibfk_2` FOREIGN KEY (`IdCategorie`) REFERENCES `categorieambiance` (`IdCategorie`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `platcategorieambiance`
--

LOCK TABLES `platcategorieambiance` WRITE;
/*!40000 ALTER TABLE `platcategorieambiance` DISABLE KEYS */;
INSERT INTO `platcategorieambiance` VALUES (1,1),(6,1),(2,2),(3,3),(4,4),(5,5),(7,6),(8,7),(1,8),(2,9);
/*!40000 ALTER TABLE `platcategorieambiance` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `recette`
--

DROP TABLE IF EXISTS `recette`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recette` (
  `IdRecette` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(100) NOT NULL,
  PRIMARY KEY (`IdRecette`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `recette`
--

LOCK TABLES `recette` WRITE;
/*!40000 ALTER TABLE `recette` DISABLE KEYS */;
INSERT INTO `recette` VALUES (1,'Ratatouille'),(2,'Tarte aux fraises'),(3,'Poulet basquaise'),(4,'Couscous royal'),(5,'Gratin dauphinois'),(6,'Soupe à l’oignon'),(7,'Tajine d’agneau'),(8,'Lasagnes végétariennes'),(9,'Tarte tatin'),(10,'Paella valencienne'),(11,'Boeuf bourguignon'),(12,'Quiche lorraine'),(13,'Pizza margherita'),(14,'Spaghetti carbonara'),(15,'Poulet curry coco'),(16,'Pad thaï'),(17,'Falafel maison'),(18,'Soupe pho'),(19,'Chili con carne'),(20,'Riz cantonais'),(21,'Biryani au poulet'),(22,'Gnocchis à la crème'),(23,'Hamburger maison'),(24,'Bouillon asiatique'),(25,'Croque-monsieur'),(26,'Nuggets de poulet'),(27,'Wok de légumes'),(28,'Raviolis vapeur'),(29,'Gratin de courgettes'),(30,'Burger végétarien'),(31,'Gratin de pâtes'),(32,'Curry de pois chiches'),(33,'Tacos mexicains'),(34,'Banh mi au porc'),(35,'Poisson pané maison'),(36,'Tartiflette'),(37,'Soupe courge-lait coco'),(38,'Rouleaux de printemps'),(39,'Gaspacho'),(40,'Tartes aux pommes'),(41,'Clafoutis cerise'),(42,'Cookies au chocolat'),(43,'Brownie fondant'),(44,'Cheesecake citron'),(45,'Crème brûlée'),(46,'Mousse au chocolat'),(47,'Tiramisu classique'),(48,'Moelleux au chocolat'),(49,'Madeleines'),(50,'Flan pâtissier');
/*!40000 ALTER TABLE `recette` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `station`
--

DROP TABLE IF EXISTS `station`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `station` (
  `IdStation` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(100) NOT NULL,
  `Latitude` float NOT NULL,
  `Longitude` float NOT NULL,
  PRIMARY KEY (`IdStation`)
) ENGINE=InnoDB AUTO_INCREMENT=51 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `station`
--

LOCK TABLES `station` WRITE;
/*!40000 ALTER TABLE `station` DISABLE KEYS */;
INSERT INTO `station` VALUES (1,'Châtelet',48.8581,2.347),(2,'Gare de Lyon',48.8443,2.3744),(3,'Bastille',48.853,2.369),(4,'Nation',48.8485,2.3958),(5,'Montparnasse',48.8414,2.3209),(6,'Porte de Clignancourt',48.8983,2.3446),(7,'République',48.8674,2.3632),(8,'Opéra',48.8704,2.332),(9,'Place d\'Italie',48.8311,2.3552),(10,'Trocadéro',48.8633,2.2889),(11,'Saint-Lazare',48.8756,2.3259),(12,'La Motte-Picquet – Grenelle',48.8492,2.2986),(13,'Denfert-Rochereau',48.8331,2.3321),(14,'Alésia',48.8283,2.3266),(15,'Porte d\'Orléans',48.8254,2.3259),(16,'Porte de Vincennes',48.8489,2.4164),(17,'Porte de Bagnolet',48.8671,2.4091),(18,'Gambetta',48.8642,2.3984),(19,'Ménilmontant',48.8662,2.3897),(20,'Père Lachaise',48.8619,2.3889),(21,'Faidherbe – Chaligny',48.8494,2.3852),(22,'Voltaire',48.8573,2.3807),(23,'Richard-Lenoir',48.8579,2.3736),(24,'Oberkampf',48.8644,2.3693),(25,'Strasbourg – Saint-Denis',48.8692,2.3541),(26,'Bonne Nouvelle',48.8713,2.3498),(27,'Grands Boulevards',48.8729,2.3448),(28,'Richelieu – Drouot',48.8721,2.3378),(29,'Cadet',48.8767,2.3433),(30,'Poissonnière',48.8793,2.3518),(31,'Barbès – Rochechouart',48.8822,2.3495),(32,'Château Rouge',48.8846,2.3445),(33,'Abbesses',48.8842,2.3387),(34,'Lamarck – Caulaincourt',48.8896,2.3375),(35,'Jules Joffrin',48.8927,2.3416),(36,'Simplon',48.8952,2.3445),(37,'Marcadet – Poissonniers',48.8921,2.3498),(38,'Marx Dormoy',48.8922,2.3594),(39,'La Chapelle',48.8843,2.3625),(40,'Stalingrad',48.8842,2.3707),(41,'Jaurès',48.8846,2.3776),(42,'Louis Blanc',48.8833,2.3761),(43,'Château-Landon',48.8796,2.3697),(44,'Gare de l\'Est',48.8763,2.3594),(45,'Jacques Bonsergent',48.8708,2.3629),(46,'Arts et Métiers',48.8663,2.3561),(47,'Temple',48.8678,2.3617),(48,'Réaumur – Sébastopol',48.8682,2.3524),(49,'Sentier',48.8689,2.3481),(50,'Pigalle',48.882,2.3371);
/*!40000 ALTER TABLE `station` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `transaction`
--

DROP TABLE IF EXISTS `transaction`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `transaction` (
  `IdTransaction` int NOT NULL AUTO_INCREMENT,
  `IdCommande` int NOT NULL,
  `Montant` decimal(10,2) NOT NULL,
  `Reussie` tinyint(1) DEFAULT '0',
  `DateTransaction` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`IdTransaction`),
  KEY `IdCommande` (`IdCommande`),
  CONSTRAINT `transaction_ibfk_1` FOREIGN KEY (`IdCommande`) REFERENCES `commande` (`IdCommande`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=21 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `transaction`
--

LOCK TABLES `transaction` WRITE;
/*!40000 ALTER TABLE `transaction` DISABLE KEYS */;
INSERT INTO `transaction` VALUES (1,1,25.00,0,'2025-04-04 17:41:20'),(2,1,57.29,1,'2025-03-31 00:47:12'),(3,2,57.60,0,'2025-04-03 19:37:12'),(4,3,20.23,0,'2025-03-26 02:04:18'),(5,4,38.26,0,'2025-03-26 04:14:15'),(6,5,30.68,0,'2025-03-31 12:16:54'),(7,6,58.54,0,'2025-03-24 11:56:54'),(8,7,37.87,0,'2025-03-28 20:25:14'),(9,8,19.70,0,'2025-03-27 21:00:14'),(10,9,56.10,0,'2025-04-03 19:39:10'),(11,10,21.58,0,'2025-03-27 21:18:18'),(12,11,21.87,1,'2025-04-03 12:02:51'),(13,12,31.09,1,'2025-03-29 20:51:54'),(14,13,10.75,1,'2025-03-31 16:12:13'),(15,14,37.64,0,'2025-03-27 15:20:02'),(16,15,50.03,0,'2025-03-25 19:14:42'),(17,2,15.00,1,'2025-04-04 17:41:20'),(18,3,12.00,1,'2025-04-04 17:41:20'),(19,4,13.00,1,'2025-04-04 17:41:20'),(20,5,18.00,1,'2025-04-04 17:41:20');
/*!40000 ALTER TABLE `transaction` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `utilisateur`
--

DROP TABLE IF EXISTS `utilisateur`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `utilisateur` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Nom` varchar(50) NOT NULL,
  `Prenom` varchar(50) NOT NULL,
  `Adresse` varchar(255) NOT NULL,
  `Telephone` varchar(10) NOT NULL,
  `Email` varchar(50) NOT NULL,
  `IdCuisinier` int DEFAULT NULL,
  `IdClient` int DEFAULT NULL,
  `IdStationProche` int DEFAULT NULL,
  `EstBanni` tinyint(1) DEFAULT '0',
  `PointFidelite` int DEFAULT '0',
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Email` (`Email`),
  KEY `IdCuisinier` (`IdCuisinier`),
  KEY `IdClient` (`IdClient`),
  KEY `IdStationProche` (`IdStationProche`),
  CONSTRAINT `utilisateur_ibfk_1` FOREIGN KEY (`IdCuisinier`) REFERENCES `cuisinier` (`IdCuisinier`) ON DELETE CASCADE,
  CONSTRAINT `utilisateur_ibfk_2` FOREIGN KEY (`IdClient`) REFERENCES `client` (`IdClient`) ON DELETE CASCADE,
  CONSTRAINT `utilisateur_ibfk_3` FOREIGN KEY (`IdStationProche`) REFERENCES `station` (`IdStation`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `utilisateur`
--

LOCK TABLES `utilisateur` WRITE;
/*!40000 ALTER TABLE `utilisateur` DISABLE KEYS */;
INSERT INTO `utilisateur` VALUES (1,'Dupond','Jean','10 rue de Paris','0123456789','jean.dupond@email.com',NULL,1,1,0,0),(2,'Marie','Tom','11 rue de la Paix','0123486489','tom.marie@email.com',NULL,2,1,0,0),(3,'Jarrar','Cédric','11 rue des Lampes','012343030','cedric.jarrar@email.com',NULL,3,1,0,0),(4,'Lévêque','Agathe','86 chemin Pons, 75018 Paris','2341750628','maurychristophe@guillon.fr',47,NULL,1,0,0),(5,'Mary','Margaud','36 avenue Guillaume Cohen, 75004 Paris','9558684561','hamonphilippine@durand.fr',NULL,22,2,0,0),(6,'Le Roux','Alexandre','29 rue de Perrier, 75019 Paris','2741698447','ferrandtherese@tiscali.fr',22,NULL,3,0,0),(7,'Klein','Monique','512 chemin de Deschamps, 75002 Paris','6236928014','thibaultmahe@hotmail.fr',NULL,31,4,0,0),(8,'Diaz','Stéphane','697 boulevard de Bouvet, 75007 Paris','9121765847','sauvagegenevieve@yahoo.fr',6,NULL,5,0,0),(9,'Guérin','Frédéric','451 boulevard Roussel, 75004 Paris','8026335138','marthegerard@gros.com',14,NULL,50,0,0),(10,'Martin','Sofie','15 avenue de Lyon','0987654321','sofie.martin@email.com',1,NULL,2,0,0);
/*!40000 ALTER TABLE `utilisateur` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-04-04 17:43:45
