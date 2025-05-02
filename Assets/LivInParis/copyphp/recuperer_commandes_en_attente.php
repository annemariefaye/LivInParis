<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

if (mysqli_connect_errno()) {
    echo "1 : Echec de la connexion à MySQL"; 
    exit();
}

$nomutilisateur = $_POST['nomutilisateur'];

$nomutilisateurcheckquerry = "SELECT Id FROM Utilisateur WHERE NomUtilisateur = '$nomutilisateur';";
$nomutilisateurcheck = mysqli_query($con, $nomutilisateurcheckquerry);

if (!$nomutilisateurcheck || mysqli_num_rows($nomutilisateurcheck) != 1) {
    echo "5 : L'utilisateur n'existe pas.";
    exit();
}

$utilisateur = mysqli_fetch_assoc($nomutilisateurcheck);
$idClient = $utilisateur['Id'];

$query = "
    SELECT c.IdCommande, p.IdPlat, p.Nom, p.Prix, p.CheminAccesPhoto, lc.Quantite, lc.DateLivraison, lc.LieuLivraison
    FROM Commande c
    JOIN LigneDeCommande lc ON c.IdCommande = lc.IdCommande
    JOIN Plat p ON lc.IdPlat = p.IdPlat
    WHERE c.IdClient = '$idClient' AND c.Statut = 'En attente';
";

$result = mysqli_query($con, $query);

if (!$result) {
    echo "Erreur lors de la récupération des commandes en attente.";
    exit();
}

$plats = array();

for ($i = 0; $row = mysqli_fetch_assoc($result); $i++) {

    $dateLivraison = date('d/m/Y', strtotime($row['DateLivraison']));

    $plats[] = array(
        'IdCommande' => $row['IdCommande'],
        'IdPlat' => $row['IdPlat'],
        'Nom' => $row['Nom'],
        'Prix' => $row['Prix'], 
        'CheminAccesPhoto' => $row['CheminAccesPhoto'],
        'Quantite' => $row['Quantite'],
        'DateLivraison' => $dateLivraison, 
        'AdresseLivraison' => $row['LieuLivraison']
    );
}

echo "0\t" . json_encode($plats);

?>
