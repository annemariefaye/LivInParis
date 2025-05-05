using System.Collections;
using UnityEngine;

namespace PbSI
{

    /// <summary>
    /// Classe permettant de gérer les informations affichées dans le panneau d'information.
    /// </summary>
    public class InfoPanelManager : MonoBehaviour
    {
        #region Méthodes
        /// <summary>
        /// Méthode appelée à chaque frame pour gérer les interactions avec le panneau d'information.
        /// </summary>
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                GameObject hit = null;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out hitInfo))
                {
                    hit = hitInfo.collider.gameObject;
                }

                if (hit == null || hit.GetComponent<EventPointer>() == null)
                {
                    if (EventPointer.panneauActif != null)
                    {
                        var canvasGroup = EventPointer.panneauActif.GetComponent<CanvasGroup>();
                        if (canvasGroup != null)
                        {
                            StartCoroutine(EventPointer.FadeOutStatic(canvasGroup));
                            EventPointer.panneauActif = null;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
