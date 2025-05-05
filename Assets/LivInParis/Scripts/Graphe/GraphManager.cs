using UnityEngine;

namespace PbSI
{
    /// <summary>
    /// Classe pour gérer le graphe principal de l'application.
    /// </summary>
    public class GraphManager : MonoBehaviour
    {
        #region Attributs
        /// <summary>
        /// Instance unique de GraphManager.
        /// </summary>
        private static GraphManager _instance;

        /// <summary>
        /// Graphe principal de l'application.
        /// </summary>
        private Graphe<StationMetro> graphe;
        #endregion

        #region Méthodes
        /// <summary>
        /// Retourne l'instance unique de GraphManager.
        /// </summary>
        public static GraphManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("GraphManager");
                    _instance = go.AddComponent<GraphManager>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        /// <summary>
        /// Retourne le graphe principal.
        /// </summary>
        public Graphe<StationMetro> Graphe
        {
            get { return graphe; }
        }

        /// <summary>
        /// Méthode appelée lors de l'initialisation de l'objet.
        /// </summary>
        void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);

            ReseauMetro reseau = new ReseauMetro();
            graphe = reseau.Graphe;
        }
        #endregion
    }
}