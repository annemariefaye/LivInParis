<?php
error_reporting(E_ALL);
ini_set('display_errors', 1);

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
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
    SELECT 
        lc.LieuLivraison, 
        p.IdCuisinier, 
        u.Adresse AS AdresseCuisinier, 
        m.Titre AS TitreMusique,
        c.IdCommande
    FROM Commande c
    JOIN LigneDeCommande lc ON c.IdCommande = lc.IdCommande
    JOIN Plat p ON lc.IdPlat = p.IdPlat
    JOIN Utilisateur u ON p.IdCuisinier = u.Id
    LEFT JOIN (
        SELECT Titre, Nationalite
        FROM Musique
    ) m ON m.Nationalite = p.Nationalite
    WHERE c.IdClient = '$idClient' AND c.Statut = 'En attente'
    ORDER BY lc.DateLivraison ASC
    LIMIT 1;
";

$result = mysqli_query($con, $query);

if (!$result || mysqli_num_rows($result) == 0) {
    echo "Erreur : Aucune commande en attente trouvée pour cet utilisateur.";
    exit();
}

$row = mysqli_fetch_assoc($result);
$adresseDepart = $row['AdresseCuisinier'];
$adresseArrivee = $row['LieuLivraison'];
$idCuisinier = $row['IdCuisinier'];
$titreMusique = $row['TitreMusique'] ?? 'default';
$idCommande = $row['IdCommande'] ?? '0';

echo "0\t$adresseDepart\t$adresseArrivee\t$idCuisinier\t$titreMusique\t$idCommande";
?>
