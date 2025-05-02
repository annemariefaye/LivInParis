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

$sql = "SELECT Nom FROM Recette";
$result = mysqli_query($con, $sql);

$recettes = "0\t";

if ($result->num_rows > 0) {
    while($row = $result->fetch_assoc()) {
        $recettes .= $row['Nom'] . "\t";
    }
} 
else {
    echo "Aucune recette trouvée.";
    exit();
}

echo $recettes;

?>
