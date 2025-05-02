using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreerCuisinier
{
    public static IEnumerator Creation(string PDJ, string nu)
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", nu);
        form.AddField("pdj", PDJ);

        WWW www = new WWW("http://localhost/livinparis/creercuisinier.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Cuisinier créé avec succès");
        }
        else
        {
            Debug.Log("Erreur dans la création du cuisinier. Erreur # " + www.text);
        }
    }
}
