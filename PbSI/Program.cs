using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace PbSI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            
            ReseauMetro reseau = new ReseauMetro("MetroParis.xlsx");
            Graphe<StationMetro> graphe = reseau.Graphe;

            double[,] m = graphe.MatriceAdjacence;
            var l = graphe.ListeAdjacence;


            RechercheStationProche recherche = new RechercheStationProche("55 Rue du Faubourg Saint-Honoré, 75008 Paris, France", graphe);
            await recherche.InitialiserAsync(); // On attend la fin de l'initialisation

            int depart;
            try
            {
                depart = recherche.IdStationProche;
                RechercheChemin<StationMetro>.FloydWarshall(graphe);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur : {e.Message}");
            }*/
            
            /*RechercheChemin<StationMetro>.DFS_Liste(graphe, 1);
            RechercheChemin<StationMetro>.DFS_Matrice(graphe, 1);
            RechercheChemin<StationMetro>.Dijkstra(graphe.MatriceAdjacence, 1, 300);*/
            
            /*RechercheChemin<StationMetro>.DFS_Liste(graphe, 1);
            RechercheChemin<StationMetro>.DFS_Matrice(graphe, 1);
            RechercheChemin<StationMetro>.Dijkstra(graphe.MatriceAdjacence, 1, 300);*/
            
            /*

            Connexion bdd = new Connexion();
            bdd.executerRequete("SELECT * FROM Cuisinier");
            bdd.afficherResultatRequete();
            bdd.exporterResultatRequete();
            bdd.fermerConnexion();
            */

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
