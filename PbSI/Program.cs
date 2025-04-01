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
            RechercheStationProche recherche2 = new RechercheStationProche("Place Charles de Gaulle, 75017 Paris", graphe);
            await recherche.InitialiserAsync(); 
            await recherche2.InitialiserAsync();

            List<int> depart;
            List<int> arrivee;
            double distanceMin = double.MaxValue;
            try
            {
                depart = recherche.IdStationsProches;
                arrivee = recherche2.IdStationsProches;

                RechercheChemin<StationMetro>.DijkstraListe(graphe, depart, arrivee);
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
