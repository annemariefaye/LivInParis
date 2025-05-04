using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModifierPanierManager : MonoBehaviour
{
    public GameObject templatePanierPrefab;
    public Transform parentContainer;
    public List<TemplatePanier> tousLesTemplates = new List<TemplatePanier>();
    public TextMeshProUGUI boutonResteAPayerText;
    public Button validerButton;

    private float totalDeBase = 0f;
    private float totalActuel = 0f;

    [System.Serializable]
    public class CommandePlatData
    {
        public int IdCommande;
        public int IdPlat;
        public string Nom;
        public float Prix;
        public string CheminAccesPhoto;
        public int Quantite;
        public string DateLivraison;
        public string AdresseLivraison;
    }

    [System.Serializable]
    public class CommandePlatDataList
    {
        public List<CommandePlatData> plats;
    }

    void Start()
    {
        StartCoroutine(ChargerCommandesEnAttente());
    }

    IEnumerator ChargerCommandesEnAttente()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", DBManager.nomutilisateur);

        WWW www = new WWW("http://localhost/livinparis/recuperer_commandes_en_attente.php", form);
        yield return www;

        if (www.text[0] != '0')
        {
            Debug.LogError("Erreur serveur : " + www.text);
            yield break;
        }

        string json = "{\"plats\":" + www.text.Substring(1) + "}";
        CommandePlatDataList dataList = JsonUtility.FromJson<CommandePlatDataList>(json);

        foreach (CommandePlatData plat in dataList.plats)
        {
            AfficherPlatDansPanier(plat);
        }

        CalculerTotal(true);
    }

    void AfficherPlatDansPanier(CommandePlatData platData)
    {
        if (platData.Quantite > 0)
        {
            GameObject panierItem = Instantiate(templatePanierPrefab, parentContainer);
            TemplatePanier panierScript = panierItem.GetComponent<TemplatePanier>();
            tousLesTemplates.Add(panierScript);

            panierScript.InitComplete(platData.IdPlat, platData.IdCommande, platData.Quantite);

            panierScript.synchroniserAvecDB = false;

            panierScript.titreText.text = platData.Nom;
            panierScript.prixText.text = platData.Prix.ToString("0.00") + "€";
            panierScript.quantiteText.text = platData.Quantite.ToString();
            panierScript.dateLivraisonInput.text = platData.DateLivraison;
            panierScript.adresseLivraisonInput.text = platData.AdresseLivraison;
            panierScript.inputIncorrectDate.SetActive(false);
            panierScript.inputIncorrectAdresse.SetActive(false);

            if (!string.IsNullOrEmpty(platData.CheminAccesPhoto))
            {
                Sprite img = Resources.Load<Sprite>(platData.CheminAccesPhoto);
                if (img != null)
                    panierScript.platImage.sprite = img;
                else
                    Debug.LogWarning(
                        "Image introuvable dans Resources : " + platData.CheminAccesPhoto
                    );
            }

            panierScript.plusButton.onClick.AddListener(() =>
            {
                platData.Quantite++;
                panierScript.quantiteText.text = platData.Quantite.ToString();
                CalculerTotal(false);
            });

            panierScript.moinsButton.onClick.AddListener(() =>
            {
                platData.Quantite = Mathf.Max(0, platData.Quantite - 1);
                panierScript.quantiteText.text = platData.Quantite.ToString();
                CalculerTotal(false);
            });
        }
    }

    void CalculerTotal(bool premierCalcul = false)
    {
        totalActuel = 0f;

        foreach (var template in tousLesTemplates)
        {
            int quantite = int.Parse(template.quantiteText.text);
            string prixString = template.prixText.text.Replace("€", "").Trim();

            if (!float.TryParse(prixString, out float prix))
            {
                prix = 0f;
            }

            totalActuel += quantite * prix;
        }

        if (premierCalcul)
        {
            totalDeBase = totalActuel;
        }

        if (totalActuel > totalDeBase)
        {
            boutonResteAPayerText.text =
                "Reste à payer : "
                + (totalActuel - totalDeBase).ToString("0.00", CultureInfo.InvariantCulture)
                + "€\nModifier commande";
        }
        else if (totalActuel < totalDeBase)
        {
            boutonResteAPayerText.text =
                "à rembourser : "
                + (totalDeBase - totalActuel).ToString("0.00", CultureInfo.InvariantCulture)
                + "€\nModifier commande";
        }
        else
        {
            boutonResteAPayerText.text = "Reste à payer : 0.00€\nModifier commande";
        }
    }

    void ActiverBouton()
    {
        bool toutEstValide = true;

        foreach (TemplatePanier template in tousLesTemplates)
        {
            if (!template.dateValide || !template.adresseValide)
            {
                toutEstValide = false;
            }
        }

        validerButton.interactable = toutEstValide;
    }

    public async void ChangerPage()
    {
        bool changer = true;
        foreach (TemplatePanier template in tousLesTemplates)
        {
            bool valide = await DBManager.AdresseValide(template.adresseLivraisonInput.text);
            template.inputIncorrectAdresse.SetActive(!valide);
            if (valide == false)
            {
                template.adresseLivraisonInput.text = "";
                changer = false;
            }
        }

        if (changer)
        {
            CallModifierCommande();
        }
    }

    IEnumerator ModifierCommande()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", DBManager.nomutilisateur);

        int indexCommande = 0;
        foreach (TemplatePanier template in tousLesTemplates)
        {
            form.AddField("idCommande[]", template.idCommande);
            form.AddField("idPlat[]", template.idPlat);
            form.AddField("quantite[]", template.quantiteText.text);
            form.AddField("dateLivraison[]", template.dateLivraisonInput.text);
            form.AddField("adresseLivraison[]", template.adresseLivraisonInput.text);
            indexCommande++;
        }

        WWW www = new WWW("http://localhost/livinparis/modifier_commande.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Commande envoyée avec succès");
            CalculerTotal(false);

            int difference = (int)(totalActuel - totalDeBase);
            DBManager.fidelite += difference;
            DBManager.fidelite = Mathf.Max(0, DBManager.fidelite);

            StartCoroutine(DBManager.MettreAJourFidelite());
            SceneManager.LoadScene(5);
        }
        else
        {
            Debug.Log("Erreur lors de la modification de la commande : " + www.text);
        }
    }

    public void CallModifierCommande()
    {
        StartCoroutine(ModifierCommande());
    }

    private void Update()
    {
        ActiverBouton();
    }
}
