using UnityEngine;

namespace PbSI
{

    /// <summary>
    /// Classe permettant de gérer les déplacements de la caméra.
    /// </summary>
    public class DeplacementCamera : MonoBehaviour
    {
        #region Attributs
        /// <summary>
        /// Dernière position de la souris.
        /// </summary>
        private Vector3 dernierePosition;

        /// <summary>
        /// Indique si la caméra est en train de glisser.
        /// </summary>
        private bool estEnTrainDeGlisser = false;

        /// <summary>
        /// Vitesse de déplacement de la caméra.
        /// </summary>
        public float vitesseDeplacement = 5f;

        /// <summary>
        /// Vitesse de zoom de la caméra.
        /// </summary>
        public float vitesseZoom = 10f;

        /// <summary>
        /// Limite minimale du zoom.
        /// </summary>
        public float limiteZoomMin = 10f;

        /// <summary>
        /// Limite maximale du zoom.
        /// </summary>
        public float limiteZoomMax = 100f;
        #endregion

        #region Méthodes
        /// <summary>
        /// Méthode appelée à chaque frame pour gérer les interactions de la caméra.
        /// </summary>
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                dernierePosition = Input.mousePosition;
                estEnTrainDeGlisser = true;
            }

            if (estEnTrainDeGlisser)
            {
                Vector3 delta = Input.mousePosition - dernierePosition;
                dernierePosition = Input.mousePosition;

                Vector3 mouvement =
                    new Vector3(-delta.x, 0, -delta.y) * vitesseDeplacement * Time.deltaTime;
                transform.Translate(mouvement, Space.World);
            }

            if (Input.GetMouseButtonUp(0))
            {
                estEnTrainDeGlisser = false;
            }

            float zoom = Input.GetAxis("Mouse ScrollWheel") * vitesseZoom;
            if (zoom != 0)
            {
                Camera camera = GetComponent<Camera>();
                camera.fieldOfView = Mathf.Clamp(
                    camera.fieldOfView - zoom,
                    limiteZoomMin,
                    limiteZoomMax
                );
            }
        }
        #endregion
    }
}