using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AdminCuisinierManager : MonoBehaviour
{
    public GameObject templatePrefab;
    public Transform contentPanel;

    void Start()
    {
        StartCoroutine(ChargerCuisiniers());
    }

    IEnumerator ChargerCuisiniers()
    {
        using (WWW www = new WWW("http://localhost/livinparis/recuperer_cuisiniers.php"))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogError("Erreur de récupération : " + www.error);
                yield break;
            }

            string[] lignes = www.text.Split('\n');

            foreach (string ligne in lignes)
            {
                if (string.IsNullOrWhiteSpace(ligne))
                    continue;

                string[] donnees = ligne.Split('\t');
                if (donnees.Length < 4)
                    continue;

                GameObject obj = Instantiate(templatePrefab, contentPanel);
                TemplateAdminCuisinier template = obj.GetComponent<TemplateAdminCuisinier>();

                template.idCuisinier = int.Parse(donnees[0]);
                template.NomUtilisateur = donnees[1];
                template.Nom.text = donnees[2] + " " + donnees[3];
                template.PlatDuJour.text = donnees[4];
            }
        }
    }
}
