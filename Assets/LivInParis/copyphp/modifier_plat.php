<?php

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

function convertToDate($dateInput) {
    $dateArray = explode('/', $dateInput);
    if (count($dateArray) == 3) {
        return $dateArray[2] . '-' . $dateArray[1] . '-' . $dateArray[0];
    }
    return $dateInput;
}

if ($_SERVER['REQUEST_METHOD'] == 'POST') {
    $idPlat = mysqli_real_escape_string($con, $_POST['idPlat']);
    $nom = mysqli_real_escape_string($con, $_POST['nom']);
    $prix = mysqli_real_escape_string($con, $_POST['prix']);
    $type = mysqli_real_escape_string($con, $_POST['type']);
    $personnes = mysqli_real_escape_string($con, $_POST['personnes']);
    $dateFabrication = convertToDate(mysqli_real_escape_string($con, $_POST['dateFabrication']));
    $datePeremption = convertToDate(mysqli_real_escape_string($con, $_POST['datePeremption']));
    $nationalite = mysqli_real_escape_string($con, $_POST['nationalite']);
    $proteines = mysqli_real_escape_string($con, $_POST['proteines']);
    $chemin = mysqli_real_escape_string($con, $_POST['chemin']);
    $nomRecette = mysqli_real_escape_string($con, $_POST['recette']);
    $regime = mysqli_real_escape_string($con, $_POST['regime']);

    $queryRecette = "SELECT IdRecette FROM Recette WHERE Nom = '$nomRecette' LIMIT 1";
    $resultRecette = mysqli_query($con, $queryRecette);

    if ($resultRecette && mysqli_num_rows($resultRecette) > 0) {
        $row = mysqli_fetch_assoc($resultRecette);
        $idRecette = $row['IdRecette'];
    } else {
        echo "Erreur : Recette '$nomRecette' introuvable.";
        exit();
    }

    $updateQuery = "UPDATE Plat 
                    SET Nom = '$nom', Prix = '$prix', Type = '$type', Personnes = '$personnes', 
                        DateFabrication = '$dateFabrication', DatePeremption = '$datePeremption', 
                        Regime = '$regime', IdRecette = '$idRecette', CheminAccesPhoto = '$chemin', 
                        Nationalite = '$nationalite', Proteines = '$proteines'
                    WHERE IdPlat = '$idPlat'";

    if (mysqli_query($con, $updateQuery)) {
        echo "0"; 
    } else {
        echo "Erreur lors de la mise à jour du plat : " . mysqli_error($con);
    }
}

mysqli_close($con);
?>
