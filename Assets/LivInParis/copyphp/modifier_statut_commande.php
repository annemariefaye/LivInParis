<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $idCommande = mysqli_real_escape_string($con, $_POST['idCommande']);
    $statut = mysqli_real_escape_string($con, $_POST['statut']);

    $query = "UPDATE Commande SET Statut = '$statut' WHERE IdCommande = '$idCommande'";

    if (mysqli_query($con, $query)) {
        echo "0";
    } else {
        echo "Erreur SQL : " . mysqli_error($con);
    }
}

mysqli_close($con);
?>
