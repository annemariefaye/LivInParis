<?php

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

$nomUtilisateur = $_POST['nomutilisateur'];
$newNomUtilisateur = $_POST['newNomUtilisateur'];
$newEmail = $_POST['newEmail'];
$nom = $_POST['nom'];
$prenom = $_POST['prenom'];
$adresse = $_POST['adresse'];
$telephone = $_POST['telephone'];
$password = $_POST['mdp'];
$pdj = $_POST['pdj']; 
$nomentreprise = $_POST['nomentreprise']; 

$getUserQuery = "SELECT * FROM Utilisateur WHERE NomUtilisateur = '$nomUtilisateur'";
$result = mysqli_query($con, $getUserQuery);

if (mysqli_num_rows($result) == 0) {
    echo "Erreur : Utilisateur introuvable.";
    exit();
}

$userData = mysqli_fetch_assoc($result);

$idCuisinier = $userData['IdCuisinier'];
$idClient = $userData['IdClient'];

if ($newNomUtilisateur != $nomUtilisateur) {
    $checkUsernameQuery = "SELECT * FROM Utilisateur WHERE NomUtilisateur = '$newNomUtilisateur'";
    $usernameCheck = mysqli_query($con, $checkUsernameQuery);

    if (mysqli_num_rows($usernameCheck) > 0) {
        echo "Erreur : Le nom d'utilisateur existe déjà.";
        exit();
    }
}

if ($newEmail != $userData['Email']) {
    $checkEmailQuery = "SELECT * FROM Utilisateur WHERE Email = '$newEmail'";
    $emailCheck = mysqli_query($con, $checkEmailQuery);

    if (mysqli_num_rows($emailCheck) > 0) {
        echo "Erreur : L'email existe déjà.";
        exit();
    }
}

$salt = "\$5\$rounds=5000\$" . "steamedhams" . $newNomUtilisateur . "\$";
$hash = crypt($password, $salt);

$updateUserQuery = "UPDATE Utilisateur SET NomUtilisateur = '$newNomUtilisateur', Email = '$newEmail', Hashing = '$hash', Salt = '$salt', Nom = '$nom', Prenom = '$prenom', Adresse = '$adresse', Telephone = '$telephone' WHERE NomUtilisateur = '$nomUtilisateur'";
mysqli_query($con, $updateUserQuery) or die("Erreur de mise à jour utilisateur");

if ($pdj != null) {
    $updatePlatDuJourQuery = "UPDATE Cuisinier SET PlatDuJour = '$pdj' WHERE IdCuisinier = '$idCuisinier'";
    mysqli_query($con, $updatePlatDuJourQuery) or die("Erreur de mise à jour du plat du jour");
}

if ($nomentreprise != null) {
    $updateNomEntrepriseQuery = "UPDATE Client SET NomEntreprise = '$nomentreprise' WHERE IdClient = '$idClient'";
    mysqli_query($con, $updateNomEntrepriseQuery) or die("Erreur de mise à jour du nom d'entreprise");
}

echo "0";

?>
