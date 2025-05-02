<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

if (mysqli_connect_errno()) {
    echo "Échec de la connexion à la base de données.";
    exit();
}

$idPlat = mysqli_real_escape_string($con, $_POST['idPlat']);

$checkPlatQuery = "SELECT * FROM Plat WHERE IdPlat = '$idPlat'";
$checkResult = mysqli_query($con, $checkPlatQuery);

if (!$checkResult || mysqli_num_rows($checkResult) == 0) {
    echo "Erreur : Plat non trouvé.";
    exit();
}

$deleteQuery = "DELETE FROM Plat WHERE IdPlat = '$idPlat'";

if (mysqli_query($con, $deleteQuery)) {
    echo "0";
} else {
    echo "Erreur dans la suppression du plat : " . mysqli_error($con);
}

mysqli_close($con);

?>
