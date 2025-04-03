# Projet de Réseau de Métro de Paris

## Problème scientifique et informatique

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

### 7. `Chronometreur`

Classe pour mesurer le temps d'exécution des algorithmes de recherche de chemin.

## Comparaison des Algorithmes de Plus Court Chemin

### Introduction

Ce projet explore et compare trois algorithmes classiques de recherche du plus court chemin appliqués au réseau de stations de métro de Paris :
- **Dijkstra**
- **Bellman-Ford**
- **Floyd-Warshall**

### Résultats des Tests

Les tests ont été réalisés sur un réseau de métro simulé, et les temps d'exécution ont été mesurés :

| Algorithme       | Temps d'exécution |
|------------------|------------------|
| Dijkstra         | 3 ms             |
| Bellman-Ford     | 5 ms             |
| Floyd-Warshall   | 30753 ms         |

### Conclusion

Pour notre solution, **l'algorithme de Dijkstra est le meilleur choix** car il offre un compromis optimal entre rapidité et efficacité. Bellman-Ford peut être utile dans certains cas si des retards doivent être gérés, mais son exécution est plus lente. Floyd-Warshall, bien que complet, est trop inefficace pour un grand réseau.

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
```
