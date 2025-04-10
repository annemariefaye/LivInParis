﻿
using Mysqlx.Crud;
using MySqlX.XDevAPI.CRUD;
using System.Diagnostics;

namespace PbSI
{

    public class ResultatChemin
    {

        private readonly double poidsTotal;
        private readonly List<int> chemin;

        

        public ResultatChemin(double poidsTotal, List<int> chemin)
        {
            this.poidsTotal = poidsTotal;
            this.chemin = chemin;
        }

        public double PoidsTotal { get { return this.poidsTotal; } }
        public List<int> Chemin { get { return this.chemin; } }
    }


    public static class RechercheChemin<T> where T : notnull
    {


        #region Parcours

        /// <summary>
        /// Algorithme de BFS pour parcourir un graphe
        /// </summary>
        /// <param name="graph">Graphe sous forme de matrice d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        /// <param name="graphe">Graphe</param>
        public static void BFS_Matrice(Graphe<T> graphe, int depart)
        {
            int nbNodes = graphe.Noeuds.Count;
          
            int[] distances = new int[nbNodes];
            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            dejaExplore[depart] = true;
            distances[depart] = 0;

            Console.WriteLine($"On visite à partir du noeud {depart} :");

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(depart);

            double[,] matriceAdjacente = graphe.MatriceAdjacence;

            while (queue.Count > 0)
            {
                int enCoursIndex = queue.Dequeue();    

                Console.Write($"{enCoursIndex} ");

                for (int i = 0; i < nbNodes; i++)
                {
                    if (matriceAdjacente[enCoursIndex, i] != 0 && !dejaExplore[i])
                    {
                        dejaExplore[i] = true;
                        distances[i] = distances[enCoursIndex] + 1;
                        queue.Enqueue(i);
                    }
                }
            }

            Console.WriteLine("\n");
            AfficherSolutionMatrice(distances);

            bool connexe = dejaExplore.All(x => x);
            Console.WriteLine($"Le graphe est fortement connexe ? : {connexe}");
        }



        /// <summary>
        /// Algorithme de BFS pour parcourir un graphe à partir d'une liste d'adjacence
        /// </summary>
        /// <param name="graph">Graphe sous forme de liste d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        public static void BFS_Liste(Graphe<T> graphe, int depart)
        {
            int nbNodes = graphe.Noeuds.Count;


            int[] distances = new int[nbNodes];
            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue; 
                dejaExplore[i] = false; 
            }

            dejaExplore[depart] = true; 
            distances[depart] = 0; 

            Console.WriteLine($"On visite à partir du noeud {depart} :");

            Queue<int> queue = new Queue<int>();
            queue.Enqueue(depart); 

            double[,] matriceAdjacente = graphe.MatriceAdjacence;

            while (queue.Count > 0)
            {
                int enCoursIndex = queue.Dequeue();  

                Console.Write($"{enCoursIndex} ");  

                for (int i = 0; i < nbNodes; i++)
                {
                    if (matriceAdjacente[enCoursIndex, i] != 0 && !dejaExplore[i])
                    {
                        dejaExplore[i] = true; 
                        distances[i] = distances[enCoursIndex] + 1; 
                        queue.Enqueue(i); 
                    }
                }
            }

            Console.WriteLine("\n");
            AfficherSolutionListe(distances, graphe.ListeAdjacence);

            bool connexe = dejaExplore.All(x => x);
            Console.WriteLine($"Le graphe est fortement connexe ? : {connexe}");
        }

        /// <summary>
        /// Algorithme de DFS pour parcourir un graphe
        /// </summary>
        /// <param name="graph">Graphe sous forme de matrice d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        /// <param name="graphe">Graphe</param>
        public static void DFS_Matrice(Graphe<T> graphe, int depart)
        {
            int nbNodes = graphe.Noeuds.Count;

            int[] distances = new int[nbNodes];
            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            dejaExplore[depart] = true;
            distances[depart] = 0;

            Console.WriteLine("On visite à partir du noeud " + depart + ":");

            Stack<int> stack = new Stack<int>();
            stack.Push(depart);
            double[,] matriceAdjacente = graphe.MatriceAdjacence;

            while (stack.Count > 0)
            {
                int nodeToVisitIndex = stack.Pop();

                Console.Write(nodeToVisitIndex + " ");

                for (int i = 0; i < nbNodes; i++)
                {
                    if (matriceAdjacente[nodeToVisitIndex, i] != 0 && !dejaExplore[i])
                    {
                        dejaExplore[i] = true;
                        distances[i] = distances[nodeToVisitIndex] + 1;
                        stack.Push(i);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            bool connexe = dejaExplore.All(x => x);
            Console.WriteLine($"Le graphe est fortement connexe ? : {connexe}");

            AfficherSolutionMatrice(distances);
        }


        /// <summary>
        /// Algorithme de DFS pour parcourir un graphe à partir d'une liste d'adjacence
        /// </summary>
        /// <param name="graph">Graphe sous forme de matrice d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        public static void DFS_Liste(Graphe<T> graphe, int depart)
        {
            int nbNodes = graphe.Noeuds.Count;

            int[] distances = new int[nbNodes];
            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = int.MaxValue;
                dejaExplore[i] = false;
            }

            dejaExplore[depart] = true;
            distances[depart] = 0;

            Console.WriteLine("On visite à partir du noeud " + depart + ":");

            Stack<int> stack = new Stack<int>();
            stack.Push(depart);

            while (stack.Count > 0)
            {
                int nodeToVisitIndex = stack.Pop();
                Noeud<T> nodeToVisit = graphe.Noeuds[nodeToVisitIndex];
                int nodeToVisitId = nodeToVisit.Id; 

                Console.Write(nodeToVisitId + " ");

                var voisins = graphe.ListeAdjacence[nodeToVisit];

                foreach (var voisin in voisins)
                {
                    int voisinIndex = voisin.Item1.Id;

                    if (!dejaExplore[voisinIndex])
                    {
                        dejaExplore[voisinIndex] = true;
                        distances[voisinIndex] = distances[nodeToVisitIndex] + 1;
                        stack.Push(voisinIndex);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            bool connexe = dejaExplore.All(x => x);
            Console.WriteLine($"Le graphe est fortement connexe ? : {connexe}");

            AfficherSolutionListe(distances, graphe.ListeAdjacence);
        }


        #endregion

        #region Plus court chemin

        /// <summary>
        /// Algorithme de Dijkstra pour trouver le plus court chemin entre un noeud de départ et tous les autres noeuds
        /// </summary>
        /// <param name="graph">Graphe sous forme de matrice d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        public static ResultatChemin Dijkstra(Graphe<StationMetro> graphe, int depart, int arrivee)
        {
            double[,] matriceAdjacence = graphe.MatriceAdjacence;
            int nbNodes = matriceAdjacence.GetLength(0);

            double[] distances = new double[nbNodes];
            bool[] dejaExplore = new bool[nbNodes];
            int[] parents = new int[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = double.MaxValue;
                dejaExplore[i] = false;
                parents[i] = -1;
            }

            int departIndex = depart;
            int arriveeIndex = arrivee;

            distances[departIndex] = 0;

            for (int count = 0; count < nbNodes - 1; count++)
            {
                int indexMinDistance = minimum_distance(distances, dejaExplore, nbNodes);
                dejaExplore[indexMinDistance] = true;

                for (int n = 0; n < nbNodes; n++)
                {
                    if (!dejaExplore[n] && matriceAdjacence[indexMinDistance, n] != 0)
                    {
                        double newDist = distances[indexMinDistance] + matriceAdjacence[indexMinDistance, n];

                        if (newDist < distances[n])
                        {
                            distances[n] = newDist;
                            parents[n] = indexMinDistance;
                        }
                    }
                }
            }

            List<int> chemin = ObtenirChemin(parents, departIndex, arriveeIndex);
            // AfficherChemin(chemin, graphe);

            return new ResultatChemin(distances[arrivee], chemin);
        }

        public static ResultatChemin DijkstraListe(Graphe<StationMetro> graphe, List<int> depart, List<int> arrivee)
        {
            double distanceMin = double.MaxValue;
            List<int> cheminMin = new List<int>();

            foreach (int id in depart)
            {
                foreach (int id2 in arrivee)
                {
                    ResultatChemin resultat = RechercheChemin<StationMetro>.Dijkstra(graphe, id, id2);
                    if (resultat.PoidsTotal < distanceMin)
                    {
                        distanceMin = resultat.PoidsTotal;
                        cheminMin = resultat.Chemin;
                    }
                }

            }

            AfficherChemin(cheminMin, graphe);
            Console.WriteLine($"Poids total du chemin est de : " + distanceMin);
            return new ResultatChemin(distanceMin, cheminMin);

        }



        /// <summary>
        /// Retourne l'indice du noeud non exploré avec la distance minimale
        /// </summary>
        /// <param name="distances">Distances entre les noeuds</param>
        /// <param name="dejaExplore">Tableau de booléens indiquant si un noeud a déjà été exploré</param>
        /// <param name="nbNodes">Nombre de noeuds</param>
        /// <returns>Indice du noeud non exploré avec la distance minimale</returns>
        private static int minimum_distance(double[] distances, bool[] dejaExplore, int nbNodes)
        {
            double min_distance = double.MaxValue;
            int min_index = -1;

            for (int n = 0; n < nbNodes; n++)
            {
                if (!dejaExplore[n] && distances[n] <= min_distance)
                {
                    min_distance = distances[n];
                    min_index = n;
                }
            }

            return min_index;
        }

        ///<summary>
        ///Algorithme de Bellman-Ford pour trouver le chemin le plus court

        public static void BellmanFord(Graphe<T> graphe, int departIndex, int arriveeIndex)
        {
            int nbNodes = graphe.Noeuds.Count;
            double[] distances = new double[nbNodes];
            int[] parents = new int[nbNodes];
            // initialisation
            for (int i = 0; i < nbNodes; i++)
            {
                distances[i] = double.MaxValue;
                parents[i] = -1;
            }
            distances[departIndex] = 0;

            
            for (int i = 0; i < nbNodes - 1; i++)
            {
              
                foreach (var lien in graphe.Liens)
                {
                    int u = lien.Source.Id;
                    int v = lien.Destination.Id;
                    double poids = lien.Poids;

                    if (distances[u] + poids < distances[v])
                    {
                        distances[v] = distances[u] + poids;
                        parents[v] = u;
                    }
                }
            }
            
            /// verification de la presence de cycle negatif
            foreach (var lien in graphe.Liens)
            {

                int u = lien.Source.Id;
                int v = lien.Destination.Id;
                double poids = lien.Poids;

                if (distances[u] + poids < distances[v])
                {
                    Console.WriteLine("Cycle de poids negatif detecte");
                    return;
                }
            }

            
            if (distances[arriveeIndex] == double.MaxValue)
            {
                Console.WriteLine($"Aucun chemin trouvé entre {departIndex} et {arriveeIndex}.");
                return;
            }

            //Console.WriteLine($"Distance minimale entre {departIndex} et {arriveeIndex} : {distances[arriveeIndex]}");
            /*List<int> chemin = new List<int>();
            for (int v = arriveeIndex; v != -1; v = parents[v])
            {
                chemin.Add(v);
            }
            chemin.Reverse();
            Console.WriteLine("Chemin le plus court : " + string.Join("->", chemin));*/



        }

        ///<summary>
        ///algorithme de Floyd Warshall
        ///</summary>

        public static void FloydWarshall(Graphe<T> graphe, int departIndex, int arriveeIndex)
        {
            int n = graphe.Noeuds.Count;

            double[,] distances = new double[n, n];
            int?[,] predecesseurs = new int?[n, n];

            // Initialisation
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j)
                    {
                        distances[i, j] = 0;
                    }
                    else if (graphe.MatriceAdjacence[i, j] > 0)
                    {
                        distances[i, j] = graphe.MatriceAdjacence[i, j];
                        predecesseurs[i, j] = i;
                    }
                    else
                    {
                        distances[i, j] = double.MaxValue;
                        predecesseurs[i, j] = null;
                    }
                }
            }

            // Floyd-Warshall
            for (int k = 0; k < n; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (distances[i, k] != double.MaxValue &&
                            distances[k, j] != double.MaxValue &&
                            distances[i, k] + distances[k, j] < distances[i, j])
                        {
                            distances[i, j] = distances[i, k] + distances[k, j];
                            predecesseurs[i, j] = predecesseurs[k, j];
                        }
                    }
                }
            }

            // Affichage
            /*if (distances[departIndex, arriveeIndex] == double.MaxValue)
            {
                Console.WriteLine($"Aucun chemin trouvé entre {departIndex} et {arriveeIndex}.");
                return;
            }

            Console.WriteLine($"Distance minimale entre {departIndex} et {arriveeIndex} : {distances[departIndex, arriveeIndex]}");

            // Reconstruction du chemin
            List<int> chemin = new List<int>();
            int? courant = arriveeIndex;
            while (courant != null)
            {
                chemin.Add(courant.Value);
                if (courant == departIndex)
                    break;
                courant = predecesseurs[departIndex, courant.Value];
            }

            chemin.Reverse();

            Console.WriteLine("Chemin le plus court : " + string.Join(" -> ", chemin));*/
        }



        public static ResultatChemin AEtoile(Graphe<StationMetro> graphe, int depart, int arrivee)
        {
            var listeAdjacence = graphe.ListeAdjacence;
            int nbNodes = graphe.Noeuds.Count;

            double[] gScore = new double[nbNodes];
            double[] fScore = new double[nbNodes];
            int[] parents = new int[nbNodes];
            bool[] dejaExplore = new bool[nbNodes];

            for (int i = 0; i < nbNodes; i++)
            {
                gScore[i] = double.MaxValue;
                fScore[i] = double.MaxValue;
                parents[i] = -1;
            }

            gScore[depart] = 0;
            fScore[depart] = Heuristique(graphe, depart, arrivee);

            PriorityQueue<int, double> openSet = new PriorityQueue<int, double>();
            openSet.Enqueue(depart, fScore[depart]);

            Dictionary<int, double> openSetEntries = new Dictionary<int, double>();
            openSetEntries[depart] = fScore[depart];

            while (openSet.Count > 0)
            {
                int current = openSet.Dequeue();
                openSetEntries.Remove(current);

                if (current == arrivee)
                    break;

                dejaExplore[current] = true;

                if (!listeAdjacence.ContainsKey(graphe.TrouverNoeudParId(current)))
                    continue;

                foreach (var (voisin, poids) in listeAdjacence[graphe.TrouverNoeudParId(current)])
                {
                    int voisinId = voisin.Id;
                    if (dejaExplore[voisinId]) continue;

                    double tentativeGScore = gScore[current] + poids;

                    if (tentativeGScore < gScore[voisinId])
                    {
                        parents[voisinId] = current;
                        gScore[voisinId] = tentativeGScore;
                        fScore[voisinId] = gScore[voisinId] + Heuristique(graphe, voisinId, arrivee);

                        if (!openSetEntries.ContainsKey(voisinId) || fScore[voisinId] < openSetEntries[voisinId])
                        {
                            openSetEntries[voisinId] = fScore[voisinId];
                            openSet.Enqueue(voisinId, fScore[voisinId]);
                        }
                    }
                }
            }

            List<int> chemin = ObtenirChemin(parents, depart, arrivee);
            return new ResultatChemin(gScore[arrivee], chemin);
        }



        private static double Heuristique(Graphe<StationMetro> graphe, int a, int b)
        {
            Noeud<StationMetro> noeudA = graphe.TrouverNoeudParId(a);
            Noeud<StationMetro> noeudB = graphe.TrouverNoeudParId(b);

            if (noeudA == null || noeudB == null) return 0;

            double dx = noeudA.Contenu.Longitude - noeudB.Contenu.Longitude;
            double dy = noeudA.Contenu.Latitude - noeudB.Contenu.Latitude;
            return Math.Sqrt(dx * dx + dy * dy);
        }


        public static ResultatChemin AEtoileListe(Graphe<StationMetro> graphe, List<int> depart, List<int> arrivee)
        {
            double distanceMin = double.MaxValue;
            List<int> cheminMin = new List<int>();

            foreach (int id in depart)
            {
                foreach (int id2 in arrivee)
                {
                    ResultatChemin resultat = AEtoile(graphe, id, id2);
                    if (resultat.PoidsTotal < distanceMin)
                    {
                        distanceMin = resultat.PoidsTotal;
                        cheminMin = resultat.Chemin;
                    }
                }
            }

            AfficherChemin(cheminMin, graphe);
            Console.WriteLine($"Poids total du chemin est de : " + distanceMin);
            return new ResultatChemin(distanceMin, cheminMin);
        }


        #endregion

        #region Cycle

        /// <summary>
        /// Vérifie si un graphe contient un cycle
        /// </summary>
        /// <param name="mat">Matrice d'adjacence du graphe</param>
        /// <returns>Un stack contenant les noeuds du cycle s'il existe, sinon un stack vide</returns>
        public static Stack<T> ContientCycle(double[,] mat, Dictionary<T, int> mapIdIndex)
        {
            int nbNodes = mat.GetLength(0);
            bool[] dejaExplore = new bool[nbNodes];
            int[] parent = new int[nbNodes];
            Array.Fill(parent, -1);

            for (int i = 0; i < nbNodes; i++)
            {
                if (!dejaExplore[i])
                {
                    Stack<(int node, int parent)> stack = new Stack<(int node, int parent)>();
                    stack.Push((i, -1));

                    while (stack.Count > 0)
                    {
                        var (node, parentNode) = stack.Pop();

                        if (dejaExplore[node])
                        {
                            Stack<T> cycle = new Stack<T>();
                            int courant = node;

                            while (courant != -1)
                            {
                                T nodeId = mapIdIndex.FirstOrDefault(x => x.Value == courant).Key;
                                cycle.Push(nodeId);
                                courant = parent[courant];
                            }

                            T cycleStartId = mapIdIndex.FirstOrDefault(x => x.Value == node).Key;
                            cycle.Push(cycleStartId);
                            return cycle;
                        }

                        dejaExplore[node] = true;
                        parent[node] = parentNode;

                        for (int j = 0; j < nbNodes; j++)
                        {
                            if (mat[node, j] !=0 && j != parentNode)
                            {
                                stack.Push((j, node));
                            }
                        }
                    }
                }
            }
            return new Stack<T>();
        }

        #endregion

        #region Affichage

        /// <summary>
        /// Affiche les distances depuis le noeud de départ
        /// </summary>
        /// <param name="distances">Distances depuis le noeud de départ</param>
        /// <param name="idToIndex">Dictionnaire associant les IDs à des indices</param>
        private static void AfficherSolutionMatrice(int[] distances)
        {
            Console.WriteLine("Distances depuis le départ :");

            for(int i=0; i < distances.Length; i++)
            {
                Console.WriteLine($"Noeud {i}: {distances[i]}");
            }
        }

        /// <summary>
        /// Affiche les distances depuis le noeud de départ
        /// </summary>
        /// <param name="distances">Distances depuis le noeud de départ</param>
        /// <param name="graph">Graphe sous forme de liste d'adjacence</param>
        private static void AfficherSolutionListe(int[] distances, Dictionary<Noeud<T>, List<(Noeud<T>, double poids)>> listeAdjacence)
        {
            Console.WriteLine("Distances depuis le départ :");

            foreach (var kvp in listeAdjacence)
            {
                Noeud<T> node = kvp.Key;
                int nodeId = node.Id; 

                string distance = distances[nodeId].ToString();  
                Console.WriteLine($"Noeud {nodeId}: {distance}"); 
                   
            }
        }

        private static List<int> ObtenirChemin(int[] parents, int departIndex, int arriveeIndex)
        {
            List<int> chemin = new List<int>();
            for (int courant = arriveeIndex; courant != -1; courant = parents[courant])
            {
                chemin.Add(courant);
            }
            chemin.Reverse();
            return chemin;
        }


        private static void AfficherChemin(List<int> chemin, Graphe<StationMetro> graphe)
        {
            
            Console.WriteLine("Le chemin à prendre est :");
            List<string> libelleChemin = new List<string>();

            foreach (int id in chemin)
            {
                var contenu = graphe.TrouverNoeudParId(id).Contenu;
                if(contenu != null)
                {
                    libelleChemin.Add(contenu.Libelle + " (Ligne " + contenu.Ligne + ")");
                }
            }

            Console.WriteLine(string.Join(" -> ", libelleChemin));
        }


        #endregion


    }
}
