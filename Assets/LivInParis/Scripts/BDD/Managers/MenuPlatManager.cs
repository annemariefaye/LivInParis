using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlatData
{
    public int IdPlat;
    public string Nom;
    public string Prix;
    public string NomCuisinier;
    public string CheminAccesPhoto;
    public string Regime;
    public string Adresse;
    public string Type;
    public string Nationalite;
    public string Proteines;
    public List<string> Categories;
    public float NoteMoyenne;
}

[System.Serializable]
public class PlatDataList
{
    public PlatData[] plats;
}

public class MenuPlatManager : MonoBehaviour
{
    public GameObject templatePlatPrefab;
    public Transform parentContainer;
    public GameObject supp_button;

    private List<GameObject> platsAffiches = new List<GameObject>();
    [HideInInspector]
    public List<PlatData> tousLesPlats = new List<PlatData>();

    public static MenuPlatManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(GetPlats());
        LayoutRebuilder.ForceRebuildLayoutImmediate(parentContainer.GetComponent<RectTransform>());
    }

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(15);
    }
    IEnumerator GetPlats()
    {
        WWWForm form = new WWWForm();
        WWW www = new WWW("http://localhost/livinparis/recuperer_plats.php", form);
        yield return www;

        if (www.text[0] != '0')
        {
            Debug.LogError("Erreur serveur : " + www.text);
            yield break;
        }

        string json = "{\"plats\":" + www.text.Substring(1) + "}";
        PlatDataList dataList = JsonUtility.FromJson<PlatDataList>(json);

        tousLesPlats = new List<PlatData>(dataList.plats);
        AfficherPlats(tousLesPlats);
    }

    void AfficherPlats(List<PlatData> liste)
    {
        foreach (GameObject go in platsAffiches)
        {
            TemplateMenuPlat script = go.GetComponent<TemplateMenuPlat>();
            Destroy(go);
        }
        platsAffiches.Clear();

        foreach (PlatData plat in liste)
        {
            GameObject go = Instantiate(templatePlatPrefab, parentContainer);
            TemplateMenuPlat script = go.GetComponent<TemplateMenuPlat>();

            int quantite = DBManager.quantitesDansPanier.ContainsKey(plat.IdPlat) ? DBManager.quantitesDansPanier[plat.IdPlat] : 0;
            script.Init(plat.IdPlat, quantite);

            script.Titre.text = plat.Nom;
            script.Cuisinier.text = plat.NomCuisinier;
            script.Prix.text = plat.Prix + "€";
            script.Description.text = plat.Type + " - " + plat.Nationalite + " - " + plat.Regime + " - " + plat.Proteines + "g protéines";
            script.Adresse.text = plat.Adresse;
            script.Note.text = plat.NoteMoyenne.ToString("0.0");

            if (!string.IsNullOrEmpty(plat.CheminAccesPhoto))
            {
                Sprite img = Resources.Load<Sprite>(plat.CheminAccesPhoto);
                if (img != null)
                    script.platBouton.sprite = img;
                else
                    Debug.LogWarning("Image introuvable dans Resources : " + plat.CheminAccesPhoto);
            }

            platsAffiches.Add(go);
        }
    }

    public void FiltrerParCategorie(string categorie)
    {
        List<PlatData> filtres = tousLesPlats.FindAll(p => p.Categories.Contains(categorie));
        supp_button.SetActive(true);
        AfficherPlats(filtres);
    }

    public void SupprimerFiltre()
    {
        supp_button.SetActive(false);
        AfficherPlats(tousLesPlats);
    }
}
