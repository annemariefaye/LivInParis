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
            RechercheStationProche recherche2 = new RechercheStationProche("8 rue Sainte-Anne, 75001 Paris", graphe);
            await recherche.InitialiserAsync(); 
            await recherche2.InitialiserAsync();

            List<int> depart;
            List<int> arrivee;
            float tempsDeplacementDepart = 0;
            float tempsDeplacementArrivee = 0;
            double distanceMin = double.MaxValue;
            try
            {
                depart = recherche.IdStationsProches;
                arrivee = recherche2.IdStationsProches;

                tempsDeplacementDepart = recherche.TempsDeplacement;
                tempsDeplacementArrivee = recherche2.TempsDeplacement;

                RechercheChemin<StationMetro>.DijkstraListe(graphe, depart, arrivee);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur : {e.Message}");
            }

            Console.WriteLine("Temps total de déplacement : " + tempsDeplacementArrivee+tempsDeplacementDepart);


            Console.WriteLine("Appuyez sur une touche pour continuer...");

            Console.ReadKey();

            Menu menu = new Menu();

        }
        
    }
}
