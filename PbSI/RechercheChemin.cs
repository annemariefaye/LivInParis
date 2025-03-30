namespace PbSI
{
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
            Console.WriteLine($"Le graphe est connexe ? : {connexe}");
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
            Console.WriteLine($"Le graphe est connexe ? : {connexe}");
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
            Console.WriteLine($"Le graphe est connexe ? : {connexe}");

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
            Console.WriteLine($"Le graphe est connexe ? : {connexe}");

            AfficherSolutionListe(distances, graphe.ListeAdjacence);
        }


        #endregion

        #region Plus court chemin

        /// <summary>
        /// Algorithme de Dijkstra pour trouver le plus court chemin entre un noeud de départ et tous les autres noeuds
        /// </summary>
        /// <param name="graph">Graphe sous forme de matrice d'adjacence</param>
        /// <param name="depart">Noeud de départ</param>
        public static void Dijkstra(double[,] matriceAdjacence, int depart, int arrivee)
        {
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

                //Console.WriteLine($"On visite à partir du noeud {mapIdIndex.FirstOrDefault(x => x.Value == indexMinDistance).Key} : ");

                for (int n = 0; n < nbNodes; n++)
                {
                    if (!dejaExplore[n] && matriceAdjacence[indexMinDistance, n] != 0)
                    {
                        double newDist = distances[indexMinDistance] + matriceAdjacence[indexMinDistance, n];

                        if (newDist < distances[n])
                        {
                            distances[n] = newDist;
                            parents[n] = indexMinDistance;
                            Console.WriteLine($"Node : {n}, Poids : {matriceAdjacence[indexMinDistance, n]}, Distance totale : {distances[n]}");
                        }
                    }
                }
                //Console.WriteLine();
            }

            AfficherChemin(parents, departIndex, arriveeIndex);
            Console.WriteLine($"Poids total du chemin de {depart} à {arrivee} : {distances[arriveeIndex]}");
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


        private static void AfficherChemin(int[] parents, int departIndex, int arriveeIndex)
        {
            Stack<int> chemin = new Stack<int>();
            for (int courant = arriveeIndex; courant != -1; courant = parents[courant])
            {
                chemin.Push(courant);
            }

            Console.Write("Chemin de " + departIndex + " à " + arriveeIndex + ": ");
            while (chemin.Count > 0)
            {
                Console.Write(chemin.Pop());
                if (chemin.Count > 0)
                {
                    Console.Write(" -> ");
                }
            }
            Console.WriteLine();
        }


        #endregion
    }
}
