DROP DATABASE IF EXISTS LivInParis;
CREATE DATABASE LivInParis;
USE LivInParis;

CREATE TABLE Client(
    IdClient INT AUTO_INCREMENT PRIMARY KEY,
    NomEntreprise VARCHAR (255),
    MotDePasse VARCHAR(255) NOT NULL   
);

CREATE TABLE Cuisinier(
    IdCuisinier INT AUTO_INCREMENT PRIMARY KEY,
    MotDePasse VARCHAR(255) NOT NULL   
);

CREATE TABLE Station (
    IdStation INT AUTO_INCREMENT PRIMARY KEY,
    Nom VARCHAR(100) NOT NULL,
    Latitude FLOAT NOT NULL,
    Longitude FLOAT NOT NULL
);


CREATE TABLE Utilisateur (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nom VARCHAR(50) NOT NULL,
    Prenom VARCHAR(50) NOT NULL,
    Adresse VARCHAR(255) NOT NULL,
    Telephone VARCHAR(10) NOT NULL,
    Email VARCHAR(50) NOT NULL UNIQUE,
    IdCuisinier INT DEFAULT NULL,
    IdClient INT DEFAULT NULL,
    IdStationProche INT,
    EstBanni BOOL DEFAULT FALSE,
    FOREIGN KEY (IdCuisinier) REFERENCES Cuisinier (IdCuisinier),
    FOREIGN KEY (IdClient) REFERENCES Client(IdClient),
    FOREIGN KEY (IdStationProche) REFERENCES Station(IdStation) 
);
CREATE TABLE Recette(
	IdRecette INT AUTO_INCREMENT PRIMARY KEY,
	Nom VARCHAR(100) NOT NULL
);

CREATE TABLE Plat (
    IdPlat INT AUTO_INCREMENT PRIMARY KEY,
    Nom VARCHAR(100) NOT NULL,
    Prix DECIMAL(10,2) NOT NULL,
    IdCuisinier INT NOT NULL,
    Type ENUM('Entrée', 'Plat Principal', 'Dessert') NOT NULL,
    Personnes INT NOT NULL,
    DateFabrication DATE NOT NULL,
    DatePeremption DATE NOT NULL,
    Regime VARCHAR(50) NOT NULL,
    IdRecette INT NOT NULL,
    CheminAccesPhoto VARCHAR(255) NULL,
    Nationalite VARCHAR (255) NOT NULL,
    FOREIGN KEY (IdRecette) REFERENCES Recette(IdRecette),
    FOREIGN KEY (IdCuisinier) REFERENCES Utilisateur(Id)
);
CREATE TABLE Ingredient(
	IdIngredient INT AUTO_INCREMENT PRIMARY KEY,
	Nom VARCHAR (100) NOT NULL,
	Prix DECIMAL (10,2) NOT NULL
);

CREATE TABLE ListeIngredients(
	IdIngredient INT NOT NULL,
	IdRecette INT NOT NULL,
	Quantite INT DEFAULT 1,
	FOREIGN KEY (IdRecette) REFERENCES Recette (IdRecette),
	FOREIGN KEY (IdIngredient) REFERENCES Ingredient(IdIngredient),
	PRIMARY KEY(IdIngredient, IdRecette)
);
CREATE TABLE Commande (
    IdCommande INT AUTO_INCREMENT PRIMARY KEY,
    IdClient INT NOT NULL,
    DateCommande DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    Statut ENUM('En attente', 'Validée', 'Livrée', 'Annulée') NOT NULL,
    FOREIGN KEY (IdClient) REFERENCES Utilisateur(Id)
);

CREATE TABLE LigneDeCommande (
    IdLigneCommande INT AUTO_INCREMENT PRIMARY KEY,
    IdCommande INT NOT NULL,
    IdPlat INT NOT NULL,
    Quantite INT NOT NULL,
    DateLivraison DATE NOT NULL,
    LieuLivraison VARCHAR(255) NOT NULL,
    FOREIGN KEY (IdCommande) REFERENCES Commande(IdCommande) ,
    FOREIGN KEY (IdPlat) REFERENCES Plat(IdPlat) 
);

CREATE TABLE Livraison (
    IdLivraison INT AUTO_INCREMENT PRIMARY KEY,
    IdLigneCommande INT NOT NULL,
    IdLivreur INT NOT NULL,
    IdStationDepart INT NULL,
    IdStationArrivee INT NULL,
    Statut ENUM('En attente', 'En cours', 'Livrée') DEFAULT 'En attente',
    FOREIGN KEY (IdLigneCommande) REFERENCES LigneDeCommande(IdLigneCommande) ,
    FOREIGN KEY (IdLivreur) REFERENCES Utilisateur(Id) ,
    FOREIGN KEY (IdStationDepart) REFERENCES Station(IdStation),
    FOREIGN KEY (IdStationArrivee) REFERENCES Station(IdStation)
);



CREATE TABLE Ligne (
	IdLigne INT AUTO_INCREMENT PRIMARY KEY,
    Nom VARCHAR(255)
);

CREATE TABLE Correspondance(
	IdStation INT NOT NULL,
	IdLigne INT NOT NULL,
	FOREIGN KEY (IdStation) REFERENCES Station(IdStation),
	FOREIGN KEY (IdLigne) REFERENCES Ligne(IdLigne),
	PRIMARY KEY (IdLigne, IdStation)
);

CREATE TABLE Transaction (
    IdTransaction INT AUTO_INCREMENT PRIMARY KEY,
    IdCommande INT NOT NULL,
    Montant DECIMAL(10,2) NOT NULL,
    Reussie BOOL DEFAULT FALSE,
    DateTransaction DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (IdCommande) REFERENCES Commande(IdCommande) 
);

