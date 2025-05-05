using UnityEngine;

namespace PbSI
{
    /// <summary>
    /// Classe permettant de suivre les déplacements du joueur.
    /// </summary>
    public class SuivreJoueur : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset = new Vector3(0, 10, -10);
        public float smoothSpeed = 0.125f;
        public float vitesseZoom = 10f;
        public float limiteZoomMin = 10f;
        public float limiteZoomMax = 100f;

        private Camera cam;

        #region Méthodes
        /// <summary>
        /// Méthode appelée au démarrage pour initialiser le suivi.
        /// </summary>
        void Start()
        {
            cam = GetComponent<Camera>();
        }

        /// <summary>
        /// Méthode appelée à chaque frame pour mettre à jour la position suivie.
        /// </summary>
        void LateUpdate()
        {
            if (target != null)
            {
                Vector3 desiredPosition = target.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(
                    transform.position,
                    desiredPosition,
                    smoothSpeed
                );
                transform.position = smoothedPosition;
            }

            float zoom = Input.GetAxis("Mouse ScrollWheel") * vitesseZoom;
            if (zoom != 0 && cam != null)
            {
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - zoom, limiteZoomMin, limiteZoomMax);
            }
        }
        #endregion
    }
}
