using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PbSI
{
    public class ExportRequeteSelect : MonoBehaviour
    {
        /// <summary>
        /// Référence à la fenêtre pop-up d'information.
        /// </summary>
        public GameObject popUp;

        /// <summary>
        /// Référence au champ de texte affichant les messages dans la pop-up.
        /// </summary>
        public TextMeshProUGUI popUpText;

        /// <summary>
        /// Lance l’export des résultats de plusieurs requêtes SQL.
        /// </summary>
        public void CallExport()
        {
            List<string> requetes = new List<string>
            {
                "SELECT * FROM Utilisateur",
                "SELECT * FROM Commande",
                "SELECT * FROM LigneDeCommande",
                "SELECT * FROM Plat",
                "SELECT * FROM Livraison"
            };

            for (int i = 0; i < requetes.Count; i++)
            {
                string nomFichier = "export" + i;
                StartCoroutine(ExporterResultatRequete(nomFichier, requetes[i]));
            }

            popUp.SetActive(true);
        }

        /// <summary>
        /// Coroutine qui envoie une requête SQL au serveur et enregistre le résultat dans un fichier.
        /// </summary>
        /// <param name="nomFichier">Nom du fichier de destination.</param>
        /// <param name="requete">Requête SQL à exécuter.</param>
        IEnumerator ExporterResultatRequete(string nomFichier, string requete)
        {
            WWWForm form = new WWWForm();
            form.AddField("nomFichier", nomFichier);
            form.AddField("requeteSQL", requete);

            WWW www = new WWW("http://localhost/livinparis/exporter_resultat.php", form);
            yield return www;

            if (www.text == "0")
            {
                popUpText.text = "Export valide !";
                Debug.Log("Export " + nomFichier + " : " + www.text);
            }
            else
            {
                popUpText.text = "Erreur de l'export !";
                Debug.Log("Erreur lors de l'export : " + www.text);
            }
        }

        /// <summary>
        /// Ferme la fenêtre pop-up.
        /// </summary>
        public void RemovePopUp()
        {
            popUp.SetActive(false);
        }
    }
}
