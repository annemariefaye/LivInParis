using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using TMPro;

public class CuisinierCommandesManager : MonoBehaviour
{
    public GameObject templateCommandePrefab;
    public Transform contentPanel;

    void Start()
    {
        StartCoroutine(ChargerCommandes());
    }

    IEnumerator ChargerCommandes()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", DBManager.nomutilisateur);

        using (WWW www = new WWW("http://localhost/livinparis/recuperer_commandes_cuisinier.php", form))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Erreur lors de la récupération des plats : " + www.error);
                yield break;
            }

            string[] lignes = www.text.Split('\n');

            foreach (string ligne in lignes)
            {
                if (string.IsNullOrWhiteSpace(ligne))
                {
                    continue;
                }

                string[] donnees = ligne.Split('\t');

                if (donnees.Length < 6)
                {
                    continue;
                }

                GameObject newCommande = Instantiate(templateCommandePrefab, contentPanel);
                TemplateCommandeCuisinier template = newCommande.GetComponent<TemplateCommandeCuisinier>();

                template.Titre.text = donnees[0];
                DateTime date = DateTime.Parse(donnees[1]);
                template.Date.text = date.ToString("dd/MM/yyyy");
                template.Client.text = donnees[2] + " " + donnees[3];
                TMP_Dropdown dropdown = template.GetComponentInChildren<TMP_Dropdown>();
                int index = dropdown.options.FindIndex(option => option.text == donnees[4]);
                if (index >= 0)
                {
                    dropdown.value = index;
                    dropdown.RefreshShownValue();
                }

                template.idCommande = int.Parse(donnees[5]);
            }
        }
    }
}
