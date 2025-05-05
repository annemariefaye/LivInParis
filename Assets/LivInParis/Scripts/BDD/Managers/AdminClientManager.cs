using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminClientManager : MonoBehaviour
{
    public GameObject templatePrefab;
    public Transform contentPanel;

    private List<GameObject> templatesInstancies = new List<GameObject>();

    void Start()
    {
        StartCoroutine(ChargerClientsNom());
    }

    public void CallChargerClientsNom()
    {
        StartCoroutine(ChargerClientsNom());
    }

    IEnumerator ChargerClientsNom()
    {
        SupprimerTemplatesExistants();

        using (WWW www = new WWW("http://localhost/livinparis/recuperer_clients_nom.php"))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
                yield break;

            string[] lignes = www.text.Split('\n');

            for (int i = 0; i < lignes.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lignes[i]))
                    continue;

                string[] donnees = lignes[i].Split('\t');
                if (donnees.Length < 5)
                    continue;

                GameObject obj = Instantiate(templatePrefab, contentPanel);
                templatesInstancies.Add(obj);

                TemplateAdminClient template = obj.GetComponent<TemplateAdminClient>();
                template.NomUtilisateur = donnees[0];
                template.Nom.text = donnees[1] + " " + donnees[2];
                template.Adresse.text = donnees[3];
                template.Montant.text = "Montant des achats : " + donnees[4];
            }
        }
    }

    public void CallChargerClientsAdresse()
    {
        StartCoroutine(ChargerClientsAdresse());
    }

    IEnumerator ChargerClientsAdresse()
    {
        SupprimerTemplatesExistants();

        using (WWW www = new WWW("http://localhost/livinparis/recuperer_clients_adresse.php"))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
                yield break;

            string[] lignes = www.text.Split('\n');

            for (int i = 0; i < lignes.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lignes[i]))
                    continue;

                string[] donnees = lignes[i].Split('\t');
                if (donnees.Length < 5)
                    continue;

                GameObject obj = Instantiate(templatePrefab, contentPanel);
                templatesInstancies.Add(obj);

                TemplateAdminClient template = obj.GetComponent<TemplateAdminClient>();
                template.NomUtilisateur = donnees[0];
                template.Nom.text = donnees[1] + " " + donnees[2];
                template.Adresse.text = donnees[3];
                template.Montant.text = "Montant des achats : " + donnees[4];
            }
        }
    }

    public void CallChargerClientsMontant()
    {
        StartCoroutine(ChargerClientsMontant());
    }

    IEnumerator ChargerClientsMontant()
    {
        SupprimerTemplatesExistants();

        using (WWW www = new WWW("http://localhost/livinparis/recuperer_clients_montant.php"))
        {
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
                yield break;

            string[] lignes = www.text.Split('\n');

            for (int i = 0; i < lignes.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lignes[i]))
                    continue;

                string[] donnees = lignes[i].Split('\t');
                if (donnees.Length < 5)
                    continue;

                GameObject obj = Instantiate(templatePrefab, contentPanel);
                templatesInstancies.Add(obj);

                TemplateAdminClient template = obj.GetComponent<TemplateAdminClient>();
                template.NomUtilisateur = donnees[0];
                template.Nom.text = donnees[1] + " " + donnees[2];
                template.Adresse.text = donnees[3];
                template.Montant.text = "Montant des achats : " + donnees[4];
            }
        }
    }

    void SupprimerTemplatesExistants()
    {
        for (int i = 0; i < templatesInstancies.Count; i++)
        {
            Destroy(templatesInstancies[i]);
        }
        templatesInstancies.Clear();
    }
}
