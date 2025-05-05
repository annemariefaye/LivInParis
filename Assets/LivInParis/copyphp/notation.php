<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

$idCuisinierUtilisateur = $_POST['idCuisinier'];
$note = $_POST['note'];
$commentaire = $_POST['commentaire'];

$idCuisinierUtilisateur = mysqli_real_escape_string($con, $idCuisinierUtilisateur);
$note = mysqli_real_escape_string($con, $note);
$commentaire = mysqli_real_escape_string($con, $commentaire);

$idcuisiniercheckquerry = "SELECT IdCuisinier FROM Utilisateur WHERE Id = '$idCuisinierUtilisateur';";
$resultCheck = mysqli_query($con, $idcuisiniercheckquerry);

if (!$resultCheck || mysqli_num_rows($resultCheck) != 1) {
    echo "5 : L'utilisateur n'existe pas ou n'est pas cuisinier.";
    exit();
}

$row = mysqli_fetch_assoc($resultCheck);
$idCuisinier = $_POST['idCuisinier'];

$querry = "INSERT INTO NotationCuisinier (IdCuisinier, Note, Commentaire) VALUES ('$idCuisinier', '$note', '$commentaire');";
$result = mysqli_query($con, $querry);

if (!$result) {
    echo "Erreur requête : " . mysqli_error($con);
    exit();
} else {
    echo "0";
}

mysqli_close($con);
?>
