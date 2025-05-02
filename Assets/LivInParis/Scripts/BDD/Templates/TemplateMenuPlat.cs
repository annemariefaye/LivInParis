using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using TMPro;

public class TemplateMenuPlat : MonoBehaviour
{
    public Image platBouton;

    public TMP_Text Titre;
    public TMP_Text Cuisinier;
    public TMP_Text Note;
    public Image Etoile;
    public TMP_Text Description;
    public TMP_Text Prix;
    public TMP_Text Adresse;

    public TextMeshProUGUI QuantiteText;
    public Button PlusButton;
    public Button MoinsButton;

    private int quantite = 0;

    public int IdPlat { get; private set; }

    void Start()
    {
        UpdateQuantiteText();

        PlusButton.onClick.AddListener(() =>
        {
            quantite++;
            UpdateQuantiteText();
            UpdateQuantiteDansDBManager();
        });

        MoinsButton.onClick.AddListener(() =>
        {
            quantite = Mathf.Max(0, quantite - 1);
            UpdateQuantiteText();
            UpdateQuantiteDansDBManager();
        });
    }

    void UpdateQuantiteText()
    {
        QuantiteText.text = quantite.ToString();
    }

    public void Init(int idPlat, int quantiteInitiale)
    {
        IdPlat = idPlat;
        quantite = quantiteInitiale;
        UpdateQuantiteText();
        UpdateQuantiteDansDBManager();
    }

    public int GetQuantite()
    {
        return quantite;
    }

    private void UpdateQuantiteDansDBManager()
    {
        if (DBManager.quantitesDansPanier.ContainsKey(IdPlat))
        {
            DBManager.quantitesDansPanier[IdPlat] = quantite;
        }
        else
        {
            DBManager.quantitesDansPanier.Add(IdPlat, quantite);
        }
    }

}
