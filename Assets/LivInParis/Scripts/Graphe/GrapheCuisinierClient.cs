using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using PbSI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

[System.Serializable]
public class Wrapper<T>
{
    public T[] items;
}

[System.Serializable]
public class Client
{
    public int Id;
    public string NU;
}

[System.Serializable]
public class Cuisinier
{
    public int Id;
    public string NU;
}


[System.Serializable]
public class CuisinierEtClient
{
    public int Id;
    public string NU;
}

[System.Serializable]
public class Relation
{
    public string ClientNU;
    public string CuisinierNU;
}

public class GrapheCuisinierClient : MonoBehaviour
{
    private Graphe<string> graphe;

    public GameObject nodePrefab;
    public Transform graphContainer;
    public Material lineMaterial;

    private Dictionary<int, RectTransform> noeudIdToRect = new Dictionary<int, RectTransform>();
    private Dictionary<int, string> noeudIdNomUtilisateur = new Dictionary<int, string>();

    int idCount = 0;


    void Start()
    {
        graphe = new Graphe<string>();
        StartCoroutine(ConstruireGraphe());
    }

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(12);
    }

    void Colorier()
    {
        Coloration<string> colorer = new Coloration<string>(graphe);
        colorer.WelshPowell();
        colorer.AfficherColoration();
    }

    IEnumerator ConstruireGraphe()
    {
        yield return StartCoroutine(ChargerClients());
        yield return StartCoroutine(ChargerCuisiniers());
        yield return StartCoroutine(ChargerCuisiniersEtClients());
        yield return StartCoroutine(CreerRelations());
        Colorier();
        AfficherGrapheDansContent();
    }

    IEnumerator ChargerClients()
    {
        string query = "SELECT IdClient, NomUtilisateur FROM Utilisateur WHERE IdClient IS NOT NULL AND IdCuisinier IS NULL;";
        WWWForm form = new WWWForm();
        form.AddField("requete", query);
        WWW www = new WWW("http://localhost/livinparis/saisi_sql.php", form);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("Erreur lors de la requête : " + www.error);
            yield break;
        }

        string json = www.text;
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("Aucune donnée reçue du serveur.");
            yield break;
        }

        if (json[0] == '0')
        {
            json = json.Substring(1);
        }

        try
        {
            json = json.Replace("IdClient", "Id")
                       .Replace("NomUtilisateur", "NU");

            json = "{\"items\":" + json + "}";

            var clients = JsonUtility.FromJson<Wrapper<Client>>(json);
            foreach (var client in clients.items)
            {
                noeudIdNomUtilisateur[idCount] = "Client " + client.NU;
                graphe.AjouterMembre(new Noeud<string>(idCount, $"Client: {client.NU}"));
                idCount++;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erreur lors de la désérialisation du JSON : " + ex.Message);
        }
    }

    IEnumerator ChargerCuisiniers()
    {
        string query = "SELECT IdCuisinier, NomUtilisateur FROM Utilisateur WHERE IdCuisinier IS NOT NULL AND IdClient IS NULL;";
        WWWForm form = new WWWForm();
        form.AddField("requete", query);
        WWW www = new WWW("http://localhost/livinparis/saisi_sql.php", form);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("Erreur lors de la requête : " + www.error);
            yield break;
        }

        string json = www.text;
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("Aucune donnée reçue du serveur.");
            yield break;
        }

        if (json[0] == '0')
        {
            json = json.Substring(1);
        }

        try
        {
            json = json.Replace("IdCuisinier", "Id")
                       .Replace("NomUtilisateur", "NU");

            json = "{\"items\":" + json + "}";

            var cuisiniers = JsonUtility.FromJson<Wrapper<Cuisinier>>(json);
            foreach (var cuisinier in cuisiniers.items)
            {
                noeudIdNomUtilisateur[idCount] = "Cuisinier " + cuisinier.NU;
                graphe.AjouterMembre(new Noeud<string>(idCount, $"Cuisinier: {cuisinier.NU}"));
                idCount++;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erreur lors de la désérialisation du JSON : " + ex.Message);
        }
    }

    IEnumerator ChargerCuisiniersEtClients()
    {
        string query = "SELECT NomUtilisateur FROM Utilisateur WHERE IdCuisinier IS NOT NULL AND IdClient IS NOT NULL;";
        WWWForm form = new WWWForm();
        form.AddField("requete", query);
        WWW www = new WWW("http://localhost/livinparis/saisi_sql.php", form);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("Erreur lors de la requête : " + www.error);
            yield break;
        }

        string json = www.text;
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("Aucune donnée reçue du serveur.");
            yield break;
        }

        if (json[0] == '0')
        {
            json = json.Substring(1);
        }

        try
        {
            json = json.Replace("IdCuisinierClient", "Id")
                       .Replace("NomUtilisateur", "NU");

            json = "{\"items\":" + json + "}";

            var cuisiniersClients = JsonUtility.FromJson<Wrapper<CuisinierEtClient>>(json);
            foreach (var cuisinierClient in cuisiniersClients.items)
            {
                noeudIdNomUtilisateur[idCount] = "Cuisinier et Client " + cuisinierClient.NU;
                graphe.AjouterMembre(new Noeud<string>(idCount, $"Cuisinier et Client: {cuisinierClient.NU}"));
                idCount++;
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erreur lors de la désérialisation du JSON : " + ex.Message);
        }
    }

    IEnumerator CreerRelations()
    {
        ///attention sur plusieurs ligne avec le @ ca marche plus, faut le refactor avant de l'envoyer a requete relation si tu veux le mettre sur une ligne
        string requeteRelations = "SELECT DISTINCT ucli.NomUtilisateur AS ClientNU, ucui.NomUtilisateur AS CuisinierNU FROM Commande co JOIN Utilisateur ucli ON co.IdClient = ucli.Id JOIN LigneDeCommande ldc ON co.IdCommande = ldc.IdCommande JOIN Plat p ON ldc.IdPlat = p.IdPlat JOIN Utilisateur ucui ON p.IdCuisinier = ucui.Id;";
        WWWForm form = new WWWForm();
        form.AddField("requete", requeteRelations);
        WWW www = new WWW("http://localhost/livinparis/saisi_sql.php", form);
        yield return www;

        if (www.error != null)
        {
            Debug.Log("Erreur lors de la requête : " + www.error);
            yield break;
        }

        string json = www.text;
        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("Aucune donnée reçue du serveur.");
            yield break;
        }

        if (json[0] == '0')
        {
            json = json.Substring(1);
        }

        if (string.IsNullOrEmpty(json))
        {
            Debug.Log("JSON après modification est vide.");
            yield break;
        }

        try
        {
            json = "{\"items\":" + json + "}";
            var relations = JsonUtility.FromJson<Wrapper<Relation>>(json);
            foreach (var relation in relations.items)
            {
                int IdClient = -1;
                int IdCuisinier = -1;

                string nomClient = "Cuisinier et Client " + relation.ClientNU;
                if (noeudIdNomUtilisateur.ContainsValue(nomClient))
                {
                    IdClient = noeudIdNomUtilisateur
                        .FirstOrDefault(x => x.Value == nomClient).Key;
                }
                else
                {
                    nomClient = "Client " + relation.ClientNU;
                    IdClient = noeudIdNomUtilisateur
                        .FirstOrDefault(x => x.Value == nomClient).Key;
                }

                string nomCuisinier = "Cuisinier et Client " + relation.CuisinierNU;
                if (noeudIdNomUtilisateur.ContainsValue(nomCuisinier))
                {
                    IdCuisinier = noeudIdNomUtilisateur
                        .FirstOrDefault(x => x.Value == nomCuisinier).Key;
                }
                else
                {
                    nomCuisinier = "Cuisinier " + relation.CuisinierNU;
                    IdCuisinier = noeudIdNomUtilisateur
                        .FirstOrDefault(x => x.Value == nomCuisinier).Key;
                }

                var noeudClient = graphe.TrouverNoeudParId(IdClient);
                var noeudCuisinier = graphe.TrouverNoeudParId(IdCuisinier);

                if (noeudClient != null && noeudCuisinier != null)
                {
                    graphe.AjouterRelation(noeudClient, noeudCuisinier, 1);
                    graphe.AjouterRelation(noeudCuisinier, noeudClient, 1);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("Erreur lors de la désérialisation du JSON : " + ex.Message);
        }
    }

    void AfficherGrapheDansContent()
    {
        float width = 1920f;
        float height = 980f;

        List<Noeud<string>> noeuds = graphe.Noeuds;
        int total = noeuds.Count;
        float rayon = Mathf.Min(width, height) / 2f - 100f;

        Vector2 centre = new Vector2(width / 2f, -height / 2f);

        Color[] couleursPrededefinies = new Color[]
        {
            Color.green,     
            Color.blue,      
            Color.black,
            Color.yellow,
        };

        Dictionary<string, Color> couleurParType = new Dictionary<string, Color>();
        int compteurCouleur = 0;

        for (int i = 0; i < total; i++)
        {
            string couleurType = noeuds[i].Couleur;

            if (!couleurParType.ContainsKey(couleurType))
            {
                couleurParType[couleurType] = couleursPrededefinies[compteurCouleur % couleursPrededefinies.Length];
                compteurCouleur++;
            }

            float angle = 2 * Mathf.PI * i / total;
            float x = centre.x + rayon * Mathf.Cos(angle);
            float y = centre.y + rayon * Mathf.Sin(angle);

            GameObject nodeObj = Instantiate(nodePrefab, graphContainer);
            RectTransform rect = nodeObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(x, y);

            noeudIdToRect.Add(noeuds[i].Id, rect);

            TMP_Text txt = nodeObj.GetComponentInChildren<TMP_Text>();
            if (txt != null) txt.text = noeuds[i].Contenu;

            Button nodeButton = nodeObj.GetComponentInChildren<Button>();
            if (nodeButton != null)
            {
                Image buttonImage = nodeButton.GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = couleurParType[couleurType];
                }
                else
                {
                    Debug.Log("Image du bouton introuvable pour le noeud avec ID: " + noeuds[i].Id);
                }
            }
            else
            {
                Debug.Log("Bouton introuvable pour le noeud avec ID: " + noeuds[i].Id);
            }
        }


        foreach (var arc in graphe.Liens)
        {
            RectTransform rectA = noeudIdToRect[arc.Source.Id];
            RectTransform rectB = noeudIdToRect[arc.Destination.Id];

            Vector2 posA = rectA.anchoredPosition;
            Vector2 posB = rectB.anchoredPosition;

            GameObject line = new GameObject("Lien", typeof(RectTransform));
            line.transform.SetParent(graphContainer, false);

            RectTransform rt = line.GetComponent<RectTransform>();

            rt.anchorMin = new Vector2(0, 1);
            rt.anchorMax = new Vector2(0, 1);
            rt.pivot = new Vector2(0, 1);

            Vector2 diff = posB - posA;

            rt.sizeDelta = new Vector2(diff.magnitude, 3f);
            rt.anchoredPosition = posA;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            rt.rotation = Quaternion.Euler(0, 0, angle);

            Image image = line.AddComponent<Image>();
            image.color = Color.red;

            line.transform.SetAsFirstSibling();
        }
    }



}
