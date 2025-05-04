using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TemplateCommandeCuisinier : MonoBehaviour
{
    public TMP_Text Titre;
    public TMP_Text Date;
    public TMP_Text Client;

    public TMP_Dropdown Options;

    public int idCommande;

    private void Start()
    {
        Options.onValueChanged.AddListener(
            delegate
            {
                OnStatutChanged();
            }
        );
    }

    void OnStatutChanged()
    {
        string nouveauStatut = Options.options[Options.value].text;
        StartCoroutine(UpdateStatutCommande(nouveauStatut));
    }

    IEnumerator UpdateStatutCommande(string nouveauStatut)
    {
        WWWForm form = new WWWForm();
        form.AddField("idCommande", idCommande);
        form.AddField("statut", nouveauStatut);

        using (WWW www = new WWW("http://localhost/livinparis/modifier_statut_commande.php", form))
        {
            yield return www;

            if (www.text != "0")
            {
                Debug.LogError("Erreur lors de la mise à jour du statut : " + www.text);
            }
        }
    }
}
