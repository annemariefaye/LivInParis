using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapbox.Examples;
using Mapbox.Utils;
using UnityEngine.UI;
using PbSI;

public class EventPointer : MonoBehaviour
{
    public static GameObject panneauActif;  

    [SerializeField] float rotationSpeed = 50f;
    [SerializeField] float amplitude = 2.0f;
    [SerializeField] float frequency = 0.50f;

    LocationStatus playerLocation;
    [SerializeField] public Vector2d eventPos;

    public Noeud<StationMetro> noeud;

    Text uiText;
    GameObject infoPanel;
    CanvasGroup canvasGroup;

    void Start()
    {
        infoPanel = GameObject.Find("InfoPanel");
        uiText = GameObject.Find("LibelleText").GetComponent<Text>();
        canvasGroup = infoPanel.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void Update()
    {
        FloatAndRotatePointer(); 
    }

    void FloatAndRotatePointer()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude + 15, transform.position.z);
    }

    private void OnMouseDown()
    {
        playerLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
        var currentPlayerLocation = new GeoCoordinatePortable.GeoCoordinate(playerLocation.GetLocationLat(), playerLocation.GetLocationLong());
        var eventLocation = new GeoCoordinatePortable.GeoCoordinate(eventPos[0], eventPos[1]);

        var distance = currentPlayerLocation.GetDistanceTo(eventLocation);

        if (panneauActif != null && panneauActif != infoPanel)
        {
            StartCoroutine(FadeOutStatic(panneauActif.GetComponent<CanvasGroup>()));
        }

        panneauActif = infoPanel;
        infoPanel.SetActive(true);
        uiText.text = "Libellé : " + noeud.Contenu.Libelle + "\nDistance : " + ((int)distance) + " m";
        infoPanel.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 10, 0));

        StartCoroutine(FadeIn(canvasGroup));
    }

    public static IEnumerator FadeOutStatic(CanvasGroup canvasGroup)
    {
        float duration = 1f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    public IEnumerator FadeIn(CanvasGroup canvasGroup)
    {
        float duration = 1f;
        float timeElapsed = 0f;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}


