using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PbSI
{

    /// <summary>
    /// Classe représentant un élément de station dans un graphe.
    /// </summary>
    [System.Serializable]
    public class ItemStationGraphe
    {
        #region Attributs
        /// <summary>
        /// Identifiant de la station.
        /// </summary>
        public int stationId;

        /// <summary>
        /// Nom de la station.
        /// </summary>
        public string stationNom;

        /// <summary>
        /// Identifiant de la station précédente.
        /// </summary>
        public string precedentId;

        /// <summary>
        /// Identifiant de la station suivante.
        /// </summary>
        public string suivantId;

        /// <summary>
        /// Temps entre deux stations en secondes.
        /// </summary>
        public int tempsEntreDeuxStations;

        /// <summary>
        /// Temps de changement en secondes.
        /// </summary>
        public int tempsDeChangement;

        /// <summary>
        /// Indique si le sens est unique.
        /// </summary>
        public string sensUnique;
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur de la classe ItemStationGraphe.
        /// </summary>
        /// <param name="stationId">Identifiant de la station.</param>
        /// <param name="stationNom">Nom de la station.</param>
        /// <param name="precedentId">Identifiant de la station précédente.</param>
        /// <param name="suivantId">Identifiant de la station suivante.</param>
        /// <param name="tempsEntreDeuxStations">Temps entre deux stations.</param>
        /// <param name="tempsDeChangement">Temps de changement.</param>
        /// <param name="sensUnique">Indique si le sens est unique.</param>
        public ItemStationGraphe(
            int stationId,
            string stationNom,
            string precedentId,
            string suivantId,
            int tempsEntreDeuxStations,
            int tempsDeChangement,
            string sensUnique
        )
        {
            this.stationId = stationId;
            this.stationNom = stationNom;
            this.precedentId = precedentId;
            this.suivantId = suivantId;
            this.tempsEntreDeuxStations = tempsEntreDeuxStations;
            this.tempsDeChangement = tempsDeChangement;
            this.sensUnique = sensUnique;
        }
        #endregion
    }
}