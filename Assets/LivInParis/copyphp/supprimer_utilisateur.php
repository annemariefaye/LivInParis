<?php

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

$nomUtilisateur = $_POST['nomutilisateur'];

if (empty($nomUtilisateur)) {
    echo "Le nom d'utilisateur est nécessaire.";
    exit();
}

$deleteUserQuery = "DELETE FROM Utilisateur WHERE NomUtilisateur = '$nomUtilisateur'";
if (mysqli_query($con, $deleteUserQuery)) {
    echo "0";
} else {
    echo "Erreur lors de la suppression de l'utilisateur : " . mysqli_error($con);
}

mysqli_close($con);

?>
