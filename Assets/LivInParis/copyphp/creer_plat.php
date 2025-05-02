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

function convertToDate($dateInput) {
    $dateArray = explode('/', $dateInput);
    if (count($dateArray) == 3) {
        return $dateArray[2] . '-' . $dateArray[1] . '-' . $dateArray[0]; 
    }
    return $dateInput;  
}

$nom = mysqli_real_escape_string($con, $_POST['nom']);
$prix = mysqli_real_escape_string($con, $_POST['prix']);
$type = mysqli_real_escape_string($con, $_POST['type']);
$personnes = mysqli_real_escape_string($con, $_POST['personnes']);
$dateFabrication = convertToDate(mysqli_real_escape_string($con, $_POST['dateFabrication']));
$datePeremption = convertToDate(mysqli_real_escape_string($con, $_POST['datePeremption']));
$nationalite = mysqli_real_escape_string($con, $_POST['nationalite']);
$proteines = mysqli_real_escape_string($con, $_POST['proteines']);
$chemin = mysqli_real_escape_string($con, $_POST['chemin']);
$nomutilisateur = mysqli_real_escape_string($con, $_POST['nomutilisateur']);
$recette = mysqli_real_escape_string($con, $_POST['recette']);
$regime = mysqli_real_escape_string($con, $_POST['regime']);

$checkPlatQuery = "SELECT * FROM Plat WHERE Nom = '$nom'";
$checkResult = mysqli_query($con, $checkPlatQuery);

if ($checkResult && mysqli_num_rows($checkResult) > 0) {
    echo "Erreur : Un plat avec ce nom existe déjà.";
    exit();
}

$idRecetteQuery = "SELECT IdRecette FROM Recette WHERE Nom = '$recette'";
$resultRecette = mysqli_query($con, $idRecetteQuery);

if (!$resultRecette || mysqli_num_rows($resultRecette) == 0) {
    echo "Recette introuvable. Veuillez vérifier le nom de la recette.";
    exit();
}

$rowRecette = mysqli_fetch_assoc($resultRecette);
$idRecette = $rowRecette['IdRecette'];

$idCuisinierQuery = "SELECT Id FROM Utilisateur WHERE NomUtilisateur = '$nomutilisateur'";

$result = mysqli_query($con, $idCuisinierQuery);

if (!$result) {
    echo "Erreur lors de l'exécution de la requête : " . mysqli_error($con);
    exit();
}

if (mysqli_num_rows($result) == 0) {
    echo "Aucun résultat trouvé pour cet utilisateur.";
    exit();
}

$row = mysqli_fetch_assoc($result);

if (isset($row['Id'])) {
    $idCuisinier = $row['Id']; 
} else {
    echo "Erreur : L'IdCuisinier n'a pas été trouvé dans les résultats.";
    exit();
}

$insertquery = "INSERT INTO Plat (Nom, Prix, IdCuisinier, Type, Personnes, DateFabrication, DatePeremption, Regime, IdRecette, CheminAccesPhoto, Nationalite, Proteines)
VALUES ('$nom', '$prix', '$idCuisinier', '$type', '$personnes', '$dateFabrication', '$datePeremption', '$regime', '$idRecette', '$chemin', '$nationalite', '$proteines');";

if (mysqli_query($con, $insertquery)) {
    echo "0";
} else {
    echo "Erreur dans la création du plat : " . mysqli_error($con);
}

mysqli_close($con);

?>
