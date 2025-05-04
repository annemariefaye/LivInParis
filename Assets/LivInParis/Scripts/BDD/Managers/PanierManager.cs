using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanierManager : MonoBehaviour
{
    public GameObject templatePanierPrefab;
    public Transform parentContainer;

    public Button validerButton;
    public GameObject fidelite;
    public TMP_Text fideliteText;
    public TMP_Text texteTotal;

    public TMP_Text fideliteHeader;

    private Dictionary<int, int> quantitesDansPanier => DBManager.quantitesDansPanier;
    private List<TemplatePanier> tousLesTemplates = new List<TemplatePanier>();

    float total = 0f;

    void Start()
    {
        ChargerPanier();
        CalculerTotal();

        if (DBManager.fidelite >= 100)
        {
            fidelite.SetActive(true);
            fideliteText.text = "Utiliser mes points";
        }
        else
        {
            fidelite.SetActive(false);
            fideliteText.text = "Utiliser mes points";
        }

    }

    public void UtiliserPoints()
    {
        if (!DBManager.fideliteActivee && DBManager.fidelite >= 100)
        {
            float prixMax = float.MinValue;
            TemplatePanier platOffert = null;

            foreach (TemplatePanier template in tousLesTemplates)
            {
                if (quantitesDansPanier[template.idPlat] > 0 &&
                    float.TryParse(template.prixText.text.Replace("€", ""), NumberStyles.Float, CultureInfo.InvariantCulture, out float prix))
                {
                    if (prix > prixMax)
                    {
                        prixMax = prix;
                        platOffert = template;
                    }
                }
            }

            if (platOffert != null)
            {
                DBManager.fidelite -= 100;
                DBManager.fideliteActivee = true;
                CalculerTotal();
                fideliteHeader.text = "Vous avez " + DBManager.fidelite + " points";
                fideliteText.text = "Retirer mes points";
            }
        }
        else if (DBManager.fideliteActivee)
        {
            DBManager.fidelite += 100;
            DBManager.fideliteActivee = false;
            CalculerTotal();
            fideliteHeader.text = "Vous avez " + DBManager.fidelite + " points";
            fideliteText.text = "Utiliser mes points";
        }
    }



    void ChargerPanier()
    {
        foreach (var kvp in DBManager.quantitesDansPanier)
        {
            int idPlat = kvp.Key;
            int quantite = kvp.Value;

            PlatData platData = TrouverPlatParId(idPlat);
            if (platData != null && DBManager.quantitesDansPanier[idPlat] > 0)
            {
                AfficherPlatDansPanier(platData, quantite);
            }
        }
    }

    public void CalculerTotal()
    {
        total = 0f;
        DBManager.idPlatOffert = -1;

        foreach (var kvp in quantitesDansPanier)
        {
            int idPlat = kvp.Key;
            int quantite = kvp.Value;

            PlatData platData = TrouverPlatParId(idPlat);
            if (
                platData != null
                && float.TryParse(
                    platData.Prix,
                    NumberStyles.Float,
                    CultureInfo.InvariantCulture,
                    out float prixUnitaire
                )
            )
            {
                total += prixUnitaire * quantite;
            }
        }

        float totalAffiche = total;

        if (DBManager.fideliteActivee)
        {
            float prixMax = float.MinValue;

            foreach (var kvp in quantitesDansPanier)
            {
                if (kvp.Value > 0)
                {
                    PlatData platData = TrouverPlatParId(kvp.Key);
                    if (platData != null &&
                        float.TryParse(platData.Prix, NumberStyles.Float, CultureInfo.InvariantCulture, out float prix))
                    {
                        if (prix > prixMax)
                        {
                            prixMax = prix;
                            DBManager.idPlatOffert = kvp.Key;
                        }
                    }
                }
            }

            if (prixMax != float.MinValue)
            {
                totalAffiche -= prixMax;
            }
        }

        texteTotal.text = $"Total : {totalAffiche} € . Valider commande";
    }


    PlatData TrouverPlatParId(int idPlat)
    {
        foreach (PlatData plat in MenuPlatManager.Instance.tousLesPlats)
        {
            if (plat.IdPlat == idPlat)
            {
                return plat;
            }
        }
        return null;
    }

    void AfficherPlatDansPanier(PlatData platData, int quantite)
    {
        GameObject panierItem = Instantiate(templatePanierPrefab, parentContainer);
        TemplatePanier panierScript = panierItem.GetComponent<TemplatePanier>();
        tousLesTemplates.Add(panierScript);

        panierScript.Init(platData.IdPlat, quantite);

        panierScript.titreText.text = platData.Nom;
        panierScript.prixText.text = platData.Prix + "€";
        panierScript.quantiteText.text = quantite.ToString();
        panierScript.dateLivraisonInput.text = "";
        panierScript.adresseLivraisonInput.text = "";
        panierScript.inputIncorrectDate.SetActive(false);
        panierScript.inputIncorrectAdresse.SetActive(false);

        if (!string.IsNullOrEmpty(platData.CheminAccesPhoto))
        {
            Sprite img = Resources.Load<Sprite>(platData.CheminAccesPhoto);
            if (img != null)
                panierScript.platImage.sprite = img;
            else
                Debug.LogWarning("Image introuvable dans Resources : " + platData.CheminAccesPhoto);
        }

        panierScript.plusButton.onClick.AddListener(() =>
        {
            quantite++;
            panierScript.quantiteText.text = quantite.ToString();
            quantitesDansPanier[platData.IdPlat] = quantite;
            CalculerTotal();
        });

        panierScript.moinsButton.onClick.AddListener(() =>
        {
            quantite = Mathf.Max(0, quantite - 1);
            panierScript.quantiteText.text = quantite.ToString();
            quantitesDansPanier[platData.IdPlat] = quantite;
            CalculerTotal();
        });
    }

    public void ActiverBouton()
    {
        bool toutEstValide = false;

        foreach (TemplatePanier template in tousLesTemplates)
        {
            if (DBManager.quantitesDansPanier.ContainsKey(template.idPlat) &&
                DBManager.quantitesDansPanier[template.idPlat] > 0)
            {
                if (!template.dateValide || !template.adresseValide)
                {
                    validerButton.interactable = false;
                    return;
                }
                toutEstValide = true;
            }
        }

        validerButton.interactable = toutEstValide;
    }

    IEnumerator EnvoyerCommande()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", DBManager.nomutilisateur);

        int idPlatOffert = -1;
        if (DBManager.fideliteActivee)
        {
            idPlatOffert = DBManager.idPlatOffert;
        }

        int indexCommande = 0;
        foreach (TemplatePanier template in tousLesTemplates)
        {
            if (template.idPlat == idPlatOffert) continue;
            if (DBManager.quantitesDansPanier[template.idPlat] <= 0) continue;

            form.AddField("idPlat[]", template.idPlat);
            form.AddField("quantite[]", DBManager.quantitesDansPanier[template.idPlat]);
            form.AddField("dateLivraison[]", template.dateLivraisonInput.text);
            form.AddField("adresseLivraison[]", template.adresseLivraisonInput.text);
            indexCommande++;
        }

        WWW www = new WWW("http://localhost/livinparis/envoyer_commande.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Commande envoyée avec succès");
            CalculerTotal();
            if (!DBManager.fideliteActivee)
            {
                DBManager.fidelite += (int)total;
                StartCoroutine(DBManager.MettreAJourFidelite());
            }
            SceneManager.LoadScene(17);
        }
        else
        {
            Debug.Log("Erreur lors de l'envoi de la commande : " + www.text);
        }
    }

    public void CallEnvoyerCommande()
    {
        StartCoroutine(EnvoyerCommande());
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
            CallEnvoyerCommande();
        }
    }

    public void ViderPanier()
    {
        foreach (Transform child in parentContainer)
        {
            Destroy(child.gameObject);
        }

        tousLesTemplates.Clear();
        DBManager.quantitesDansPanier.Clear();
    }

    public void PropagerDate(string date)
    {
        for (int i = 1; i < tousLesTemplates.Count; i++)
        {
            tousLesTemplates[i].dateLivraisonInput.text = tousLesTemplates[0].dateLivraisonInput.text;
        }
    }

    public void PropagerAdresse(string adresse)
    {
        for (int i = 1; i < tousLesTemplates.Count; i++)
        {
            tousLesTemplates[i].adresseLivraisonInput.text = tousLesTemplates[0].adresseLivraisonInput.text;
        }
    }

    private void Update()
    {
        ActiverBouton();
    }
}
