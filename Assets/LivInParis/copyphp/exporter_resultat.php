<?php

$con = mysqli_connect("localhost", "root", "root", "livinparis");

if (!$con) {
    die("Erreur de connexion MySQL : " . mysqli_connect_error());
}

$nomFichier = isset($_POST['nomFichier']) ? basename($_POST['nomFichier']) : null;
$requete = isset($_POST['requeteSQL']) ? $_POST['requeteSQL'] : null;

if (empty($nomFichier) || empty($requete)) {
    echo "Paramètres manquants.";
    exit();
}

$result = mysqli_query($con, $requete);

if (!$result) {
    echo "Erreur dans la requête : " . mysqli_error($con);
    mysqli_close($con);
    exit();
}

$xml = new SimpleXMLElement("<$nomFichier/>");

while ($row = mysqli_fetch_assoc($result)) {
    $item = $xml->addChild("row");
    foreach ($row as $cle => $valeur) {
        $item->addChild($cle, htmlspecialchars($valeur));
    }
}

$chemin = "../export/$nomFichier.xml";
$xml->asXML($chemin);

echo "0";

mysqli_free_result($result);
mysqli_close($con);

?>
