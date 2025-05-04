using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeConnecter : MonoBehaviour
{
    public TMP_InputField nomUtilisateurField;
    public TMP_InputField mdpField;
    public GameObject erreur;

    public Button submitButton;

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(11);
    }

    public void CallLogin()
    {
        StartCoroutine(Connecter());
    }

    IEnumerator Connecter()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", nomUtilisateurField.text);
        form.AddField("mdp", mdpField.text);

        WWW www = new WWW("http://localhost/livinparis/connecter.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            erreur.SetActive(false);

            DBManager.nomutilisateur = nomUtilisateurField.text;
            DBManager.email = www.text.Split('\t')[1];
            DBManager.nom = www.text.Split('\t')[2];
            DBManager.prenom = www.text.Split('\t')[3];
            DBManager.adresse = www.text.Split('\t')[4];
            DBManager.telephone = www.text.Split('\t')[5];
            DBManager.fidelite = int.Parse(www.text.Split('\t')[6]);

            DBManager.platDuJour = string.IsNullOrEmpty(www.text.Split('\t')[7])
                ? null
                : www.text.Split('\t')[7];
            DBManager.nomEntreprise = string.IsNullOrEmpty(www.text.Split('\t')[8])
                ? null
                : www.text.Split('\t')[8];

            DBManager.idClient = www.text.Split('\t')[9] == "null" ? (int?)null : int.Parse(www.text.Split('\t')[9]);
            DBManager.idCuisinier = www.text.Split('\t')[10] == "null" ? (int?)null : int.Parse(www.text.Split('\t')[10]);

            if (DBManager.nomutilisateur == "administrateur")
            {
                SceneManager.LoadScene(12);
            }

            else if (DBManager.idCuisinier == null && DBManager.idClient != null)
            {
                SceneManager.LoadScene(5);
            }
            else if (DBManager.idClient != null && DBManager.idCuisinier != null)
            {
                SceneManager.LoadScene(9);
            }
        }
        else
        {
            erreur.SetActive(true);
            Debug.Log("Echec de la connexion. Erreur#" + www.text);

        }
    }

    public void VerifyInputs()
    {
        bool isNomUtilisateurValid = nomUtilisateurField.text.Length >= 4;
        bool isMDPValid = mdpField.text.Length >= 8;

        submitButton.interactable = (isNomUtilisateurValid && isMDPValid);
    }
}
