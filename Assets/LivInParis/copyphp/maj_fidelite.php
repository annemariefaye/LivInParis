<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

$nomutilisateur = $_POST['nomutilisateur'];
$fidelite = $_POST['fidelite'];

$query = "UPDATE Utilisateur 
SET PointFidelite = '$fidelite'
WHERE Id = (SELECT Id FROM (SELECT Id FROM Utilisateur WHERE NomUtilisateur = '$nomutilisateur') AS temp);";

if (mysqli_query($con, $query)) {
    echo "0";
} else {
    echo "1";
}
?>
