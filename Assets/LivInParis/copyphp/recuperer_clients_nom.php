<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");
if (!$con) {
    die("Erreur de connexion : " . mysqli_connect_error());
}

$query = "
SELECT 
    u.Id AS IdUtilisateur,
    u.NomUtilisateur,
    u.Nom,
    u.Prenom,
    u.Adresse,
    SUM(t.Montant) AS MontantTotal
FROM Utilisateur u
JOIN Commande c ON u.Id = c.IdClient
JOIN Transaction t ON c.IdCommande = t.IdCommande
WHERE u.IdClient IS NOT NULL
GROUP BY u.Id, u.NomUtilisateur, u.Nom, u.Prenom
ORDER BY Nom
;
";

$result = mysqli_query($con, $query);

while ($row = mysqli_fetch_assoc($result)) {
    echo $row['NomUtilisateur'] . "\t" . $row['Nom'] . "\t" . $row['Prenom'] . "\t" . $row['Adresse'] . "\t" . $row['MontantTotal'] . "\n";
}

mysqli_close($con);
?>
