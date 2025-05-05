using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEngine;

namespace PbSI
{

    /// <summary>
    /// Classe pour gérer le mouvement du joueur dans le sous-graphe.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        #region Attributs
        /// <summary>
        /// Référence à l'objet VisuelSimu pour obtenir les positions du sous-graphe.
        /// </summary>
        public VisuelSimu visu;

        /// <summary>
        /// Liste des positions du sous-graphe à suivre.
        /// </summary>
        public List<Vector3> cheminSousGraphe = null;

        /// <summary>
        /// Vitesse de déplacement du joueur.
        /// </summary>
        public float vitesse = 2.0f;

        /// <summary>
        /// Position actuelle du joueur dans le chemin.
        /// </summary>
        private int indexPosition = 0;

        /// <summary>
        /// Indique si le joueur est en train de se déplacer.
        /// </summary>
        private bool estEnDeplacement = false;
        #endregion

        #region Méthodes
        /// <summary>
        /// Méthode appelée pour mettre à jour le mouvement du joueur.
        /// </summary>
        void Update()
        {
            if (cheminSousGraphe == null || cheminSousGraphe.Count <= 0)
            {
                cheminSousGraphe = visu.GetPosSousGraphe();
                Debug.Log("nb arrets : " + cheminSousGraphe.Count);
            }

            if (cheminSousGraphe.Count > 1 && !estEnDeplacement)
            {
                StartCoroutine(SuivreChemin());
            }
        }

        /// <summary>
        /// Coroutine pour suivre le chemin du sous-graphe.
        /// </summary>
        /// <returns>Coroutine</returns>
        IEnumerator SuivreChemin()
        {
            estEnDeplacement = true;
            transform.position = cheminSousGraphe[indexPosition];
            while (indexPosition < cheminSousGraphe.Count)
            {
                Vector3 targetPosition = cheminSousGraphe[indexPosition];
                float step = vitesse * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

                if (transform.position == targetPosition)
                {
                    indexPosition++;
                }

                yield return null;
            }

            estEnDeplacement = false;
        }

        /// <summary>
        /// Méthode pour dessiner le chemin du sous-graphe.
        /// </summary>
        /// <param name="nouveauChemin">Le nouveau chemin à dessiner</param>
        public void SetChemin(List<Vector3> nouveauChemin)
        {
            cheminSousGraphe = nouveauChemin;
            indexPosition = 0;
        }
        #endregion
    }
}