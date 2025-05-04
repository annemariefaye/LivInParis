using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Modifier : MonoBehaviour
{
    public TMP_InputField nomUtilisateurField;
    public TMP_InputField mdpField;
    public TMP_InputField emailField;
    public TMP_InputField nomField;
    public TMP_InputField prenomField;
    public TMP_InputField adressField;
    public TMP_InputField telField;
    public TMP_InputField clientField;
    public TMP_InputField cuisinierField;

    public GameObject nomUtilisateurErreur;
    public GameObject mdpErreur;
    public GameObject emailErreur;
    public GameObject nomErreur;
    public GameObject prenomErreur;
    public GameObject adresseErreur;
    public GameObject telErreur;
    public GameObject clientErreur;
    public GameObject cuisinierErreur;


    public TextMeshProUGUI affichageUtilisateur;

    public bool estCuisinier = false;
    public bool estClient = false;

    public bool etaitCuisinier = false;
    public bool etaitClient = false;

    public Button submitButton;
    public Button deleteButton;

    public Toggle cuisiniercheckbox;
    public Toggle clientcheckbox;

    public void Start()
    {
        if (DBManager.Connecte)
        {
            affichageUtilisateur.text = "Veuillez modifier votre compte " + DBManager.prenom;
        }

        PreRemplirChamps();
    }

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(5);
    }

    public void ToggleCuisinier()
    {
        estCuisinier = !estCuisinier;
    }

    public void ToggleClient()
    {
        estClient = !estClient;
    }

    public void CallEdit()
    {
        StartCoroutine(Edit());
    }

    public void PreRemplirChamps()
    {
        nomUtilisateurField.text = DBManager.nomutilisateur;
        emailField.text = DBManager.email;
        nomField.text = DBManager.nom;
        prenomField.text = DBManager.prenom;
        adressField.text = DBManager.adresse;
        telField.text = DBManager.telephone;

        if (DBManager.platDuJour == null)
        {
            cuisinierField.text = "";
        }
        else
        {
            cuisinierField.text = DBManager.platDuJour;
            etaitCuisinier = true;
        }

        if (DBManager.nomEntreprise == null)
        {
            clientField.text = "";
        }
        else
        {
            clientField.text = DBManager.nomEntreprise;
            etaitClient = true;
        }
    }

    IEnumerator Edit()
    {
        WWWForm form = new WWWForm();
        form.AddField("newNomUtilisateur", nomUtilisateurField.text);
        form.AddField("nomutilisateur", DBManager.nomutilisateur);
        form.AddField("mdp", mdpField.text);
        form.AddField("newEmail", emailField.text);
        form.AddField("email", DBManager.email);
        form.AddField("nom", nomField.text);
        form.AddField("prenom", prenomField.text);
        form.AddField("adresse", adressField.text);
        form.AddField("telephone", telField.text);
        form.AddField("pdj", cuisinierField.text);
        form.AddField("nomentreprise", clientField.text);

        WWW www = new WWW("http://localhost/livinparis/modifier_utilisateur.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Utilisateur modifié avec succès");

            if (!etaitCuisinier && estCuisinier)
            {
                StartCoroutine(
                    CreerCuisinier.Creation(cuisinierField.text, DBManager.nomutilisateur)
                );
            }

            if (etaitCuisinier && !estCuisinier)
            {
                StartCoroutine(SupprimerCuisinier());
            }

            if (!etaitClient && estClient)
            {
                StartCoroutine(CreerClient.Creation(clientField.text, DBManager.nomutilisateur));
            }

            if (etaitClient && !estClient)
            {
                StartCoroutine(SupprimerClient());
            }
        }
        else
        {
            Debug.Log("Erreur dans la modification de l'utilisateur. Erreur # " + www.text);
        }
    }

    IEnumerator SupprimerCuisinier()
    {
        if (!string.IsNullOrEmpty(DBManager.platDuJour))
        {
            WWWForm form = new WWWForm();
            form.AddField("nomutilisateur", nomUtilisateurField.text);

            WWW www = new WWW("http://localhost/livinparis/supprimer_cuisinier.php", form);
            yield return www;

            if (www.text == "0")
            {
                Debug.Log("Cuisinier supprimé avec succès");
            }
            else
            {
                Debug.Log("Erreur lors de la suppression du cuisinier. Erreur#" + www.text);
            }
        }
    }

    IEnumerator SupprimerClient()
    {
        if (!string.IsNullOrEmpty(DBManager.nomEntreprise))
        {
            WWWForm form = new WWWForm();
            form.AddField("nomutilisateur", nomUtilisateurField.text);

            WWW www = new WWW("http://localhost/livinparis/supprimer_client.php", form);
            yield return www;

            if (www.text == "0")
            {
                Debug.Log("Client supprimé avec succès");
            }
            else
            {
                Debug.Log("Erreur lors de la suppression du client. Erreur# " + www.text);
            }
        }
    }

    public void SupprimerUtilisateur()
    {
        StartCoroutine(SupprimerUtilisateurCoroutine());
    }

    IEnumerator SupprimerUtilisateurCoroutine()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", nomUtilisateurField.text);

        WWW www = new WWW("http://localhost/livinparis/supprimer_utilisateur.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Utilisateur supprimé avec succès");
            SceneManager.LoadScene(0);
        }
        else
        {
            Debug.Log("Erreur lors de la suppression de l'utilisateur");
        }
    }

    public void VerifyInputs()
    {
        bool isNomUtilisateurValid = nomUtilisateurField.text.Length >= 4;
        bool isMDPValid = mdpField.text.Length >= 8;
        bool isEmailValid = Regex.IsMatch(emailField.text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        bool isNomValid = !string.IsNullOrWhiteSpace(nomField.text);
        bool isPrenomValid = !string.IsNullOrWhiteSpace(prenomField.text);
        bool isAdresseValid = !string.IsNullOrWhiteSpace(adressField.text);
        bool isTelValid = Regex.IsMatch(telField.text, @"^\d{10}$");

        bool cuistoValid = true;
        if (estCuisinier && string.IsNullOrWhiteSpace(cuisinierField.text))
        {
            cuistoValid = false;
        }

        nomUtilisateurErreur.SetActive(!isNomUtilisateurValid);
        mdpErreur.SetActive(!isMDPValid);
        emailErreur.SetActive(!isEmailValid);
        nomErreur.SetActive(!isNomValid);
        prenomErreur.SetActive(!isPrenomValid);
        adresseErreur.SetActive(!isAdresseValid);
        telErreur.SetActive(!isTelValid);
        cuisinierErreur.SetActive(!cuistoValid);

        submitButton.interactable = (
            isNomUtilisateurValid
            && isMDPValid
            && isEmailValid
            && isNomValid
            && isPrenomValid
            && isAdresseValid
            && isTelValid
            && cuistoValid
            && (estCuisinier || estClient)
        );
    }
}
