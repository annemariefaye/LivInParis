using System;
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace PbSI
{
    public class ReseauMetro
    {
        #region Attributs
        private readonly string chemin;
        private readonly Graphe<StationMetro> graphe;
        private readonly List<Noeud<StationMetro>> stations;
        #endregion

        #region Constructeurs

        public ReseauMetro(string chemin)
        {
            this.chemin = chemin;
            this.graphe = new Graphe<StationMetro>();
            this.stations = new List<Noeud<StationMetro>>();
            var donneesNoeuds = LireFeuilleNoeud();
            CreerStations(donneesNoeuds);
            var donneesArcs = LireFeuilleArc();
            CreerRelations(donneesArcs);
            CreerCorrespondances(donneesArcs);
        }
        #endregion

        #region Propriétés
        public Graphe<StationMetro> Graphe => this.graphe;
        #endregion

        #region Méthodes
        private void CreerStations(List<List<string>> donneesNoeuds)
        {
            foreach (var data in donneesNoeuds)
            {
                var station = new Noeud<StationMetro>(int.Parse(data[0]), StationMetro.Parse(data));
                graphe.AjouterMembre(station);
                stations.Add(station);
            }
        }

        /// <summary>
        /// Crée les relations entre les stations à partir des données des arcs
        /// </summary>
        /// <param name="donneesArcs">lignes de données des arcs</param>
        private void CreerRelations(List<List<string>> donneesArcs)
        {
            HashSet<(int, int)> relationsAjoutees = new HashSet<(int, int)>();

            foreach (var dataStation in donneesArcs)
            {
                if (dataStation[0] != "-1" && dataStation[2] != "-1")
                {
                    int idStation = int.Parse(dataStation[0]);
                    int idStationPrecedente = int.Parse(dataStation[2]);
                    double temps = int.Parse(dataStation[4]);

                    var stationCurrent = graphe.TrouverNoeudParId(idStation);
                    var stationPrecedente = graphe.TrouverNoeudParId(idStationPrecedente);

                    if (stationCurrent != null && stationPrecedente != null)
                    {
                        var relation = (idStationPrecedente, idStation);
                        if (!relationsAjoutees.Contains(relation))
                        {
                            this.graphe.AjouterRelation(stationPrecedente, stationCurrent, temps);
                            this.graphe.AjouterRelation(stationCurrent, stationPrecedente, temps);
                            relationsAjoutees.Add(relation);
                        }
                    }
                }

                if (dataStation[0] == "44" || dataStation[0] == "69")
                {
                    int idStation = int.Parse(dataStation[0]);
                    var stationCurrent = graphe.TrouverNoeudParId(idStation);
                    var stationSuivante = graphe.TrouverNoeudParId(int.Parse(dataStation[3]));

                    if (stationCurrent != null && stationSuivante != null)
                    {
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
        }

        /// <summary>
        /// Crée les correspondances entre les stations à partir des données des arcs
        /// </summary>
        /// <param name="donneesArcs">Ligne de données des arcs</param>
        private void CreerCorrespondances(List<List<string>> donneesArcs)
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

                    if (correspondances.TryGetValue(libelleStation, out var idsCorrespondance))
                    {
                        foreach (var idCorrespondance in idsCorrespondance)
                        {
                            if (idCorrespondance != idStation)
                            {
                                var stationCurrent = graphe.TrouverNoeudParId(idStation);
                                var stationCorrespondance = graphe.TrouverNoeudParId(idCorrespondance);

                                if (stationCurrent != null && stationCorrespondance != null)
                                {
                                    this.graphe.AjouterRelation(stationCurrent, stationCorrespondance, temps);
                                }
                            }
                        }
                    }
                }
            }
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
                        ligneDonnees.Add(string.IsNullOrEmpty(valeurCellule) ? "-1" : valeurCellule);
                    }
                    donnees.Add(ligneDonnees);
                }

                return donnees;
            }
        }
    }
    #endregion
}