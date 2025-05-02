<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}


if(mysqli_connect_errno()) {
    echo "1 : Echec de la connexion à MySQL"; ///Echec de la connexion à MySQL
    exit();
}

$nomutilisateur = $_POST['nomutilisateur'];
$password = $_POST['mdp'];
$email = $_POST['email'];
$nom = $_POST['nom'];
$prenom = $_POST['prenom'];
$adresse = $_POST['adresse'];
$telephone = $_POST['telephone'];

$nomutilisateurcheckquerry = "SELECT * FROM Utilisateur WHERE NomUtilisateur = '$nomutilisateur';";
$nomutilisateurcheck = mysqli_query($con, $nomutilisateurcheckquerry) or die("2 : Echec de la requête nom utilisateur"); ///Echec de la requête username;

if (mysqli_num_rows($nomutilisateurcheck) > 0) {
    echo "Le nom d'utilisateur existe déjà.";
    exit();
}

$emailcheckquerry = "SELECT * FROM Utilisateur WHERE email = '$email'";
$emailcheck = mysqli_query($con, $emailcheckquerry) or die("3 : Echec de la requete email"); ///Echec de la requete email;

if (mysqli_num_rows($emailcheck) > 0) {
    echo "L'email existe déjà.";
    exit();
}

$salt = "\$5\$rounds=5000\$" . "steamedhams" . $nomutilisateur . "\$";
$hash = crypt($password, $salt);

/// Inserer les données dans la base de données
$insertquery = "INSERT INTO Utilisateur (NomUtilisateur, Hashing, Salt , Nom, Prenom, Adresse, Telephone, Email) 
VALUES  ('".$nomutilisateur."', '".$hash."', '".$salt."', '".$nom."', '".$prenom."', '".$adresse."', '".$telephone."', '".$email."');";

mysqli_query($con, $insertquery) or die("4 : Echec de la requête d'insertion"); ///Echec de la requête d'insertion
echo "0"; //Inscription réussie !

?>