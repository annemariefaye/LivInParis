using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

namespace PbSI
{
    public class GrapheSimuManager : MonoBehaviour
    {
        public TextMeshProUGUI adresseDepart;
        public TextMeshProUGUI adresseArrivee;
        public TextMeshProUGUI idCommande;
        public TextMeshProUGUI tempsTotal;

        public VisuelSimu visuelScript;

        string depart;
        string arrivee;

        void Start()
        {
            CallAdresses();
        }

        public void Noter()
        {
            SceneManager.LoadScene(20);
        }

        public void CallAdresses()
        {
            StartCoroutine(RecupererAdresses());
        }

        IEnumerator RecupererAdresses()
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
                    DBManager.idCuisinierNote = int.Parse(adresses[3]);

                    if (!string.IsNullOrEmpty(adresses[4]))
                    {
                        string nomMusique = adresses[4].ToLower();
                        AudioClip clip = Resources.Load<AudioClip>("Musique/" + nomMusique);

                        if (clip != null)
                        {
                            AudioSource audioSource = GetComponent<AudioSource>();
                            audioSource.clip = clip;
                            audioSource.Play();
                        }
                        else
                        {
                            Debug.LogError("Musique introuvable dans Resources/Musique : " + nomMusique);
                        }
                    }
                    else
                    {
                        AudioClip clip = Resources.Load<AudioClip>("Musique/default");
                        if (clip != null)
                        {
                            AudioSource audioSource = GetComponent<AudioSource>();
                            audioSource.clip = clip;
                            audioSource.Play();
                        }
                        else
                        {
                            Debug.LogError("Musique par défaut introuvable !");
                        }
                    }

                    idCommande.text = "Commande " + adresses[5];
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
}

