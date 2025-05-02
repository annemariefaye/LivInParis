<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");
if (!$con) {
    die("Erreur de connexion : " . mysqli_connect_error());
}

$query = "
    SELECT u.Id, u.NomUtilisateur, u.Nom, u.Prenom, c.PlatDuJour
        FROM Utilisateur u
        LEFT JOIN Cuisinier c ON u.IdCuisinier = c.IdCuisinier
        WHERE u.IdCuisinier IS NOT NULL;
";

$result = mysqli_query($con, $query);

while ($row = mysqli_fetch_assoc($result)) {
    echo $row['Id'] . "\t" . $row['NomUtilisateur'] . "\t" . $row['Nom'] . "\t" . $row['Prenom'] . "\t" . $row['PlatDuJour'] . "\n";
}

mysqli_close($con);
?>
