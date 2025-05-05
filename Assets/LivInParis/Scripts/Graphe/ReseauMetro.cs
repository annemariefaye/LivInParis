using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace PbSI
{

    public class ReseauMetro
    {
        #region Attributs
        private List<ItemStationGraphe> stationGraphe;
        private List<ItemStationData> stationData;
        private readonly Graphe<StationMetro> graphe;
        private readonly List<Noeud<StationMetro>> stations;
        #endregion

        #region Constructeurs

        public ReseauMetro()
        {
            this.graphe = new Graphe<StationMetro>();
            this.stations = new List<Noeud<StationMetro>>();
            LoadCSV loadcsv = new LoadCSV();
            stationGraphe = loadcsv.StationGrapheList;
            stationData = loadcsv.StationDataList;
            /*Debug.Log("reseau lancé");
            Debug.Log(stationData.Count);
            Debug.Log(stationGraphe.Count);*/
            CreerStations(stationData);
            CreerRelations(stationGraphe);
            CreerCorrespondances(stationGraphe);
        }
        #endregion

        #region Propriétés
        public Graphe<StationMetro> Graphe => this.graphe;
        #endregion

        #region Méthodes
        private void CreerStations(List<ItemStationData> stationData)
        {
            foreach (var data in stationData)
            {
                var station = new Noeud<StationMetro>(data.id, StationMetro.Parse(data));
                graphe.AjouterMembre(station);
                stations.Add(station);
            }
        }

        /// <summary>
        /// Crée les relations entre les stations à partir des données des arcs
        /// </summary>
        /// <param name="donneesArcs">lignes de données des arcs</param>
        private void CreerRelations(List<ItemStationGraphe> stationGraphe)
        {
            HashSet<(int, int)> relationsAjoutees = new HashSet<(int, int)>();

            foreach (var dataStation in stationGraphe)
            {
                if (dataStation.stationId != -1 && dataStation.precedentId != "-1")
                {
                    int idStation = dataStation.stationId;
                    int idStationPrecedente = int.Parse(dataStation.precedentId);
                    int temps = dataStation.tempsEntreDeuxStations;

                    var stationCurrent = graphe.TrouverNoeudParId(idStation);
                    var stationPrecedente = graphe.TrouverNoeudParId(idStationPrecedente);

                    if (stationCurrent != null && stationPrecedente != null)
                    {
                        var relation = (idStationPrecedente, idStation);
                        if (!relationsAjoutees.Contains(relation))
                        {
                            graphe.AjouterRelation(stationPrecedente, stationCurrent, temps);
                            graphe.AjouterRelation(stationCurrent, stationPrecedente, temps);
                            relationsAjoutees.Add(relation);
                        }
                    }
                }

                if (dataStation.stationId == 44 || dataStation.stationId == 69)
                {
                    int idStation = dataStation.stationId;
                    int idStationSuivante = int.Parse(dataStation.suivantId);

                    var stationCurrent = graphe.TrouverNoeudParId(idStation);
                    var stationSuivante = graphe.TrouverNoeudParId(idStationSuivante);

                    if (stationCurrent != null && stationSuivante != null)
                    {
                        int temps = dataStation.tempsEntreDeuxStations;
                        var relation1 = (idStation, idStationSuivante);
                        var relation2 = (idStationSuivante, idStation);

                        if (
                            !relationsAjoutees.Contains(relation1)
                            && !relationsAjoutees.Contains(relation2)
                        )
                        {
                            graphe.AjouterRelation(stationCurrent, stationSuivante, temps);
                            graphe.AjouterRelation(stationSuivante, stationCurrent, temps);
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
        private void CreerCorrespondances(List<ItemStationGraphe> stationGraphe)
        {
            Dictionary<string, List<int>> correspondances = new Dictionary<string, List<int>>();

            foreach (var dataStation in stationGraphe)
            {
                string libelleStation = dataStation.stationNom;
                int idStation = dataStation.stationId;

                if (!correspondances.ContainsKey(libelleStation))
                {
                    correspondances[libelleStation] = new List<int>();
                }
                correspondances[libelleStation].Add(idStation);
            }

            foreach (var dataStation in stationGraphe)
            {
                if (dataStation.tempsDeChangement != -1 && dataStation.stationId != -1)
                {
                    int idStation = dataStation.stationId;
                    int temps = dataStation.tempsDeChangement;
                    string libelleStation = dataStation.stationNom;

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
                                    graphe.AjouterRelation(
                                        stationCurrent,
                                        stationCorrespondance,
                                        temps
                                    );
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
}