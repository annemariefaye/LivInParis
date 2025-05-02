using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreerClient
{
    public static IEnumerator Creation(string nomEntreprise, string nu)
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", nu);
        form.AddField("nomentreprise", nomEntreprise);

        WWW www = new WWW("http://localhost/livinparis/creerclient.php", form);
        yield return www;

        if (www.text == "0")
        {
            Debug.Log("Client créé avec succès");
        }
        else
        {
            Debug.Log("Erreur dans la création du client. Erreur # " + www.text);
        }
    }

}
