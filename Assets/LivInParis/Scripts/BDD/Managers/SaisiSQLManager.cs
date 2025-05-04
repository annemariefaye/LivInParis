using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SimpleJSON;

public class SaisiSQLManager : MonoBehaviour
{
    public TextMeshProUGUI resultat;
    public TMP_InputField requete;


    public void CallRequete()
    {
        StartCoroutine(Requete());
    }

    IEnumerator Requete()
    {
        WWWForm form = new WWWForm();
        form.AddField("requete", requete.text);

        WWW www = new WWW("http://localhost/livinparis/saisi_sql.php", form);
        yield return www;

        if (www.text[0] == '0')
        {
            string json = www.text.Substring(1);
            var parsed = JSON.Parse(json);

            if (!parsed.IsArray)
            {
                resultat.text = "Résultat illisible";
                yield break;
            }

            var rows = parsed.AsArray;

            if (rows.Count == 0)
            {
                resultat.text = "Aucune donnée";
                yield break;
            }

            List<string> colonnes = new List<string>();
            foreach (KeyValuePair<string, JSONNode> kvp in rows[0].AsObject)
                colonnes.Add(kvp.Key);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine(string.Join("\t", colonnes));
            sb.AppendLine(new string('-', 80));

            foreach (JSONNode row in rows)
            {
                List<string> valeurs = new List<string>();
                foreach (string col in colonnes)
                    valeurs.Add(row[col]);
                sb.AppendLine(string.Join("\t", valeurs));
            }

            resultat.text = sb.ToString();
        }
        else
        {
            resultat.text = "Echec de la saisi sql. Erreur#" + www.text;
        }
    }

}
