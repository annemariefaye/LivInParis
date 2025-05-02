<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

if(mysqli_connect_errno()) {
    echo "1 : Echec de la connexion à MySQL"; 
    exit();
}

$nomEntreprise = $_POST['nomentreprise'];

$insertClientQuery = "INSERT INTO Client (NomEntreprise) VALUES ('$nomEntreprise')";
mysqli_query($con, $insertClientQuery) or die("Erreur création client");

$idClient = mysqli_insert_id($con);

$nomUtilisateur = $_POST['nomutilisateur'];

$updateUserQuery = "UPDATE Utilisateur SET IdClient = '$idClient' WHERE NomUtilisateur = '$nomUtilisateur'";
mysqli_query($con, $updateUserQuery) or die("Erreur association client-utilisateur");

echo "0";

?>
