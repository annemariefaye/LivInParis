<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");
if (!$con) {
    die("Erreur de connexion : " . mysqli_connect_error());
}

$query = "
    SELECT u.NomUtilisateur, u.Nom, u.Prenom
        FROM Utilisateur u
        LEFT JOIN Client c ON u.IdClient = c.IdClient
        WHERE u.IdClient IS NOT NULL;
";

$result = mysqli_query($con, $query);

while ($row = mysqli_fetch_assoc($result)) {
    echo $row['NomUtilisateur'] . "\t" . $row['Nom'] . "\t" . $row['Prenom'] . "\n";
}

mysqli_close($con);
?>
