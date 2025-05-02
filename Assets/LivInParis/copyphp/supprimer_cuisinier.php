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
$idCuisinier = $userData['IdCuisinier'];

$updateUserQuery = "UPDATE Utilisateur SET IdCuisinier = NULL WHERE NomUtilisateur = '$nomUtilisateur'";
mysqli_query($con, $updateUserQuery) or die("Erreur de mise à jour de l'utilisateur");

$deleteCuisinierQuery = "DELETE FROM Cuisinier WHERE IdCuisinier = '$idCuisinier'";
mysqli_query($con, $deleteCuisinierQuery) or die("Erreur lors de la suppression du cuisinier");

echo "0";

mysqli_close($con);

?>
