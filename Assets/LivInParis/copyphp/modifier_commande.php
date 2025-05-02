<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

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

$nomutilisateur = $_POST['nomutilisateur'];
$getUserIdQuery = "SELECT Id FROM Utilisateur WHERE NomUtilisateur = '$nomutilisateur'";
$result = mysqli_query($con, $getUserIdQuery);
if (!$result || mysqli_num_rows($result) == 0) {
    echo "1 : Utilisateur introuvable";
    die();
}
$userRow = mysqli_fetch_assoc($result);
$idClient = $userRow['Id'];

if (isset($_POST['idPlat']) && is_array($_POST['idPlat']) && count($_POST['idPlat']) > 0) {
    $idCommandes= $_POST['idCommande'];
    $idPlats = $_POST['idPlat'];
    $quantites = $_POST['quantite'];
    $datesLivraison = $_POST['dateLivraison'];
    $adressesLivraison = $_POST['adresseLivraison'];

    for ($index = 0; $index < count($idPlats); $index++) {
        $idCommande = $idCommandes[$index];
        $idPlat = $idPlats[$index];
        $quantite = $quantites[$index];
        $dateLivraison = convertToDate($datesLivraison[$index]);
        $adresseLivraison = $adressesLivraison[$index];

        $updateLigne = "UPDATE LigneDeCommande
                        SET 
                            Quantite = '$quantite',
                            LieuLivraison = '$adresseLivraison',
                            DateLivraison = '$dateLivraison'
                        WHERE 
                            IdPlat = '$idPlat' 
                            AND IdCommande = '$idCommande';";
        mysqli_query($con, $updateLigne) or die("3 : Erreur mise à jour ligne de commande à l'index $index");
    }
    echo "0";
} else {
    echo "Erreur : les données de la commande sont manquantes.";
}
?>
