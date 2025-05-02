<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");
if (!$con) {
    die("Erreur de connexion : " . mysqli_connect_error());
}

$query = "
   SELECT 
    u.NomUtilisateur, 
    u.Nom, 
    u.Prenom, 
    ROUND(AVG(lc.Quantite * p.Prix), 2) AS MoyenneCommande
FROM Utilisateur u
JOIN Commande co ON co.IdClient = u.Id
JOIN LigneDeCommande lc ON lc.IdCommande = co.IdCommande
JOIN Plat p ON lc.IdPlat = p.IdPlat
WHERE u.IdClient IS NOT NULL
GROUP BY u.Id;

";

$result = mysqli_query($con, $query);

while ($row = mysqli_fetch_assoc($result)) {
    echo $row['Nom'] . "\t" . $row['Prenom'] . "\t" . $row['MoyenneCommande'] . "\n";
}

mysqli_close($con);
?>
