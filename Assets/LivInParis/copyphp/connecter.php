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

$nomutilisateur = $_POST['nomutilisateur'];
$password = $_POST['mdp'];

$nomutilisateurcheckquerry = "SELECT NomUtilisateur, Salt, Hashing, Email, Nom, Prenom, Adresse, Telephone, PointFidelite, IdClient, IdCuisinier FROM Utilisateur WHERE NomUtilisateur = '$nomutilisateur';";
$nomutilisateurcheck = mysqli_query($con, $nomutilisateurcheckquerry) or die("2 : Echec de la requête nom utilisateur");

if (mysqli_num_rows($nomutilisateurcheck) != 1) {
    echo "5 : Soit le nom d'utilisateur n'existe pas, soit il existe plusieurs fois.";
    exit();
}

$existinginfo = mysqli_fetch_assoc($nomutilisateurcheck);

$salt = $existinginfo['Salt'];
$hash = $existinginfo['Hashing'];

$loginhash = crypt($password, $salt);
if ($loginhash != $hash) {
    echo "6 : Le mot de passe est incorrect.";
    exit();
}

$idClient = $existinginfo['IdClient'] !== null ? $existinginfo['IdClient'] : "null";
$idCuisinier = $existinginfo['IdCuisinier'] !== null ? $existinginfo['IdCuisinier'] : "null";

$clientQuery = "SELECT nomEntreprise FROM client WHERE idClient = '$idClient';";
$clientResult = mysqli_query($con, $clientQuery);
$clientInfo = mysqli_fetch_assoc($clientResult);
$nomEntreprise = $clientInfo['nomEntreprise'] ?? "null";

$cuisinierQuery = "SELECT platDuJour FROM cuisinier WHERE idCuisinier = '$idCuisinier';";
$cuisinierResult = mysqli_query($con, $cuisinierQuery);
$cuisinierInfo = mysqli_fetch_assoc($cuisinierResult);
$platDuJour = $cuisinierInfo['platDuJour'] ?? "null";

echo "0\t" . $existinginfo['Email'] . "\t" . $existinginfo['Nom'] . "\t" . $existinginfo['Prenom'] . "\t" . $existinginfo['Adresse'] . "\t" . $existinginfo['Telephone'] . "\t" . $existinginfo['PointFidelite'] . "\t" . $platDuJour . "\t" . $nomEntreprise . "\t" . $idClient . "\t" . $idCuisinier . "\t";

?>
