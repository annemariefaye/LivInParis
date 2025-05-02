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

$insertCommande = "INSERT INTO Commande (IdClient, Statut) VALUES ('$idClient', 'En attente')";
mysqli_query($con, $insertCommande) or die("2 : Erreur insertion commande");
$idCommande = mysqli_insert_id($con);

$idPlats = $_POST['idPlat'];
$quantites = $_POST['quantite'];
$datesLivraison = $_POST['dateLivraison'];
$adressesLivraison = $_POST['adresseLivraison'];

for ($index = 0; $index < count($idPlats); $index++) {
    $idPlat = $idPlats[$index];
    $quantite = $quantites[$index];
    $dateLivraison = convertToDate($datesLivraison[$index]);
    $adresseLivraison = $adressesLivraison[$index];

    $insertLigne = "INSERT INTO LigneDeCommande (IdCommande, IdPlat, Quantite, DateLivraison, LieuLivraison) 
                    VALUES ('$idCommande', '$idPlat', '$quantite', '$dateLivraison', '$adresseLivraison')";
    mysqli_query($con, $insertLigne) or die("3 : Erreur insertion ligne de commande à l'index $index");
    $idLigneCommande = mysqli_insert_id($con);

    $getCuisinierQuery = "SELECT IdCuisinier FROM Plat WHERE IdPlat = '$idPlat'";
    $cuisinierResult = mysqli_query($con, $getCuisinierQuery);
    if (!$cuisinierResult || mysqli_num_rows($cuisinierResult) == 0) {
        echo "4 : Plat ou cuisinier introuvable pour le plat $idPlat";
        die();
    }
    $cuisinierRow = mysqli_fetch_assoc($cuisinierResult);
    $idLivreur = $cuisinierRow['IdCuisinier'];

    $insertLivraison = "INSERT INTO Livraison (IdLigneCommande, IdLivreur, IdStationDepart, IdStationArrivee, Statut)
                        VALUES ('$idLigneCommande', '$idLivreur', NULL, NULL, 'En attente')";
    mysqli_query($con, $insertLivraison) or die("5 : Erreur insertion livraison à l'index $index");
}

echo "0";

?>
