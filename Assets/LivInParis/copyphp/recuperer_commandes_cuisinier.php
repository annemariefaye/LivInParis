<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $nomUtilisateur = mysqli_real_escape_string($con, $_POST['nomutilisateur']);

    $getIdQuery = "SELECT Id FROM Utilisateur WHERE NomUtilisateur = '$nomUtilisateur'";
    $getIdResult = mysqli_query($con, $getIdQuery);

    if (!$getIdResult || mysqli_num_rows($getIdResult) === 0) {
        echo "Erreur : cuisinier introuvable";
        exit();
    }

    $idCuisinier = mysqli_fetch_assoc($getIdResult)['Id'];

    $query = "
        SELECT p.Nom AS NomPlat, c.DateCommande,  c.IdCommande, u.Prenom, u.Nom AS NomClient, c.Statut
        FROM Commande c
        JOIN LigneDeCommande lc ON c.IdCommande = lc.IdCommande
        JOIN Plat p ON lc.IdPlat = p.IdPlat
        JOIN Utilisateur u ON c.IdClient = u.Id
        WHERE p.IdCuisinier = '$idCuisinier'
    ";

    $result = mysqli_query($con, $query);

    if (!$result) {
        echo "Erreur SQL : " . mysqli_error($con);
        exit();
    }

    while ($row = mysqli_fetch_assoc($result)) {
        echo $row['NomPlat'] . "\t" . $row['DateCommande'] . "\t" . $row['NomClient'] . "\t" . $row['Prenom'] . "\t" . $row['Statut'] . "\t" . $row['IdCommande'] . "\n";
    }
}

mysqli_close($con);
?>
