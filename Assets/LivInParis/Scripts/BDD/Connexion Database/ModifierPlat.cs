using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModifierPlat : MonoBehaviour
{
    public TMP_InputField nomInput;
    public TMP_InputField prixInput;
    public TMP_Dropdown typeInput;
    public TMP_InputField personnesInput;
    public TMP_InputField dateFabricationInput;
    public TMP_InputField datePeremptionInput;
    public TMP_InputField nationaliteInput;
    public TMP_InputField proteinesInput;
    public TMP_InputField cheminInput;
    public TMP_Dropdown recetteInput;
    public TMP_Dropdown regimeInput;
    public int idPlat;

    public Button modifierButton;

    public GameObject erreurNom;
    public GameObject erreurPrix;
    public GameObject erreurPersonnes;
    public GameObject erreurDateFab;
    public GameObject erreurDatePer;
    public GameObject erreurNationalite;
    public GameObject erreurProteines;
    public GameObject erreurChemin;

    void Start()
    {
        idPlat = DBManager.idPlatModif;
        StartCoroutine(FillRecetteDropdown());
    }

    public void ModifierPlatAction()
    {
        StartCoroutine(MettreAJourPlat());
        SceneManager.LoadScene(15);
    }

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(15);
    }

    IEnumerator FillRecetteDropdown()
    {
        WWW www = new WWW("http://localhost/livinparis/recuperer_recettes.php");
        yield return www;

        if (www.text[0] == '0')
        {
            string[] recettes = www
                .text.Split('\t')
                .Where(r => !string.IsNullOrEmpty(r) && r != "0")
                .ToArray();
            recetteInput.ClearOptions();
            recetteInput.AddOptions(recettes.ToList());
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des recettes : " + www.text);
        }
    }

    IEnumerator MettreAJourPlat()
    {
        WWWForm form = new WWWForm();
        form.AddField("idPlat", idPlat.ToString());
        form.AddField("nom", nomInput.text);
        form.AddField("prix", prixInput.text);
        form.AddField("type", typeInput.options[typeInput.value].text);
        form.AddField("personnes", personnesInput.text);
        form.AddField("dateFabrication", dateFabricationInput.text);
        form.AddField("datePeremption", datePeremptionInput.text);
        form.AddField("nationalite", nationaliteInput.text);
        form.AddField("proteines", proteinesInput.text);
        form.AddField("chemin", cheminInput.text);
        form.AddField("recette", recetteInput.options[recetteInput.value].text);
        form.AddField("regime", regimeInput.options[regimeInput.value].text);

        WWW www = new WWW("http://localhost/livinparis/modifier_plat.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Plat mis à jour avec succès");
        }
        else
        {
            Debug.LogError("Erreur lors de la mise à jour du plat : " + www.text);
        }
    }

    public void SupprimerPlatAction()
    {
        StartCoroutine(SupprimerPlat());
    }

    IEnumerator SupprimerPlat()
    {
        WWWForm form = new WWWForm();
        form.AddField("idPlat", idPlat.ToString());

        WWW www = new WWW("http://localhost/livinparis/supprimer_plat.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Plat supprimé avec succès");
        }
        else
        {
            Debug.LogError("Erreur lors de la suppression du plat : " + www.text);
        }
    }

    bool ValidateNumeric(string input)
    {
        decimal result;
        return decimal.TryParse(input, out result) && result >= 0;
    }

    public void VerifyInputs()
    {
        bool isNomValid = nomInput.text.Length > 0;
        bool isCheminValid = cheminInput.text.Length > 0;
        bool isPrixValid = ValidateNumeric(prixInput.text);
        bool isPersonnesValid = ValidateNumeric(personnesInput.text);
        bool isProteinesValid = ValidateNumeric(proteinesInput.text);
        bool isDateFabValid = DateTime.TryParse(dateFabricationInput.text.Trim(), out _);
        bool isDatePerValid = DateTime.TryParse(datePeremptionInput.text.Trim(), out _);
        bool isNationaliteValid = nationaliteInput.text.Length > 0;

        erreurNom.SetActive(!isNomValid);
        erreurPrix.SetActive(!isPrixValid);
        erreurPersonnes.SetActive(!isPersonnesValid);
        erreurDateFab.SetActive(!isDateFabValid);
        erreurDatePer.SetActive(!isDatePerValid);
        erreurNationalite.SetActive(!isNationaliteValid);
        erreurProteines.SetActive(!isProteinesValid);
        erreurChemin.SetActive(!isCheminValid);

        modifierButton.interactable = (
            isNomValid
            && isPrixValid
            && isPersonnesValid
            && isProteinesValid
            && isDateFabValid
            && isDatePerValid
            && isNationaliteValid
            && isCheminValid
        );
    }
}
