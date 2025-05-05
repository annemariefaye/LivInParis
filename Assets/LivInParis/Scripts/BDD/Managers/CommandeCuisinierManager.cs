using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Plat
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
    public string[] Categories;
    public float NoteMoyenne;
    public float Frequence;
}

[System.Serializable]
public class PlatList
{
    public List<Plat> plats;
}

public class CommandeCuisinierManager : MonoBehaviour
{
    public GameObject templatePlatPrefab;
    public Transform parentContainer;

    void Start()
    {
        StartCoroutine(ChargerPlatsUtilisateur(DBManager.nomutilisateur));
    }

    IEnumerator ChargerPlatsUtilisateur(string nomUtilisateur)
    {
        WWWForm form = new WWWForm();
        form.AddField("NomUtilisateur", nomUtilisateur);

        WWW www = new WWW("http://localhost/livinparis/recuperer_plats_mode_cuisinier.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            string json = www.text.Substring(1);
            json = "{\"plats\":" + json + "}";

            PlatList platList = JsonUtility.FromJson<PlatList>(json);

            foreach (Plat plat in platList.plats)
            {
                GameObject platItem = Instantiate(templatePlatPrefab, parentContainer);
                TemplatePlatCuisinier script = platItem.GetComponent<TemplatePlatCuisinier>();

                script.Init(plat.IdPlat);

                script.titreText.text = plat.Nom;
                script.prixText.text = plat.Prix + "€";
                script.descriptionText.text =
                    plat.Type + " - " + plat.Regime + " - " + plat.Nationalite;
                script.noteText.text = plat.NoteMoyenne.ToString("0.0");
                script.nombreServisText.text = "Plat servis "  + plat.Frequence + " fois";

                Sprite img = Resources.Load<Sprite>(plat.CheminAccesPhoto);
                if (img != null)
                {
                    script.platImage.sprite = img;
                }
            }
        }
        else
        {
            Debug.Log("Erreur pour afficher les plats : " + www.text);
        }
    }
}
