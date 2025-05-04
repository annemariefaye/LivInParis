<?php

ini_set('display_errors', 1);
error_reporting(E_ALL);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    echo "Erreur de connexion MySQL : " . mysqli_connect_error();
    exit();
}

$nomUtilisateur = $_POST['nomutilisateur'];

if (empty($nomUtilisateur)) {
    echo "Nom d'utilisateur nécessaire.";
    exit();
}

$getUserQuery = "SELECT * FROM Utilisateur WHERE NomUtilisateur = '$nomUtilisateur'";
$result = mysqli_query($con, $getUserQuery);

if (!$result) {
    echo "Erreur lors de la requête SELECT : " . mysqli_error($con);
    exit();
}

if (mysqli_num_rows($result) == 0) {
    echo "Utilisateur introuvable.";
    exit();
}

$userData = mysqli_fetch_assoc($result);
$idCuisinier = $userData['IdCuisinier'];

$updateUserQuery = "UPDATE Utilisateur SET IdCuisinier = NULL WHERE NomUtilisateur = '$nomUtilisateur'";
if (!mysqli_query($con, $updateUserQuery)) {
    echo "Erreur de mise à jour de l'utilisateur : " . mysqli_error($con);
    exit();
}

$deleteCuisinierQuery = "DELETE FROM Cuisinier WHERE IdCuisinier = '$idCuisinier'";
if (!mysqli_query($con, $deleteCuisinierQuery)) {
    echo "Erreur lors de la suppression du cuisinier : " . mysqli_error($con);
    exit();
}

echo "0";

mysqli_close($con);
?>
