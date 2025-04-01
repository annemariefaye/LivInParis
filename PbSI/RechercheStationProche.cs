using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PbSI
{
    class RechercheStationProche
    {
        private readonly string adresse;
        private readonly Graphe<StationMetro> graphe;
        private List<int> idStationsProches = new List<int> { -1};
        private const double RayonTerre = 6371000.0;

        public RechercheStationProche(string adresse, Graphe<StationMetro> graphe)
        {
            this.adresse = adresse;
            this.graphe = graphe;
        }

        public async Task InitialiserAsync()
        {
            (double lon, double lat)? coordonnees = await ConvertirAdresseEnCoordonnees(adresse);
            RechercherStationsProches(coordonnees);
        }

        public List<int> IdStationsProches
        {
            get
            {
                if (this.idStationsProches[0] != -1)
                    return this.idStationsProches;
                else
                    throw new Exception("Aucune station trouvée.");
            }
        }

        public static async Task<(double lon, double lat)?> ConvertirAdresseEnCoordonnees(string adresse)
        {
            string url = $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(adresse)}";

            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# App");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JArray data = JArray.Parse(json);

                    if (data.Count > 0)
                    {
                        double lat = Convert.ToDouble(data[0]["lat"]);
                        double lon = Convert.ToDouble(data[0]["lon"]);
                        Console.WriteLine($"Coordonnées trouvées : {lat}, {lon}");
                        return (lon, lat);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la conversion de l'adresse : {ex.Message}");
            }
            return null;
        }

        public void RechercherStationsProches((double lon, double lat)? coordonneesOriginelles)
        {
            if (coordonneesOriginelles == null) return;

            Console.WriteLine("Recherche des stations les plus proches...");

            double distanceMin = double.MaxValue;
            List<int> stationsProches = new List<int>();

            foreach (var noeud in this.graphe.Noeuds)
            {
                if (noeud is Noeud<StationMetro> station && station.Contenu != null)
                {
                    (double lon, double lat) coordonneesStation = (station.Contenu.Longitude, station.Contenu.Latitude);
                    double distance = CalculerDistance(coordonneesOriginelles.Value, coordonneesStation);

                    //Console.WriteLine("La distance est : " + distance);

                    if (distance < distanceMin)
                    {
                        distanceMin = distance;
                        stationsProches.Clear(); // On enlève les anciennes stations si on trouve plus proche
                        stationsProches.Add(station.Id);
                    }
                    else if (Math.Abs(distance - distanceMin) < 1e-6) // Tolérance pour éviter erreurs de précision
                    {
                        stationsProches.Add(station.Id);
                    }
                }
            }

            // Stocke la liste des stations proches
            this.idStationsProches = stationsProches;

            Console.WriteLine("Stations les plus proches : " + string.Join(", ", stationsProches));
        }

        public double CalculerDistance((double lon, double lat) origine, (double lon, double lat) destination)
        {
            double lon1 = DegresVersRadians(origine.lon);
            double lat1 = DegresVersRadians(origine.lat);
            double lon2 = DegresVersRadians(destination.lon);
            double lat2 = DegresVersRadians(destination.lat);

            double deltaLon = lon2 - lon1;
            double deltaLat = lat2 - lat1;

            double a = Math.Pow(Math.Sin(deltaLat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(deltaLon / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return RayonTerre * c;
        }

        private double DegresVersRadians(double degres)
        {
            return degres * (Math.PI / 180);
        }
    }
}
