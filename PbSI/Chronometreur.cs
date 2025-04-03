using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PbSI
{
    using System;
    using System.Diagnostics;

    public class Chronometreur
    {

        public static void Chronometre(Action fonction)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            fonction(); 

            stopwatch.Stop();
            Console.WriteLine($"Temps d'exécution : {stopwatch.ElapsedMilliseconds} ms");
        }

        public static void ChronometreDijkstra(Graphe<StationMetro> graphe, int depart, int arrivee)
        {
            Console.WriteLine("Chronométrage de Dijkstra :");
            Chronometreur.Chronometre(() =>
            {
                RechercheChemin<StationMetro>.Dijkstra(graphe, depart, arrivee);
            });
        }

        public static void ChronometreBellmanFord(Graphe<StationMetro> graphe, int depart, int arrivee)
        {
            Console.WriteLine("Chronométrage de Bellman-Ford :");
            Chronometreur.Chronometre(() =>
            {
                RechercheChemin<StationMetro>.BellmanFord(graphe, depart, arrivee);
            });
        }

        public static void ChronometreAEtoile(Graphe<StationMetro> graphe, int depart, int arrivee)
        {
            Console.WriteLine("Chronométrage de A* :");    
            Chronometreur.Chronometre(() =>
            {
                RechercheChemin<StationMetro>.AEtoile(graphe, depart, arrivee);
            });
        }

        public static void ChronometreFloydWarshall(Graphe<StationMetro> graphe, int depart, int arrivee)
        {
            Console.WriteLine("Chronométrage de Floyd-Warshall :");
            Chronometreur.Chronometre(() =>
            {
                RechercheChemin<StationMetro>.FloydWarshall(graphe, depart, arrivee);
            });
        }
    }

}
