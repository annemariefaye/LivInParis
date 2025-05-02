using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using TMPro;
using System;
using System.Linq;

public class CreerPlat : MonoBehaviour
{
    public TMP_InputField nomField;
    public TMP_InputField prixField;
    public TMP_Dropdown typeField;
    public TMP_InputField personnesField;
    public TMP_InputField dateFabField;
    public TMP_InputField datePerField;
    public TMP_Dropdown regimeField;
    public TMP_Dropdown recetteField;
    public TMP_InputField nationaliteField;
    public TMP_InputField proteinesField;

    public Button submitButton;

    private string apiUrl = "http://localhost/livinparis/recuperer_recettes.php";

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(15);
    }

    private void Start()
    {
        StartCoroutine(FillRecetteDropdown());
    }


    IEnumerator FillRecetteDropdown()
    {
        WWW www = new WWW(apiUrl);
        yield return www;

        if (www.text[0] == '0')
        {
            string[] recettes = www.text.Split('\t').Where(r => !string.IsNullOrEmpty(r) && r != "0").ToArray();
            recetteField.ClearOptions();
            recetteField.AddOptions(recettes.ToList());
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des recettes : " + www.text);
        }
    }

    public void CallPlat()
    {
        StartCoroutine(Plat());
    }


    IEnumerator Plat()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", DBManager.nomutilisateur);
        form.AddField("nom", nomField.text);
        form.AddField("prix", prixField.text);
        form.AddField("type", typeField.options[typeField.value].text);
        form.AddField("personnes", personnesField.text);
        form.AddField("dateFabrication", dateFabField.text);
        form.AddField("datePeremption", datePerField.text);
        form.AddField("recette", recetteField.options[recetteField.value].text);
        form.AddField("regime", regimeField.options[regimeField.value].text);
        form.AddField("chemin", "default");
        form.AddField("nationalite", nationaliteField.text);
        form.AddField("proteines", proteinesField.text);

        WWW www = new WWW("http://localhost/livinparis/creer_plat.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Plat créé avec succès");
        }
        else
        {
            Debug.Log("Erreur dans la création du plat. Erreur # " + www.text);
        }

        //SceneManager.LoadScene(0);

    }

    bool ValidateNumeric(string input)
    {
        decimal result;
        return decimal.TryParse(input, out result) && result >= 0;
    }

    public void VerifyInputs()
    {
        bool isNomValid = nomField.text.Length > 0;
        bool isPrixValid = ValidateNumeric(prixField.text);
        bool isPersonnesValid = ValidateNumeric(personnesField.text);
        bool isProteinesValid = ValidateNumeric(proteinesField.text);
        bool isDateFabValid = DateTime.TryParse(dateFabField.text.Trim(), out _);
        bool isDatePerValid = DateTime.TryParse(datePerField.text.Trim(), out _);
        bool isNationaliteValid = nationaliteField.text.Length > 0;

        submitButton.interactable = (
             isNomValid &&
             isPrixValid &&
             isPersonnesValid &&
             isProteinesValid &&
             isDateFabValid &&
             isDatePerValid &&
             isNationaliteValid
        );
    }
}
