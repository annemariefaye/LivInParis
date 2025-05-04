<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

if (isset($_POST['requete'])) {
    $requete = $_POST['requete'];
    $result = mysqli_query($con, $requete);

    if ($result) {
        if (stripos($requete, 'select') === 0) {
            $rows = [];
            while ($row = mysqli_fetch_assoc($result)) {
                $rows[] = $row;
            }
            echo "0" . json_encode($rows);
        } 
        else 
        {
            echo "Erreur : Requête non valide.";
        }
    } 
    else 
    {
        echo "Erreur lors de l'exécution : " . mysqli_error($con);
    }
} 
else 
{
    echo "Aucune requête reçue.";
}

mysqli_close($con);
?>
