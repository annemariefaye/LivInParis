use livinparis;

INSERT INTO Cuisinier (PlatDuJour) VALUES 
('Lasagnes végétariennes'),
('Poulet basquaise'),
('Sushi variés');


INSERT INTO Utilisateur (NomUtilisateur, Hashing, Salt, Nom, Prenom, Adresse, Telephone, Email, IdCuisinier) VALUES 
('cuisinechloe', '$5$rounds=5000$steamedhamscuisinechloe$JekHnQji2D74yYMFUoHgox9wO.q6gTLH0oUQ50DIIM7', '$5$rounds=5000$steamedhamscuisinechloe$', 'Chloe', 'Martin', '75 rue de Paris, 75015 Paris', '0611223344', 'chloe@example.com', 1),

('chefjean', '$5$rounds=5000$steamedhamschefjean$L2Ds5lQyxcz8O7ZUI5wQ8IksLVhvMiYlWxRx0kKikm4', '$5$rounds=5000$steamedhamschefjean$', 'Jean', 'Dupont', '12 avenue des Champs, 75008 Paris', '0622334455', 'jean@example.com', 2),

('sushikaren', '$5$rounds=5000$steamedhamssushikaren$DY3FVPov.ZmpwFQHKjWRjw1JwXvDDxAwWpboOQ2skw8', '$5$rounds=5000$steamedhamssushikaren$', 'Karen', 'Ito', '4 rue du Japon, 75013 Paris', '0633445566', 'karen@example.com', 3);

INSERT INTO Recette (Nom) VALUES 
('Lasagnes végétariennes'),
('Tarte aux pommes'),
('Poulet basquaise'),
('Houmous et pain pita'),
('Sushi variés');

INSERT INTO Plat (Nom, Prix, IdCuisinier, Type, Personnes, DateFabrication, DatePeremption, Regime, IdRecette, CheminAccesPhoto, Nationalite, Proteines) 
VALUES 
('Lasagnes veggies', 12.50, 1, 'Plat Principal', 2, '2025-04-30', '2025-05-03', 'Végétarien', 1, 'lasagnes_vege', 'Italienne', 18.3),
('Tarte aux pommes', 6.00, 2, 'Dessert', 1, '2025-04-29', '2025-05-02', 'Végétarien', 2, 'tarte_pommes', 'Française', 3.5),
('Poulet basquaise', 14.90, 3, 'Plat Principal', 1, '2025-04-30', '2025-05-04', 'Sans Gluten', 3, 'poulet_basquaise', 'Française', 25.0),
('Houmous et pain pita', 8.50, 1, 'Entrée', 2, '2025-04-30', '2025-05-01', 'Vegan', 4, 'houmous', 'Moyen-Orientale', 7.8),
('Sushi du roi', 16.00, 2, 'Plat Principal', 1, '2025-04-30', '2025-05-01', 'Sans Lactose', 5, 'sushi', 'Japonaise', 21.2);

INSERT INTO Ingredient (Nom, Prix) 
VALUES 
('Tomates', 2.50),
('Farine', 1.20),
('Poulet', 5.00),
('Pain pita', 1.50),
('Algues nori', 3.80),
('Carottes', 1.80),
('Courgettes', 2.10),
('Lentilles', 2.30),
('Riz basmati', 1.60),
('Fromage râpé', 4.20),
('Crème fraîche', 3.00),
('Oignons', 1.00),
('Ail', 0.50),
('Coriandre', 1.50),
('Poivrons', 2.20),
('Avocat', 1.90),
('Haricots rouges', 2.00),
('Épinards', 2.00),
('Pommes de terre', 1.30),
('Moutarde', 0.80),
('Sauce soja', 2.40),
('Vinaigre balsamique', 1.70),
('Citron', 0.60),
('Cumin', 0.70),
('Paprika', 0.80);


INSERT INTO ListeIngredients (IdIngredient, IdRecette, Quantite) 
VALUES 
(1, 1, 3),  -- 3 Tomates pour la recette Lasagnes végétariennes
(2, 1, 2),  -- 2 Farine
(3, 1, 1),  -- 1 Poulet (si tu veux une version non-végétarienne)
(7, 1, 4),  -- 4 Courgettes
(8, 1, 2),  -- 2 Lentilles
(13, 1, 1),  -- 1 Ail
(19, 1, 2);  -- 2 Épinards


INSERT INTO ListeIngredients (IdIngredient, IdRecette, Quantite) 
VALUES 
(1, 2, 4),  -- 4 Pommes
(2, 2, 1),  -- 1 Farine
(3, 2, 1),  -- 1 Poulet (si version sucrée, tu peux ignorer)
(9, 2, 1),  -- 1 Fromage râpé (optionnel si version sucrée)
(22, 2, 1);  -- 1 Citron

INSERT INTO ListeIngredients (IdIngredient, IdRecette, Quantite) 
VALUES 
(3, 3, 3),  -- 3 Poulets
(12, 3, 2),  -- 2 Oignons
(8, 3, 1),  -- 1 Lentilles
(18, 3, 1),  -- 1 Haricots rouges
(19, 3, 1);  -- 1 Épinards


INSERT INTO ListeIngredients (IdIngredient, IdRecette, Quantite) 
VALUES 
(4, 4, 2),  -- 2 Pain pita
(12, 4, 1),  -- 1 Oignon
(16, 4, 3),  -- 3 Avocats
(11, 4, 1),  -- 1 Crème fraîche
(6, 4, 1);  -- 1 Carotte

INSERT INTO ListeIngredients (IdIngredient, IdRecette, Quantite) 
VALUES 
(5, 5, 2),  -- 2 Algues nori
(10, 5, 2),  -- 2 Sauce soja
(20, 5, 1),  -- 1 Vinaigre balsamique
(7, 5, 1),  -- 1 Courgette
(17, 5, 2);  -- 2 Haricots rouges



SELECT 
        Plat.IdPlat,
        Plat.Nom,
        Plat.Prix,
        Utilisateur.Nom AS NomCuisinier,
        Plat.CheminAccesPhoto,
        Plat.Regime,
        Utilisateur.Adresse
    FROM Plat
    INNER JOIN Utilisateur ON Plat.IdCuisinier = Utilisateur.Id
