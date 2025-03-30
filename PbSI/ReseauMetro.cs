using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace PbSI
{
    public class ReseauMetro
    {
        private readonly string chemin;
        private readonly Graphe<StationMetro> graphe;

        public ReseauMetro(string chemin) 
        { 
            this.chemin = chemin;
            this.graphe = new Graphe<StationMetro>();
            var donneesNoeuds = LireFeuilleNoeud();
            var stations = CreerStations(donneesNoeuds);
            var donneesArcs = LireFeuilleArc();
            CreerRelations(stations, donneesArcs);
            CreerCorrespondances(stations, donneesArcs);

        }

        public Graphe<StationMetro> Graphe { get { return this.graphe; } }

        private List<Noeud<StationMetro>> CreerStations(List<List<string>> donneesNoeuds)
        {
            List<Noeud<StationMetro>> stations = new List<Noeud<StationMetro>>();

            foreach(var data in donneesNoeuds)
            {
                Noeud<StationMetro> n = new Noeud<StationMetro>(int.Parse(data[0]), StationMetro.Parse(data));
                stations.Add(n);
            }

            return stations;
        }

        private int CompterLiensUniques(List<List<string>> donneesArcs)
        {
            HashSet<(int, int)> liensUniques = new HashSet<(int, int)>();

            foreach (var dataStation in donneesArcs)
            {
                if (dataStation[0] != "-1" && dataStation[2] != "-1")
                {
                    int idStation = int.Parse(dataStation[0]);
                    int idStationPrecedente = int.Parse(dataStation[2]);
                    int idStationSuivante = int.Parse(dataStation[3]);

                    // Ajouter les liens
                    liensUniques.Add((idStationPrecedente, idStation));
                    liensUniques.Add((idStation, idStationPrecedente)); // Pour le lien inverse
                }
            }

            return liensUniques.Count; 
        }



        private void CreerRelations(List<Noeud<StationMetro>> stations, List<List<string>> donneesArcs)
        {
            HashSet<(int, int)> relationsAjoutees = new HashSet<(int, int)>();

            foreach (var dataStation in donneesArcs)
            {
                if (dataStation[0] != "-1" && dataStation[2] != "-1")
                {
                    int idStation = int.Parse(dataStation[0]);
                    int idStationPrecedente = int.Parse(dataStation[2]);

                    Noeud<StationMetro>? stationCurrent = TrouverNoeudParId(stations, idStation);
                    Noeud<StationMetro>? stationPrecedente = TrouverNoeudParId(stations, idStationPrecedente);

                    double temps = int.Parse(dataStation[4]);

                    if (dataStation[6] == "-1")
                    {
                        if (idStationPrecedente != -1 && stationCurrent != null && stationPrecedente != null)
                        {
                            var relation = (idStationPrecedente, idStation);
                            if (!relationsAjoutees.Contains(relation))
                            {
                                //Console.WriteLine("Ajout de la relation entre " + stationPrecedente.Contenu.Libelle + " et " + stationCurrent.Contenu.Libelle + " en " + temps + " minutes");
                                this.graphe.AjouterRelation(stationPrecedente, stationCurrent, temps);
                                this.graphe.AjouterRelation(stationCurrent, stationPrecedente, temps);
                                relationsAjoutees.Add(relation);
                            }
                        }
                    }
                    else
                    {
                        if (idStationPrecedente != -1 && stationCurrent != null && stationPrecedente != null)
                        {
                            var relation = (idStationPrecedente, idStation);
                            if (!relationsAjoutees.Contains(relation))
                            {
                                //Console.WriteLine("(SU) Ajout de la relation entre " + stationPrecedente.Contenu.Libelle + " et " + stationCurrent.Contenu.Libelle + " en " + temps + " minutes");
                                this.graphe.AjouterRelation(stationPrecedente, stationCurrent, temps);
                                relationsAjoutees.Add(relation);
                            }
                        }
                    }
                }

                // Ajout en dur des seuls noeuds qui sont dans ni précédent ni suivant
                if (dataStation[0] == "44" || dataStation[0] == "69")
                {
                    int idStation = int.Parse(dataStation[0]);

                    Noeud<StationMetro>? stationCurrent = TrouverNoeudParId(stations, idStation);
                    Noeud<StationMetro>? stationSuivante = TrouverNoeudParId(stations, int.Parse(dataStation[3]));

                    double temps = int.Parse(donneesArcs[idStation + 1][4]);

                    var relation1 = (idStation, int.Parse(dataStation[3]));
                    var relation2 = (int.Parse(dataStation[3]), idStation);
                    if (!relationsAjoutees.Contains(relation1) && !relationsAjoutees.Contains(relation2))
                    {
                        this.graphe.AjouterRelation(stationSuivante, stationCurrent, temps);
                        this.graphe.AjouterRelation(stationCurrent, stationSuivante, temps);
                        relationsAjoutees.Add(relation1);
                        relationsAjoutees.Add(relation2);
                    }
                }
            }
        }


        private void CreerCorrespondances(List<Noeud<StationMetro>> stations, List<List<string>> donneesArcs)
        {
            Dictionary<string, List<int>> correspondances = new Dictionary<string, List<int>>();

            foreach (var dataStation in donneesArcs)
            {
                string libelleStation = dataStation[1];
                int idStation = int.Parse(dataStation[0]);

                if (!correspondances.ContainsKey(libelleStation))
                {
                    correspondances[libelleStation] = new List<int>();
                }
                correspondances[libelleStation].Add(idStation);
            }

            foreach (var dataStation in donneesArcs)
            {
                if (dataStation[5] != "-1" && dataStation[0] != "-1")
                {
                    int idStation = int.Parse(dataStation[0]);
                    int temps = int.Parse(dataStation[5]);
                    string libelleStation = dataStation[1];

                    if (correspondances.ContainsKey(libelleStation))
                    {
                        foreach (var idCorrespondance in correspondances[libelleStation])
                        {
                            if (idCorrespondance != idStation)
                            {
                                Noeud<StationMetro> stationCurrent = TrouverNoeudParId(stations, int.Parse(dataStation[0]));
                                Noeud<StationMetro> stationCorrespondance = TrouverNoeudParId(stations, idCorrespondance);

                                this.graphe.AjouterRelation(stationCurrent, stationCorrespondance, temps);
                            }
                        }
                    }
                }
            }

        }

        private Noeud<StationMetro> TrouverNoeudParId(List<Noeud<StationMetro>> stations, int id)
        {
            return stations.FirstOrDefault(station => station.Id == id)
                   ?? throw new KeyNotFoundException($"Aucun noeud trouvé avec l'ID {id}");
        }



        private List<List<string>> LireFeuilleNoeud()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var paquet = new ExcelPackage(new FileInfo(this.chemin)))
            {
                var feuilleDeTravail = paquet.Workbook.Worksheets[0];
                List<List<string>> donnees = new List<List<string>>();

                for (int ligne = 2; ligne <= feuilleDeTravail.Dimension.End.Row; ligne++)
                {
                    List<string> ligneDonnees = new List<string>();
                    for (int colonne = 1; colonne <= feuilleDeTravail.Dimension.End.Column; colonne++)
                    {
                        string valeurCellule = feuilleDeTravail.Cells[ligne, colonne].Value?.ToString() ?? string.Empty;
                        ligneDonnees.Add(valeurCellule);
                    }
                    donnees.Add(ligneDonnees);
                }

                return donnees;
            }
        }


        private List<List<string>> LireFeuilleArc()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var paquet = new ExcelPackage(new FileInfo(this.chemin)))
            {
                var feuilleDeTravail = paquet.Workbook.Worksheets[1];

                List<List<string>> donnees = new List<List<string>>();

                for (int ligne = 2; ligne <= feuilleDeTravail.Dimension.End.Row; ligne++)
                {
                    List<string> ligneDonnees = new List<string>();
                    for (int colonne = 1; colonne <= feuilleDeTravail.Dimension.End.Column; colonne++)
                    {
                        string valeurCellule = feuilleDeTravail.Cells[ligne, colonne].Value?.ToString() ?? string.Empty;

                        if (string.IsNullOrEmpty(valeurCellule))
                        {
                            ligneDonnees.Add("-1");
                        }
                        else
                        {
                            ligneDonnees.Add(valeurCellule);
                        }
                    }
                    donnees.Add(ligneDonnees);
                }

                return donnees;
            }

        }

    }
}
