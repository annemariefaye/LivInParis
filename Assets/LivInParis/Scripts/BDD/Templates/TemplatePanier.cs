using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PbSI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TemplatePanier : MonoBehaviour
{
    public Image platImage;
    public TMP_Text titreText;
    public TMP_Text prixText;
    public TMP_InputField dateLivraisonInput;
    public TMP_InputField adresseLivraisonInput;
    public TMP_Text quantiteText;
    public Button plusButton;
    public Button moinsButton;
    public GameObject inputIncorrectDate;
    public GameObject inputIncorrectAdresse;

    public bool synchroniserAvecDB = true;

    public bool dateValide = false;
    public bool adresseValide = false;

    private int quantite = 0;
    public int idPlat;
    public int idCommande;

    public void Init(int idPlatInitial, int quantiteInitiale)
    {
        idPlat = idPlatInitial;
        quantite = quantiteInitiale;
        UpdateQuantiteText();
    }

    public void InitComplete(int idPlatInitial, int idCommandeInitial, int quantiteInitiale)
    {
        idPlat = idPlatInitial;
        idCommande = idCommandeInitial;
        quantite = quantiteInitiale;
        UpdateQuantiteText();
    }

    void Start()
    {
        plusButton.onClick.AddListener(() =>
        {
            quantite++;
            UpdateQuantiteText();
            if (synchroniserAvecDB)
            {
                DBManager.quantitesDansPanier[idPlat] = quantite;
            }
        });

        moinsButton.onClick.AddListener(() =>
        {
            quantite = Mathf.Max(0, quantite - 1);
            UpdateQuantiteText();
            if (synchroniserAvecDB)
            {
                DBManager.quantitesDansPanier[idPlat] = quantite;
            }
        });
    }

    void UpdateQuantiteText()
    {
        quantiteText.text = quantite.ToString();
    }

    public int GetIdPlat() => idPlat;

    public int GetQuantite() => quantite;

    public void EstValide()
    {
        dateValide = DateTime.TryParseExact(
            dateLivraisonInput.text.Trim(),
            "dd/MM/yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out _
        );
        adresseValide = !string.IsNullOrWhiteSpace(adresseLivraisonInput.text);

        inputIncorrectDate.SetActive(!dateValide);
        inputIncorrectAdresse.SetActive(!adresseValide);
    }
}
