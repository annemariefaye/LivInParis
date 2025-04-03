USE LivInParis;

SELECT * FROM Client;
SELECT * FROM Cuisinier;
SELECT Nom, Prix, Type, Nationalite FROM Plat;
SELECT * FROM Commande WHERE Statut = 'En attente';
SELECT * FROM Plat WHERE Prix < 20.00;
SELECT IdClient, COUNT(*) AS NombreCommandes 
FROM Commande 
GROUP BY IdClient 
HAVING COUNT(*) > 3;
SELECT IdLivreur, COUNT(*) AS TotalLivraisons
FROM Livraison
GROUP BY IdLivreur;
SELECT IdPlat, COUNT(*) AS NombreCommandes
FROM LigneDeCommande
GROUP BY IdPlat
ORDER BY NombreCommandes DESC;
SELECT SUM(Montant) AS ChiffreAffairesTotal FROM Transaction WHERE Reussie = TRUE;
SELECT IdStationArrivee, COUNT(*) AS TotalLivraisons
FROM Livraison
GROUP BY IdStationArrivee
ORDER BY TotalLivraisons DESC;
SELECT AVG(Montant) AS MontantMoyen FROM Transaction;
