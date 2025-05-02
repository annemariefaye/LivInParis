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

$dateDebut = isset($_POST['dateDebut']) ? convertToDate($_POST['dateDebut']) : '';
$dateFin = isset($_POST['dateFin']) ? convertToDate($_POST['dateFin']) : '';

$query = "
    SELECT 
    p.Nom AS TitrePlat,
    c.DateCommande,
    u.Nom,
    u.Prenom,
    c.Statut,
    AVG(p.Prix) AS PrixMoyen
    FROM Commande c
    JOIN LigneDeCommande lc ON lc.IdCommande = c.IdCommande
    JOIN Plat p ON p.IdPlat = lc.IdPlat
    JOIN Utilisateur u ON p.IdCuisinier = u.Id
    WHERE c.DateCommande BETWEEN '$dateDebut' AND '$dateFin'
    GROUP BY c.IdCommande, p.Nom, c.DateCommande, u.Nom, u.Prenom, c.Statut
";

$result = mysqli_query($con, $query);

while ($row = mysqli_fetch_assoc($result)) {
    echo $row['TitrePlat'] . "\t" 
        . $row['DateCommande'] . "\t" 
        . $row['Nom'] . "\t" 
        . $row['Prenom'] . "\t" 
        . $row['Statut'] . "\t" 
        . number_format($row['PrixMoyen'], 2) . "\n";
}
mysqli_close($con);
?>
