using UnityEngine;

public class DeplacementCamera : MonoBehaviour
{
    private Vector3 dernierePosition;
    private bool estEnTrainDeGlisser = false;
    public float vitesseDeplacement = 5f;
    public float vitesseZoom = 10f;
    public float limiteZoomMin = 10f;
    public float limiteZoomMax = 100f;

    void Update()
    {
        // Déplacement de la caméra avec la souris
        if (Input.GetMouseButtonDown(0))
        {
            dernierePosition = Input.mousePosition;
            estEnTrainDeGlisser = true;
        }

        if (estEnTrainDeGlisser)
        {
            Vector3 delta = Input.mousePosition - dernierePosition;
            dernierePosition = Input.mousePosition;

            Vector3 mouvement = new Vector3(-delta.x, 0, -delta.y) * vitesseDeplacement * Time.deltaTime;
            transform.Translate(mouvement, Space.World);
        }

        if (Input.GetMouseButtonUp(0))
        {
            estEnTrainDeGlisser = false;
        }

        // Zoom avec la molette de la souris
        float zoom = Input.GetAxis("Mouse ScrollWheel") * vitesseZoom;
        if (zoom != 0)
        {
            Camera camera = GetComponent<Camera>();
            camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - zoom, limiteZoomMin, limiteZoomMax);
        }
    }
}
