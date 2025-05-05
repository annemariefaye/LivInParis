using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TemplateAdminClient : MonoBehaviour
{
    public TMP_Text Nom;
    public TMP_Text Adresse;
    public TMP_Text Montant;
    public Button SupprimerButton;

    public string NomUtilisateur;

    private void Start()
    {
        SupprimerButton.onClick.AddListener(() => StartCoroutine(SupprimerCuisinier()));
    }

    IEnumerator SupprimerCuisinier()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", NomUtilisateur);

        using (WWW www = new WWW("http://localhost/livinparis/supprimer_client.php", form))
        {
            yield return www;

            if (www.text == "0")
            {
                Destroy(gameObject);
                Debug.Log("Client supprimé avec succès");
            }
            else
            {
                Debug.Log("Erreur lors de la suppression : " + www.text);
            }
        }
    }
}
