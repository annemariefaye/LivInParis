using UnityEngine;

public class SuivreJoueur : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 10, -10);
    public float smoothSpeed = 0.125f;
    public float vitesseZoom = 10f;
    public float limiteZoomMin = 10f;
    public float limiteZoomMax = 100f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

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
}
