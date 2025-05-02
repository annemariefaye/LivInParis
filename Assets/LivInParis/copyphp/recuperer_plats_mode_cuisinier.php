<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

$nomUtilisateur = $_POST['NomUtilisateur'] ?? '';

if (empty($nomUtilisateur)) {
    echo "1\tNomUtilisateur manquant.";
    exit();
}

$nomUtilisateur = mysqli_real_escape_string($con, $nomUtilisateur);

$query = "
    SELECT 
        Plat.IdPlat,
        Plat.Nom,
        Plat.Prix,
        Utilisateur.Nom AS NomCuisinier,
        Plat.CheminAccesPhoto,
        Plat.Regime,
        Utilisateur.Adresse,
        Plat.Type,
        Plat.Nationalite,
        Plat.Proteines,
        GROUP_CONCAT(DISTINCT CategorieAmbiance.Nom) AS Categories,
        ROUND(AVG(NotationCuisinier.Note), 2) AS NoteMoyenne
    FROM Plat
    INNER JOIN Utilisateur ON Plat.IdCuisinier = Utilisateur.Id
    LEFT JOIN PlatCategorieAmbiance ON Plat.IdPlat = PlatCategorieAmbiance.IdPlat
    LEFT JOIN CategorieAmbiance ON PlatCategorieAmbiance.IdCategorie = CategorieAmbiance.IdCategorie
    LEFT JOIN NotationCuisinier ON Plat.IdCuisinier = NotationCuisinier.IdCuisinier
    WHERE Utilisateur.NomUtilisateur = '$nomUtilisateur'
    GROUP BY Plat.IdPlat
";

$result = mysqli_query($con, $query);

if (!$result) {
    echo "1\tErreur requête : " . mysqli_error($con);
    exit();
}

$plats = [];

while ($row = mysqli_fetch_assoc($result)) {
    $plats[] = [
        'IdPlat' => (int)$row['IdPlat'],
        'Nom' => $row['Nom'],
        'Prix' => number_format((float)$row['Prix'], 2, '.', ''),
        'NomCuisinier' => $row['NomCuisinier'],
        'CheminAccesPhoto' => $row['CheminAccesPhoto'],
        'Regime' => $row['Regime'],
        'Adresse' => $row['Adresse'],
        'Type' => $row['Type'],
        'Nationalite' => $row['Nationalite'],
        'Proteines' => $row['Proteines'],
        'Categories' => explode(',', $row['Categories'] ?? ''),
        'NoteMoyenne' => isset($row['NoteMoyenne']) ? (float)$row['NoteMoyenne'] : null
    ];
}

echo "0" . json_encode($plats);

mysqli_close($con);
?>
