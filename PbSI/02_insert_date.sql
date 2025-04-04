USE LivInParis;

INSERT INTO Client (NomEntreprise, MotDePasse) VALUES
('Boulangerie Dupont', 'mdp123'),
('Entreprise XYZ', 'xyz789');

INSERT INTO Cuisinier (MotDePasse, PlatDuJour) VALUES
('cuisine123', 'sushi'),
('chef789', 'burger'),
('allinedessine92', 'raclette');

INSERT INTO Station (Nom, Latitude, Longitude) VALUES
('Gare de Lyon', 40.8632, 5.4764),
('Châtelet', 45.3880, 3.3489);


INSERT INTO Utilisateur (Nom, Prenom, Adresse, Telephone, Email, IdClient, IdStationProche, EstBanni) VALUES
('Dupond', 'Louis', '8 rue Sainte-Anne, 75001 Paris', '0123456789', 'louis.dupond@email.com', 1, 1, FALSE),
('Martin', 'Clara', '55 Rue du Faubourg Saint-Honoré, 75008 Paris', '0987654321', 'clara.martin@email.com', 2, 2, FALSE);


INSERT INTO Utilisateur (Nom, Prenom, Adresse, Telephone, Email, IdCuisinier, IdStationProche, EstBanni) VALUES
('Choi', 'Min-Jun', '20 avenue des Champs-Élysées, 75008 Paris', '0147852369', 'minjun.choi@email.com', 1, 1, FALSE),
('Leroy', 'Amandine', '12 Rue de Rivoli, 75001 Paris', '0147852368', 'amandine.leroy@email.com', 2, 2, FALSE),
('Dumont', 'Julien', '15 Boulevard Montmartre, 75002 Paris', '0147852367', 'julien.dumont@email.com', 3, 1, FALSE);

INSERT INTO Recette (Nom) VALUES
('Ratatouille'),
('Tarte aux fraises');

INSERT INTO Plat (Nom, Prix, IdCuisinier, Type, Personnes, DateFabrication, DatePeremption, Regime, IdRecette, CheminAccesPhoto, Nationalite, Proteines) VALUES
('Lasagnes', 12.50, 1, 'Plat Principal', 2, '2025-03-01', '2025-03-03', 'Végétarien', 1, 'lasagnes.jpg', 'Italienne', 300),
('Couscous', 15.00, 2, 'Plat Principal', 4, '2025-03-02', '2025-03-04', 'Halal', 2, 'couscous.jpg', 'Marocaine', 250);

INSERT INTO Ingredient (Nom, Prix) VALUES
('Tomates', 2.50),
('Semoule', 1.20);

INSERT INTO ListeIngredients (IdIngredient, IdRecette, Quantite) VALUES
(1, 1, 3),
(2, 2, 2);

INSERT INTO Commande (IdClient, DateCommande, Statut) VALUES
(1, NOW(), 'En attente'),
(1, NOW(), 'Validée'),
(2, NOW(), 'Validée'),
(2, NOW(), 'Validée'),
(2, NOW(), 'Validée'),
(2, NOW(), 'Validée');

INSERT INTO LigneDeCommande (IdCommande, IdPlat, Quantite, DateLivraison, LieuLivraison) VALUES
(1, 1, 2, '2025-03-02', '10 rue de Paris'),
(2, 2, 1, '2025-03-03', '15 avenue de Lyon'),
(3, 2, 1, '2025-03-03', '15 avenue de Lyon'),
(4, 2, 1, '2025-03-03', '15 avenue de Lyon'),
(5, 2, 1, '2025-03-03', '15 avenue de Lyon'),
(6, 2, 1, '2025-03-03', '15 avenue de Lyon');

INSERT INTO Livraison (IdLigneCommande, IdLivreur, IdStationDepart, IdStationArrivee, Statut) VALUES
(1, 1, 1, 2, 'En attente'),
(2, 1, 2, 1, 'En cours'),
(3, 1, 2, 1, 'Livrée'),
(4, 1, 2, 1, 'Livrée'),
(5, 2, 2, 1, 'Livrée');

INSERT INTO Ligne (Nom) VALUES
('Ligne 1'),
('Ligne 14');

INSERT INTO Correspondance (IdStation, IdLigne) VALUES
(1, 1),
(2, 2);

INSERT INTO Transaction (IdCommande, Montant, Reussie, DateTransaction) VALUES
(1, 25.00, TRUE, NOW()),
(2, 15.00, TRUE, NOW()),
(3, 5.00, TRUE, NOW());



INSERT INTO CategorieAmbiance (Nom) VALUES 
('Brunch'),
('Déjeuner en famille'),
('Soirée romantique'),
('Soirée solo'),
('Anniversaire'),
('Enfant');


INSERT INTO PlatCategorieAmbiance (IdPlat, IdCategorie) VALUES 
(1, 1), 
(1, 3),  
(2, 2), 
(2, 4); 


INSERT INTO NotationCuisinier (IdCuisinier, Note, Commentaire) VALUES 
(1, 5, 'Excellent plat, très savoureux !'),
(2, 4, 'Bon service, mais le plat était un peu trop salé.'),
(1, 3, 'Correct, mais peut faire mieux.'),
(3, 5, 'Un vrai délice, je reviendrai !');


INSERT INTO Musique (Titre, Nationalite) VALUES 
('O Sole Mio', 'Italienne'),
('Café Tacuba', 'Marocaine'),
('La Vie en Rose', 'Française'),
('Despacito', 'Espagnole'),
('Kalinka', 'Russe'),
('Jiyuu no Tsubasa', 'Japonais');
