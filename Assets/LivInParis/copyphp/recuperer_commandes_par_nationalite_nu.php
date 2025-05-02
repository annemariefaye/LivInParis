<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");
if (!$con) {
    die("Erreur de connexion : " . mysqli_connect_error());
}

function convertToDate($dateInput) {
    $dateArray = explode('/', $dateInput);
    if (count($dateArray) == 3) {
        return $dateArray[2] . '-' . $dateArray[1] . '-' . $dateArray[0]; 
    }
    return $dateInput;  
}

if (!isset($_POST['dateDebut'], $_POST['dateFin'], $_POST['nationalite'], $_POST['nomutilisateur'])) {
    die("Paramètres manquants.");
}

$dateDebut = convertToDate($_POST['dateDebut']);
$dateFin = convertToDate($_POST['dateFin']);
$nationalite = mysqli_real_escape_string($con, $_POST['nationalite']);
$usernameClient = mysqli_real_escape_string($con, $_POST['nomutilisateur']);

$query = "SELECT p.Nom AS TitrePlat, p.Nationalite, c.DateCommande, cuisinier.Nom AS NomCuisinier, cuisinier.Prenom AS PrenomCuisinier, c.Statut
FROM Commande c
JOIN Utilisateur client ON client.Id = c.IdClient
JOIN LigneDeCommande lc ON lc.IdCommande = c.IdCommande
JOIN Plat p ON p.IdPlat = lc.IdPlat
JOIN Utilisateur cuisinier ON p.IdCuisinier = cuisinier.Id
WHERE client.NomUtilisateur = '$usernameClient'
AND p.Nationalite = '$nationalite'
AND c.DateCommande BETWEEN '$dateDebut' AND '$dateFin'
GROUP BY c.IdCommande, p.Nom, p.Nationalite, c.DateCommande, cuisinier.Nom, cuisinier.Prenom, c.Statut";

$result = mysqli_query($con, $query) or die("Erreur SQL : " . mysqli_error($con) . "\nRequête :\n" . $query);

while ($row = mysqli_fetch_assoc($result)) {
    echo $row['TitrePlat'] . "\t" 
        . $row['DateCommande'] . "\t" 
        . $row['NomCuisinier'] . "\t" 
        . $row['PrenomCuisinier'] . "\t" 
        . $row['Statut'] . "\n";
}

mysqli_close($con);
?>
