using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TemplateAdminCuisinier: MonoBehaviour
{
    public TMP_Text Nom;
    public TMP_Text PlatDuJour;
    public Button SupprimerButton;

    public int idCuisinier;
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
                Debug.Log("Cuisinier supprimé avec succès");
            }
            else
            {
                Debug.LogError("Erreur lors de la suppression : " + www.text);
            }
        }
    }
}
