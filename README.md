# Problème scientifique et informatique

### Description

Ce projet permet de lire des relations à partir d'un fichier, de construire un graphe à partir de ces relations et de visualiser ce graphe à l'aide de Windows Forms. Il comprend également des tests unitaires pour vérifier le bon fonctionnement des classes principales.

## Structure du Projet

Le projet est composé des classes suivantes :

### 1. `StationMetro`

Représente une station de métro avec les attributs suivants :
- **Attributs :**
  - `libelle`: Libellé de la station.
  - `ligne`: Numéro de la ligne.
  - `longitude`: Longitude de la station.
  - `latitude`: Latitude de la station.
  - `commune`: Nom de la commune.
  - `codeInsee`: Code INSEE de la commune.
  
- **Méthodes principales :**
  - `StationMetro(string ligne, string libelle, double longitude, double latitude, string commune, int codeInsee)`: Constructeur.
  - `static StationMetro Parse(List<string> data)`: Parse les données pour créer une instance de `StationMetro`.

### 2. `ReseauMetro`

Gère le réseau de métro en lisant les données depuis un fichier Excel et en construisant le graphe de stations.
- **Méthodes principales :**
  - `Graphe<StationMetro> Graphe`: Propriété qui retourne le graphe construit.
  - `private void CreerStations(List<List<string>> donneesNoeuds)`: Crée les stations à partir des données lues.
  - `private void CreerRelations(List<List<string>> donneesArcs)`: Crée les relations entre les stations.

### 3. `Graphe<T>`

Représente un graphe générique avec des nœuds et des liens.
- **Attributs :**
  - `noeuds`: Liste des nœuds du graphe.
  - `liens`: Liste des liens entre les nœuds.
  
- **Méthodes principales :**
  - `void AjouterMembre(Noeud<T> noeud)`: Ajoute un nœud au graphe.
  - `void AjouterRelation(Noeud<T> source, Noeud<T> destination, double poids)`: Ajoute une relation entre deux nœuds.
  - `double[,] MatriceAdjacence`: Propriété qui retourne la matrice d'adjacence du graphe.

### 4. `Noeud<T>`

Représente un nœud du graphe.
- **Attributs :**
  - `id`: Identifiant du nœud.
  - `contenu`: Contenu associé au nœud.

### 5. `Lien<T>`

Représente un lien entre deux nœuds.
- **Attributs :**
  - `source`: Nœud source du lien.
  - `destination`: Nœud destination du lien.
  - `poids`: Poids du lien.

### 6. `RechercheChemin<T>`

Classe qui contient des algorithmes pour la recherche de chemins dans le graphe.
- **Algorithmes disponibles :**
  - `Dijkstra`: Trouve le plus court chemin entre un nœud de départ et tous les autres nœuds.
  - `BellmanFord`: Trouve le chemin le plus court tout en gérant les poids négatifs.
  - `FloydWarshall`: Calcule le plus court chemin entre tous les nœuds.
  - `A*`: Trouve le plus court chemin en utilisant une heuristique pour guider la recherche.

### 7. `Chronometreur`

Classe pour mesurer le temps d'exécution des algorithmes de recherche de chemin.

## Comparaison des Algorithmes de Plus Court Chemin

### Introduction

Ce projet explore et compare trois algorithmes classiques de recherche du plus court chemin appliqués au réseau de stations de métro de Paris :
- **Dijkstra**
- **Bellman-Ford**
- **Floyd-Warshall**
- **A***

### Résultats des Tests

Les tests ont été réalisés sur un réseau de métro simulé, et les temps d'exécution ont été mesurés :

| Algorithme       | Temps d'exécution |
|------------------|------------------|
| Dijkstra         | 3 ms             |
| Bellman-Ford     | 5 ms             |
| Floyd-Warshall   | 30753 ms         |
| A*               | 8 ms             |

### Analyse de la complexité

| Algorithme       | Complexité temporelle | Avantages | Inconvénients |
|------------------|----------------------|-----------|---------------|
| **Dijkstra**    | `O((V + E) log V)` avec un tas de Fibonacci | Optimal pour les graphes pondérés positifs, rapide avec une bonne implémentation | Inefficace pour les très grands graphes avec de nombreux nœuds |
| **Bellman-Ford**| `O(VE)` | Gère les poids négatifs | Plus lent que Dijkstra |
| **Floyd-Warshall** | `O(V³)` | Calcule toutes les distances entre chaque paire de nœuds | Trop inefficace pour les grands graphes |
| **A***          | `O((V + E) log V)` (similaire à Dijkstra) mais souvent plus rapide avec une bonne heuristique | Plus rapide que Dijkstra lorsque l'heuristique est bien choisie | Nécessite une heuristique adaptée pour de bonnes performances |


### Conclusion

Pour notre solution, l'algorithme de Dijkstra est le meilleur choix car il offre un compromis optimal entre rapidité et efficacité. L'algorithme A* est également très performant, surtout lorsqu'il est associé à une heuristique appropriée, permettant des recherches plus ciblées et souvent plus rapides dans des graphes complexes. Bellman-Ford peut être utile dans certains cas si des retards doivent être gérés, mais son exécution est plus lente. Floyd-Warshall, bien que complet, est trop inefficace pour un grand réseau.

## Installation

1. Clonez le dépôt sur votre machine locale :
   ```bash
   git clone https://github.com/annemariefaye/PbSI.git
   ```

## Utilisation

Pour exécuter les algorithmes sur le réseau de métro, utilisez le fichier `Chronometreur.cs` pour mesurer leurs performances. Vous pouvez lancer les tests en exécutant :
```csharp
Chronometreur.ChronometreDijkstra(graphe, depart, arrivee);
Chronometreur.ChronometreBellmanFord(graphe, depart, arrivee);
Chronometreur.ChronometreFloydWarshall(graphe, depart, arrivee);
Chronometreur.ChronometreAStar(graphe, depart, arrivee);
```
## Acceder a la BDD:

Pour acceder a la base de données:
- Utulisateur : root
- Mot de Passe: root
  
## Equipe :

- Anne-Marie Faye  -> pseudo : annemariefaye
- Valentin Fournel -> pseudo : intfly
- Maria Ghoch      -> pseudo : ghochii
  
## Modules Autres:

1- Ajout de catégories (recommandation --> ambiance: brunch, dejeuner de famille, soirée romantique, anniversaire...)
2- Nombre de grammes de protéines par plat
3- Notation des cuisiniers
4- Recommandation d'une musique par nationalité du plat
5- Système de plat gratuit au bout de 100 points (1 point = 1 euro)



# Unity

## Description générale

Nous avons décidés, pour le rendu 3, de réaliser l'interface sur Unity. Ce choix influe sur beaucoup de points de projet, notamment la connexion à la BDD. là où nous utilisions le langage C# pour se connecter à la BDD, nous devons maintenant utiliser du PHP pour afficher nos résultats dans Unity. Nous avons décidé  d'utiliser ce projet pour nous initier à mysqli, une alternative un peu plus complexe à PDO. 

## fichiers PHP backend (Assets\LivInParis\copyphp)


Il s'agit du folder gérant l'insertion, la modification et la suppression des données de la BDD. 
notamment:
- les clients
- les commandes
- les plats
- les points de fidélité
- les statistiques
- les notes des cuisiniers
- les cuisiniers

## fichiers Unity frontend (Assets\LivInParis\Scenes)

Ce dossier regroupe l'entièreté des interfaces utilisateur comme par exemple :
- la connexion
- l'interface client
	- passage de commande
	-  interface de commande
	- visualisation du chemin pris par le livreur
	- modification des données personnelles
- interface cuisinier
	- Création et modification de plat 
	- Validation de commande
- l'interface Admin de contrôle
	- Graphe clients cuisiniers
	- Graphe station de métro (2D et 3D)
	- statistiques
	- Entrée SQL libre
	- Requêtes SQL prédéfinies (modification, contrôle, suppression)
	- Export 

## Images de l'interface utilisateur 

contient l'ensemble des images utilisées pour rendre l'UI agréable à utiliser 

Dans 2 dossiers séparés:
##### Assets\LivInParis\Images
Ensemble des icons et autres interfaces d'UI qui ne sont pas dépendants des produits
##### Assets\LivInParis\Resources
Ensemble des photos produits en lien avec la BDD

## Musiques (Assets\LivInParis\Resources\Musique)

Etant dans le folder Ressources, ces fichiers musicaux dépendent des produits. En commandant les plats de certaines nationalités, certaines musiques prédéfinies peuvent se lancer. Cela permet de rendre l'interface plus intéressante à utiliser tout en rajouter un côté plus fun à l'application.

## Graphe

Afin d'afficher les graphes, nous avons décidé d'utiliser le package Mapbox dans Unity.

Pour vérifier que les adresse mises dans la BDD sont valide et qu'elles pourront être affichés dans la carte, 3D (Graphe 3D), nous utilisons un API d'openStreetMap convertissant les adresses en coordonnées. 


