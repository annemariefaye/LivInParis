using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class NoteCuisinierManager : MonoBehaviour
{
    public EtoileManager etoileManager;
    public TMP_InputField commentaire;

    public void CallNotation()
    {
        StartCoroutine(Notation());
    }

    IEnumerator Notation()
    {
        WWWForm form = new WWWForm();
        form.AddField("idCuisinier", DBManager.idCuisinierNote);
        form.AddField("note", etoileManager.note);
        form.AddField("commentaire", commentaire.text);

        WWW www = new WWW("http://localhost/livinparis/notation.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Note Envoyée");
            SceneManager.LoadScene(11);
        }
        else
        {
            Debug.Log("Echec de la notation : " + www.text);
        }
    }
}
