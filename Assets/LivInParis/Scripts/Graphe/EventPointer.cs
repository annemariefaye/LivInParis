using System.Collections;
using System.Collections.Generic;
using Mapbox.Examples;
using Mapbox.Utils;
using PbSI;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Classe permettant de gérer les événements liés aux pointeurs dans le graphe.
/// </summary>
public class EventPointer : MonoBehaviour
{
    #region Attributs
    /// <summary>
    /// Panneau actuellement actif.
    /// </summary>
    public static GameObject panneauActif;

    /// <summary>
    /// Vitesse de rotation du pointeur.
    /// </summary>
    [SerializeField]
    float rotationSpeed = 50f;

    /// <summary>
    /// Amplitude du mouvement de flottement.
    /// </summary>
    [SerializeField]
    float amplitude = 2.0f;

    /// <summary>
    /// Fréquence du mouvement de flottement.
    /// </summary>
    [SerializeField]
    float frequency = 0.50f;

    LocationStatus playerLocation;

    /// <summary>
    /// Position de l'événement en coordonnées géographiques.
    /// </summary>
    [SerializeField]
    public Vector2d eventPos;

    /// <summary>
    /// Noeud associé à l'événement.
    /// </summary>
    public Noeud<StationMetro> noeud;

    /// <summary>
    /// Texte de l'interface utilisateur.
    /// </summary>
    Text uiText;

    /// <summary>
    /// Panneau d'information.
    /// </summary>
    GameObject infoPanel;

    /// <summary>
    /// Groupe de canvas pour gérer la transparence.
    /// </summary>
    CanvasGroup canvasGroup;
    #endregion

    #region Méthodes
    /// <summary>
    /// Initialisation des composants.
    /// </summary>
    void Start()
    {
        infoPanel = GameObject.Find("InfoPanel");
        uiText = GameObject.Find("LibelleText").GetComponent<Text>();
        canvasGroup = infoPanel.GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// Mise à jour des animations et interactions.
    /// </summary>
    void Update()
    {
        FloatAndRotatePointer();
    }

    /// <summary>
    /// Fait flotter et tourner le pointeur.
    /// </summary>
    void FloatAndRotatePointer()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude + 15,
            transform.position.z
        );
    }

    /// <summary>
    /// Gère le clic sur le pointeur.
    /// </summary>
    private void OnMouseDown()
    {
        playerLocation = GameObject.Find("Canvas").GetComponent<LocationStatus>();
        var currentPlayerLocation = new GeoCoordinatePortable.GeoCoordinate(
            playerLocation.GetLocationLat(),
            playerLocation.GetLocationLong()
        );
        var eventLocation = new GeoCoordinatePortable.GeoCoordinate(eventPos[0], eventPos[1]);

        var distance = currentPlayerLocation.GetDistanceTo(eventLocation);

        if (panneauActif != null && panneauActif != infoPanel)
        {
            StartCoroutine(FadeOutStatic(panneauActif.GetComponent<CanvasGroup>()));
        }

        panneauActif = infoPanel;
        infoPanel.SetActive(true);
        uiText.text =
            "Libellé : " + noeud.Contenu.Libelle + "\nDistance : " + ((int)distance) + " m";
        infoPanel.transform.position = Camera.main.WorldToScreenPoint(
            transform.position + new Vector3(0, 10, 0)
        );

        StartCoroutine(FadeIn(canvasGroup));
    }

    /// <summary>
    /// Fait disparaître un panneau avec une animation de fondu.
    /// </summary>
    /// <param name="canvasGroup">Groupe de canvas à faire disparaître.</param>
    /// <returns>Coroutine.</returns>
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

    /// <summary>
    /// Fait apparaître un panneau avec une animation de fondu.
    /// </summary>
    /// <param name="canvasGroup">Groupe de canvas à faire apparaître.</param>
    /// <returns>Coroutine.</returns>
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
    #endregion
}
