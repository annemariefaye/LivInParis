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
            RechercheStationProche recherche2 = new RechercheStationProche("22 rue du sergent bauchat, 75012", graphe);
            await recherche.InitialiserAsync(); // On attend la fin de l'initialisation
            await recherche2.InitialiserAsync(); // On attend la fin de l'initialisation

            int depart;
            int arrivee;
            try
            {
                depart = recherche.IdStationProche;
                arrivee = recherche2.IdStationProche;
                RechercheChemin<StationMetro>.Dijkstra(graphe.MatriceAdjacence, depart, arrivee);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur : {e.Message}");
            }
            
            /*RechercheChemin<StationMetro>.DFS_Liste(graphe, 1);
            RechercheChemin<StationMetro>.DFS_Matrice(graphe, 1);
            RechercheChemin<StationMetro>.Dijkstra(graphe.MatriceAdjacence, 1, 300);*/
            


            /*Connexion bdd = new Connexion();
            bdd.executerRequete("SELECT * FROM Cuisinier");
            bdd.afficherResultatRequete();
            bdd.exporterResultatRequete();
            bdd.fermerConnexion();*/


        }
        
    }
}
