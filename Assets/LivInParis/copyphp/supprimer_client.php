<?php

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
$idClient = $userData['IdClient'];

$updateUserQuery = "UPDATE Utilisateur SET IdClient = NULL WHERE NomUtilisateur = '$nomUtilisateur'";
if (!mysqli_query($con, $updateUserQuery)) {
    echo "Erreur de mise à jour de l'utilisateur : " . mysqli_error($con);
    exit();
}

$deleteClientQuery = "DELETE FROM Client WHERE IdClient = $idClient";
if (!mysqli_query($con, $deleteClientQuery)) {
    echo "Erreur lors de la suppression du client : " . mysqli_error($con);
    exit();
}

echo "0";

mysqli_close($con);
?>
