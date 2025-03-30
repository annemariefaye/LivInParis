# Problème scientifique et informatique

## Description

Ce projet permet de lire des relations à partir d'un fichier, de construire un graphe à partir de ces relations et de visualiser ce graphe à l'aide de Windows Forms. Il comprend également des tests unitaires pour vérifier le bon fonctionnement des classes principales.

## Structure du Projet

Le projet est composé des classes suivantes :

### 1. `LectureFichiers`

Cette classe est responsable de la lecture des relations à partir d'un fichier.

- **Attributs :**
  - `contenu`: Liste des relations (sous forme de tableaux d'entiers).
  
- **Constructeur :**
  - `LectureFichiers(string path)`: Lit le contenu d'un fichier spécifié par le chemin et initialise `contenu`.

- **Méthodes :**
  - `List<int[]> Contenu`: Propriété qui retourne le contenu du fichier.
  - `void AfficherContenu()`: Affiche le contenu du fichier dans la console.

### 2. `Graphe`

Cette classe représente un graphe en utilisant une matrice d'adjacence.

- **Attributs :**
  - `Noeuds`: Dictionnaire qui contient les nœuds et leurs relations.

- **Méthodes :**
  - `void AjouterRelation(int a, int b)`: Ajoute une relation entre deux nœuds.
  - `int[,] MatriceAdjacence()`: Retourne la matrice d'adjacence du graphe.
  - `Dictionary<int, List<int>> ListeAdjacence()`: Retourne la liste d'adjacence du graphe.

### 3. `Noeud`

Cette classe représente un nœud du graphe.

- **Attributs :**
  - `int Id`: Identifiant du nœud.
  - `List<Lien> Liens`: Liste des liens associés au nœud.

### 4. `Lien`

Cette classe représente un lien entre deux nœuds.

- **Attributs :**
  - `Noeud Noeud1`: Premier nœud du lien.
  - `Noeud Noeud2`: Deuxième nœud du lien.

### 5. `RechercheChemin`

Cette classe est utilisée pour rechercher des chemins dans le graphe.

- **Méthodes :**
  - `bool ContientCycle(int[,] matrice)`: Vérifie si le graphe contient un cycle.

### 6. `Visualisation`

Cette classe est une fenêtre de formulaire qui visualise le graphe à partir de la matrice d'adjacence.

- **Attributs :**
  - `adjacencyMatrix`: Matrice d'adjacence du graphe.
  - `nodes`: Tableau des nœuds du graphe.

- **Constructeur :**
  - `Visualisation()`: Charge les relations à partir d'un fichier et initialise l'affichage.

- **Méthodes :**
  - `void DrawGraphLines(object sender, PaintEventArgs e)`: Dessine les arêtes et les nœuds du graphe en utilisant des lignes droites.
  - `void DrawGraphCircle(object sender, PaintEventArgs e)`: Dessine le graphe sous forme de cercle.
  - `void DrawGraphWeighted(object sender, PaintEventArgs e)`: Dessine le graphe avec des arêtes pondérées.
  - `void DrawGraphWeightedCurved(object sender, PaintEventArgs e)`: Dessine le graphe avec des arêtes courbées.
  - `void DrawGraphOptimizedOld(object sender, PaintEventArgs e)`: Ancienne méthode de dessin optimisée.
  - `void DrawGraphOptimized(object sender, PaintEventArgs e)`: Méthode de dessin optimisée.
  - `PointF[] ForceDirectedLayout()`: Calcule les positions des nœuds en utilisant une disposition dirigée par la force.
  - `Color GetColorFromDegrees(int degreeA, int degreeB, int maxDegree)`: Retourne une couleur basée sur les degrés des nœuds.
  - `Color GetColorFromDegree(int degree, int maxDegree)`: Retourne une couleur basée sur un degré unique.
  - `PointF[] ForceDirectedLayoutMinCrossings()`: Calcule les positions des nœuds en minimisant les croisements.
  - `bool EdgesCross(PointF a, PointF b, PointF c, PointF d)`: Vérifie si deux arêtes se croisent.
  - `PointF GetCurvedControlPoint(PointF p1, PointF p2)`: Retourne le point de contrôle pour dessiner une arête courbée.

## Installation

1. Clonez le dépôt sur votre machine locale :
   ```bash
   git clone https://github.com/annemariefaye/PbSI.git

## Utilisation de l'IA Générative

Pour créer la classe `Visualisation`, des prompts ont été utilisés avec une IA générative afin d'optimiser la conception et l'affichage des graphes :

- Créer une visualisation de graphe en C# en utilisant un système de dessin basé sur une matrice d'adjacence.
  
- Explorer des solutions pour regrouper les nœuds de manière intelligente afin d'améliorer la lisibilité de la visualisation.
  
- Envisager des courbures pour les lignes afin d'augmenter la clarté visuelle entre les nœuds.
  
- S'inspirer de Graphviz pour le style et la disposition des éléments, sans utiliser la bibliothèque elle-même.
  
- Minimiser les croisements de lignes pour une meilleure présentation graphique.
  
- Remplacer certaines lignes droites par des courbes pour améliorer l'esthétique du graphe.
  
- Pour les nœuds avec le plus de connexions, appliquer un dégradé de couleur allant du rouge au vert, en passant par le jaune, et ajuster la taille des nœuds en fonction du nombre de connexions.

Ces prompts ont été essentiels pour orienter le développement de la visualisation et assurer une expérience utilisateur fluide et agréable.

## Tests Unitaires

### LienTest.cs

Ce fichier contient des tests unitaires pour la classe `Lien`. Les tests incluent :

1. **TestConstructor** : Vérifie que le constructeur initialise correctement les propriétés `Source`, `Destination` et `Poids`.
2. **TestPoidsProperty** : Vérifie la propriété `Poids`.
3. **TestSourceProperty** : Vérifie la propriété `Source`.
4. **TestDestinationProperty** : Vérifie la propriété `Destination`.
5. **TestToString** : Vérifie la méthode `ToString` pour s'assurer qu'elle retourne la chaîne attendue.

### GrapheTest.cs

Ce fichier contient des tests unitaires pour la classe `Graphe`. Les tests incluent :

1. **TestDefaultConstructor** : Vérifie que le constructeur par défaut initialise correctement les propriétés.
2. **TestConstructorWithAdjacencyMatrix** : Vérifie le constructeur avec une matrice d'adjacence pour s'assurer qu'il initialise les noeuds correctement.
3. **TestConstructorWithAdjacencyList** : Vérifie le constructeur avec une liste d'adjacence pour s'assurer qu'il initialise les noeuds correctement.
4. **TestAjouterMembre** : Vérifie l'ajout d'un membre dans le graphe.
5. **TestAjouterRelation** : Vérifie l'ajout d'une relation entre deux noeuds.
6. **TestGetMatriceAdjacence** : Vérifie la récupération de la matrice d'adjacence.
7. **TestGetListeAdjacence** : Vérifie la récupération de la liste d'adjacence.
8. **TestUpdateProprietes** : Vérifie la mise à jour des propriétés du graphe.

### NoeudTest.cs

Ce fichier contient des tests unitaires pour la classe `Noeud`. Les tests incluent :

1. **TestConstructor** : Vérifie que le constructeur initialise correctement l'identifiant et la liste des voisins.
2. **TestAjouterVoisin** : Vérifie l'ajout d'un voisin dans la liste des voisins.
3. **TestToString** : Vérifie la méthode `ToString` pour s'assurer qu'elle retourne la chaîne attendue.
