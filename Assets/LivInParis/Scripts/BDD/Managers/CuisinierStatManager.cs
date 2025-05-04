using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuisinierStatManager : MonoBehaviour
{
    public GameObject templatePrefab;
    public Transform contentPanel;

    void Start()
    {
        StartCoroutine(ChargerClients());
    }

    IEnumerator ChargerClients()
    {
        using (WWW www = new WWW("http://localhost/livinparis/recuperer_cuisiniers_stat.php"))
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
                if (donnees.Length < 3)
                {
                    continue;
                }

                GameObject obj = Instantiate(templatePrefab, contentPanel);
                TemplateCuisinierStats template = obj.GetComponent<TemplateCuisinierStats>();

                template.Nom.text = donnees[0] + " " + donnees[1];
                template.Commande.text = "Nombre de commandes faites : " + donnees[2];
            }
        }
    }
}
