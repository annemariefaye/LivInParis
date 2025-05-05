using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PbSI
{

    /// <summary>
    /// Classe représentant les données d'une station.
    /// </summary>
    [System.Serializable]
    public class ItemStationData
    {
        #region Attributs
        /// <summary>
        /// Identifiant de la station.
        /// </summary>
        public int id;

        /// <summary>
        /// Ligne de transport associée à la station.
        /// </summary>
        public string ligne;

        /// <summary>
        /// Libellé de la station.
        /// </summary>
        public string libelle;

        /// <summary>
        /// Longitude de la station.
        /// </summary>
        public double longitude;

        /// <summary>
        /// Latitude de la station.
        /// </summary>
        public double latitude;

        /// <summary>
        /// Commune où se trouve la station.
        /// </summary>
        public string commune;

        /// <summary>
        /// Code INSEE de la commune.
        /// </summary>
        public int codeInsee;
        #endregion

        #region Constructeurs
        /// <summary>
        /// Constructeur de la classe ItemStationData.
        /// </summary>
        /// <param name="id">Identifiant de la station.</param>
        /// <param name="ligne">Ligne de transport associée.</param>
        /// <param name="libelle">Libellé de la station.</param>
        /// <param name="longitude">Longitude de la station.</param>
        /// <param name="latitude">Latitude de la station.</param>
        /// <param name="commune">Commune où se trouve la station.</param>
        /// <param name="codeInsee">Code INSEE de la commune.</param>
        public ItemStationData(
            int id,
            string ligne,
            string libelle,
            double longitude,
            double latitude,
            string commune,
            int codeInsee
        )
        {
            this.id = id;
            this.ligne = ligne;
            this.libelle = libelle;
            this.longitude = longitude;
            this.latitude = latitude;
            this.commune = commune;
            this.codeInsee = codeInsee;
        }
        #endregion
    }
}
