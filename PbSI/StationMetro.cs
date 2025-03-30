using System.Globalization;


namespace PbSI
{
    public class StationMetro
    {
        #region Attributs spécifiques

        private readonly string libelle;  
        private readonly string ligne;         
        private readonly double longitude;  
        private readonly double latitude;  
        private readonly string commune;    
        private readonly int codeInsee;     

        #endregion

        #region Constructeur

        public StationMetro(string ligne, string libelle, double longitude, double latitude, string commune, int codeInsee)
        {
            this.libelle = libelle;  
            this.ligne = ligne;
            this.longitude = longitude;
            this.latitude = latitude;
            this.commune = commune;
            this.codeInsee = codeInsee;
        }

        #endregion

        #region Propriétés

        /// <summary>
        /// Retourne le libellé de la station  
        /// </summary>
        public string Libelle
        {
            get { return this.libelle; }
        }

        /// <summary>
        /// Retourne la longitude de la station
        /// </summary>
        public double Longitude
        {
            get { return this.longitude; }
        }

        /// <summary>
        /// Retourne la latitude de la station
        /// </summary>
        public double Latitude
        {
            get { return this.latitude; }
        }

        /// <summary>
        /// Retourne le numéro de la ligne
        /// </summary>
        public string Ligne
        {
            get { return this.ligne; }
        }

        /// <summary>
        /// Retourne le nom de la commune
        /// </summary>
        public string Commune
        {
            get { return this.commune; }
        }

        /// <summary>
        /// Retourne le code INSEE de la commune
        /// </summary>
        public int CodeInsee
        {
            get { return this.codeInsee; }
        }

        #endregion

        #region Méthodes

        public override string ToString()
        {
            return $"Station {libelle} (Ligne {ligne}) - {commune}";
        }

        public static StationMetro Parse(List<string> data)
        {
            string ligne = data[1];
            string libelle = data[2];
            double longitude = double.Parse(data[3], CultureInfo.InvariantCulture);
            double latitude = double.Parse(data[4], CultureInfo.InvariantCulture);
            string commune = data[5];
            int codeInsee = int.Parse(data[6]);

            return new StationMetro(ligne, libelle, longitude, latitude, commune, codeInsee);
        }


    #endregion
    }
}
