using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CommandeNationaliteClientManager : MonoBehaviour
{
    public GameObject templateCommandePrefab;
    public Transform contentPanel;
    public TMP_InputField dateDebut;
    public TMP_InputField dateFin;
    public TMP_InputField nationalite;
    public TMP_InputField nomutilisateur;

    public void Tri()
    {
        StartCoroutine(ChargerCommandes());
    }

    IEnumerator ChargerCommandes()
    {
        WWWForm form = new WWWForm();
        form.AddField("dateDebut", dateDebut.text);
        form.AddField("dateFin", dateFin.text);
        form.AddField("nationalite", nationalite.text);
        form.AddField("nomutilisateur", nomutilisateur.text);

        using (
            WWW www = new WWW(
                "http://localhost/livinparis/recuperer_commandes_par_nationalite_nu.php",
                form
            )
        )
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
                Debug.Log(string.Join(", ", donnees));

                if (donnees.Length < 5)
                {
                    continue;
                }

                GameObject newCommande = Instantiate(templateCommandePrefab, contentPanel);
                TemplateCommandeAdmin template = newCommande.GetComponent<TemplateCommandeAdmin>();

                template.Titre.text = donnees[0];
                DateTime date = DateTime.Parse(donnees[1]);
                template.Date.text = date.ToString("dd/MM/yyyy");
                template.Identite.text = donnees[2] + " " + donnees[3];
                template.Statut.text = donnees[4];
            }
        }
    }
}
