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
            Console.WriteLine();
            await recherche2.InitialiserAsync();
            Console.WriteLine();


            List<int> depart;
            List<int> arrivee;
            float tempsDeplacementDepart = 0;
            float tempsDeplacementArrivee = 0;
            try
            {
                depart = recherche.IdStationsProches;
                arrivee = recherche2.IdStationsProches;

                tempsDeplacementDepart = recherche.TempsDeplacement;
                tempsDeplacementArrivee = recherche2.TempsDeplacement;

                var resultat = RechercheChemin<StationMetro>.DijkstraListe(graphe, depart, arrivee);

                if (resultat != null)
                {
                    double tempsTotal = tempsDeplacementDepart + tempsDeplacementArrivee + resultat.PoidsTotal;
                    Console.WriteLine("Temps total de déplacement : " + (int)tempsTotal + "");
                }
                else
                {
                    Console.WriteLine("Aucun chemin trouvé.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur : {e.Message}");
            }

            Console.WriteLine("Appuyez sur une touche pour continuer...");

            Console.ReadKey();

            //Menu menu = new Menu();

        }
        
    }
}
