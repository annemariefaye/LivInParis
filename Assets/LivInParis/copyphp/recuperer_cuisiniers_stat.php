<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");
if (!$con) {
    die("Erreur de connexion : " . mysqli_connect_error());
}

$query = "
   SELECT 
    u.Nom,
    u.Prenom,
    COUNT(DISTINCT c.IdCommande) AS NbCommandesLivrees
FROM Utilisateur u
JOIN Plat p ON p.IdCuisinier = u.Id
JOIN LigneDeCommande lc ON lc.IdPlat = p.IdPlat
JOIN Livraison l ON l.IdLigneCommande = lc.IdLigneCommande
JOIN Commande c ON c.IdCommande = lc.IdCommande
WHERE l.Statut = 'Livrée'
GROUP BY u.Id;


";

$result = mysqli_query($con, $query);

while ($row = mysqli_fetch_assoc($result)) {
    echo $row['Nom'] . "\t" . $row['Prenom'] . "\t" . $row['NbCommandesLivrees'] . "\n";
}

mysqli_close($con);
?>
