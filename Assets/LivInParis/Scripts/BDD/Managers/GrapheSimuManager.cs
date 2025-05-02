using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GrapheSimuManager : MonoBehaviour
{
    public TextMeshProUGUI adresseDepart;
    public TextMeshProUGUI adresseArrivee;
    public TextMeshProUGUI tempsTotal;

    public VisuelSimu visuelScript;

    string depart;
    string arrivee;

    void Start()
    {
        CallAdresses();
    }

    public void CallAdresses()
    {
        StartCoroutine(RécupérerAdresses());
    }

    IEnumerator RécupérerAdresses()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", DBManager.nomutilisateur);

        WWW www = new WWW("http://localhost/livinparis/recuperer_adresses.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            string[] adresses = www.text.Split('\t');

            if (adresses.Length > 0)
            {
                adresseDepart.text = "Adresse de départ: " + adresses[1];
                depart = adresses[1];
                adresseArrivee.text = "Adresse d'arrivée: " + adresses[2];
                arrivee = adresses[2];
                visuelScript.InitialiserRechercheStations(depart, arrivee);
            }
            else
            {
                Debug.Log("Aucune adresse trouvée.");
            }
        }
        else
        {
            Debug.Log("Echec de la récupération des adresses. Erreur#" + www.text);
        }
    }

    private void Update()
    {
        tempsTotal.text = "Temps d'attente  : " + (int)visuelScript.tempsTotal + " minutes";
    }
}
