<?php

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

$nomUtilisateur = $_POST['nomutilisateur'];

if (empty($nomUtilisateur)) {
    echo "Nom d'utilisateur nécessaire.";
    exit();
}

$getUserQuery = "SELECT * FROM Utilisateur WHERE NomUtilisateur = '$nomUtilisateur'";
$result = mysqli_query($con, $getUserQuery);

if (mysqli_num_rows($result) == 0) {
    echo "Utilisateur introuvable.";
    exit();
}

$userData = mysqli_fetch_assoc($result);
$idClient = $userData['IdClient'];

$updateUserQuery = "UPDATE Utilisateur SET IdClient = NULL WHERE NomUtilisateur = '$nomUtilisateur'";
mysqli_query($con, $updateUserQuery) or die("Erreur de mise à jour de l'utilisateur");

$deleteClientQuery = "DELETE FROM Client WHERE IdClient = $idClient";
mysqli_query($con, $deleteClientQuery) or die("Erreur lors de la suppression du client");

echo "0";

mysqli_close($con);

?>