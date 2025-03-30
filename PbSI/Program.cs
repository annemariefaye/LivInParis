using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PbSI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            /*LectureFichiers relations = new LectureFichiers("relations.mtx");
            List<int[]> tableauMembres = relations.Contenu;
            //relations.AfficherContenu();

            Graphe<int> graphe = new Graphe<int>();

            foreach (int[] relation in tableauMembres)
            {
                graphe.AjouterRelation(relation[0], relation[1]);
                graphe.AjouterRelation(relation[1], relation[0]);
            }

            double[,] m = graphe.MatriceAdjacence;
            var l = graphe.ListeAdjacence;
            
            //graphe.AfficherMatriceAdjacence();
            //graphe.AfficherListeAdjacence();


            RechercheChemin<int>.DFS_Matrice(graphe, 4);
            RechercheChemin<int>.DFS_Liste(graphe, 4);

            Stack<int> cycle = RechercheChemin<int>.ContientCycle(graphe.MatriceAdjacence, graphe.MapIdIndex);
            Console.WriteLine("Ce graphe contient au moins un cycle : " + (cycle.Count != 0));
            if (cycle.Count != 0)
            {
                Console.WriteLine("Le cycle est: ");
                while (cycle.Count > 0)
                {
                    Console.WriteLine(cycle.Pop());
                }
            }

            RechercheChemin<int>.Dijkstra(graphe.MatriceAdjacence, 4, 34, graphe.MapIdIndex);

            //graphe.AfficherProprietes();*/

            ReseauMetro reseau = new ReseauMetro("MetroParis.xlsx");
            Graphe<StationMetro> graphe = reseau.Graphe;

            double[,] m = graphe.MatriceAdjacence;
            var l = graphe.ListeAdjacence;

            //graphe.AfficherListeAdjacence();
            //graphe.AfficherMatriceAdjacence();

            //RechercheChemin<int>.DFS_Liste(graphe, 1);
            //RechercheChemin<int>.DFS_Matrice(graphe, 1);

            /*
            RechercheStationProche recherche = new RechercheStationProche("55 Rue du Faubourg Saint-Honoré, 75008 Paris, France", graphe);
            await recherche.InitialiserAsync(); // On attend la fin de l'initialisation

            int depart;
            try
            {
                depart = recherche.IdStationProche;
                RechercheChemin<StationMetro>.Dijkstra(graphe.MatriceAdjacence, depart, 1);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur : {e.Message}");
            }
            
            RechercheChemin<int>.DFS_Liste(graphe, 1);
            RechercheChemin<int>.DFS_Matrice(graphe, 1);
            RechercheChemin<ibnt>.Dijkstra(graphe.MatriceAdjacence, 1, 300, graphe.MapIdIndex);
            */


            Connexion bdd = new Connexion();
            bdd.executerRequete("SELECT * FROM Cuisinier");
            bdd.afficherResultatRequete();
            bdd.exporterResultatRequete();
            bdd.fermerConnexion();


        }
        

        /*#region Méthodes d'instanciation

        /// <summary>
        /// Instancie un graphe à partir d'une liste de membres
        /// </summary>
        /// <param name="tableauMembres">Liste de membres</param>
        /// <returns>Un graphe</returns>
        static Graphe InstantiationMatrice(List<int[]> tableauMembres)
        {
            HashSet<int> hashSet = new HashSet<int>(tableauMembres.SelectMany(x => x));

            int taille = hashSet.Count;
            int[,] matrice = new int[taille, taille];

            foreach (int[] mini_tab in tableauMembres)
            {
                matrice[mini_tab[0] - 1, mini_tab[1] - 1] = 1;
                matrice[mini_tab[1] - 1, mini_tab[0] - 1] = 1;
            }

            return new Graphe(matrice);
        }

        /// <summary>
        /// Instancie un graphe à partir d'une liste de membres
        /// </summary>
        /// <param name="tableauMembres">Liste de membres</param>
        /// <returns>Un graphe</returns>
        static Graphe InstantiationListe(List<int[]> tableauMembres)
        {
            Dictionary<int, List<int>> listeAdj = new Dictionary<int, List<int>>();
            foreach (int[] mini_tab in tableauMembres)
            {
                int key = mini_tab[0];
                int value = mini_tab[1];

                if (!listeAdj.ContainsKey(key))
                {
                    listeAdj[key] = new List<int>();
                }

                if (!listeAdj.ContainsKey(value))
                {
                    listeAdj[value] = new List<int>();
                }

                listeAdj[key].Add(value);
                listeAdj[value].Add(key);
            }

            return new Graphe(listeAdj);
        }

        #endregion*/
    }
}
