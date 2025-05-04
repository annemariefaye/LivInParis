using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Enregistrer : MonoBehaviour
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


    public bool estCuisinier = false;
    public bool estClient = false;

    public Button submitButton;
    public Button returnButton;

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(11);
    }

    public void CallRegister()
    {
        StartCoroutine(Register());
    }

    public void ToggleCuisinier()
    {
        estCuisinier = !estCuisinier;
        ///Debug.Log("je suis cuisto : " + estCuisinier);
    }

    public void ToggleClient()
    {
        estClient = !estClient;
        ///Debug.Log("je suis client : " + estClient);
    }

    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", nomUtilisateurField.text);
        form.AddField("mdp", mdpField.text);
        form.AddField("email", emailField.text);
        form.AddField("nom", nomField.text);
        form.AddField("prenom", prenomField.text);
        form.AddField("adresse", adressField.text);
        form.AddField("telephone", telField.text);

        WWW www = new WWW("http://localhost/livinparis/enregistrer.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Utilisateur créé avec succès");
        }
        else
        {
            Debug.Log("Erreur dans la création de l'utilisateur. Erreur # " + www.text);
        }

        if (estCuisinier)
        {
            StartCoroutine(CreerCuisinier.Creation(cuisinierField.text, nomUtilisateurField.text));
        }

        if (estClient)
        {
            StartCoroutine(CreerClient.Creation(clientField.text, nomUtilisateurField.text));
        }

        SceneManager.LoadScene(11);
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
