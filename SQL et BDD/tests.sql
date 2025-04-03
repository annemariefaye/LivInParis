SELECT Utilisateur.*, m.total AS total_commandes
FROM Utilisateur
LEFT JOIN (
    SELECT Commande.idClient, SUM(Transaction.Montant) AS total
    FROM Transaction
    JOIN Commande ON Commande.idCommande = Transaction.idCommande
    WHERE Transaction.Reussie = 1
    GROUP BY Commande.idClient
) AS m ON m.idClient = Utilisateur.idClient;
