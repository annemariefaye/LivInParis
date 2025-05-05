using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PbSI
{

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

    /// <summary>
    /// Classe permettant de gérer le graphe des cuisiniers et clients.
    /// </summary>
    public class GrapheCuisinierClient : MonoBehaviour
    {
        #region Attributs
        /// <summary>
        /// Instance du graphe.
        /// </summary>
        private Graphe<string> graphe;

        /// <summary>
        /// Préfabriqué pour les noeuds.
        /// </summary>
        public GameObject nodePrefab;

        /// <summary>
        /// Conteneur pour afficher le graphe.
        /// </summary>
        public Transform graphContainer;

        /// <summary>
        /// Matériau pour les lignes du graphe.
        /// </summary>
        public Material lineMaterial;

        /// <summary>
        /// Dictionnaire associant les identifiants des noeuds à leurs positions.
        /// </summary>
        private Dictionary<int, RectTransform> noeudIdToRect = new Dictionary<int, RectTransform>();

        /// <summary>
        /// Dictionnaire associant les identifiants des noeuds à leurs noms d'utilisateur.
        /// </summary>
        private Dictionary<int, string> noeudIdNomUtilisateur = new Dictionary<int, string>();

        /// <summary>
        /// Compteur pour générer les identifiants des noeuds.
        /// </summary>
        int idCount = 0;
        #endregion

        #region Méthodes
        /// <summary>
        /// Méthode appelée au démarrage pour initialiser le graphe.
        /// </summary>
        void Start()
        {
            graphe = new Graphe<string>();
            StartCoroutine(ConstruireGraphe());
        }

        /// <summary>
        /// Retourne à la page précédente.
        /// </summary>
        public void RetourPagePrecedente()
        {
            SceneManager.LoadScene(12);
        }

        /// <summary>
        /// Applique une coloration au graphe.
        /// </summary>
        void Colorier()
        {
            Coloration<string> colorer = new Coloration<string>(graphe);
            colorer.WelshPowell();
            colorer.AfficherColoration();
        }

        /// <summary>
        /// Construit le graphe en chargeant les données des clients et cuisiniers.
        /// </summary>
        IEnumerator ConstruireGraphe()
        {
            yield return StartCoroutine(ChargerClients());
            yield return StartCoroutine(ChargerCuisiniers());
            yield return StartCoroutine(ChargerCuisiniersEtClients());
            yield return StartCoroutine(CreerRelations());
            Colorier();
            AfficherGrapheDansContent();
        }

        /// <summary>
        /// Charge les données des clients.
        /// </summary>
        IEnumerator ChargerClients()
        {
            string query =
                "SELECT IdClient, NomUtilisateur FROM Utilisateur WHERE IdClient IS NOT NULL AND IdCuisinier IS NULL;";
            WWWForm form = new WWWForm();
            form.AddField("requete", query);
            WWW www = new WWW("http://localhost/livinparis/saisi_sql.php", form);
            yield return www;

            if (www.error != null)
            {
                Debug.Log("Erreur lors de la requ�te : " + www.error);
                yield break;
            }

            string json = www.text;
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Aucune donn�e re�ue du serveur.");
                yield break;
            }

            if (json[0] == '0')
            {
                json = json.Substring(1);
            }

            try
            {
                json = json.Replace("IdClient", "Id").Replace("NomUtilisateur", "NU");

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
                Debug.Log("Erreur lors de la d�s�rialisation du JSON : " + ex.Message);
            }
        }

        /// <summary>
        /// Charge les données des cuisiniers.
        /// </summary>
        IEnumerator ChargerCuisiniers()
        {
            string query =
                "SELECT IdCuisinier, NomUtilisateur FROM Utilisateur WHERE IdCuisinier IS NOT NULL AND IdClient IS NULL;";
            WWWForm form = new WWWForm();
            form.AddField("requete", query);
            WWW www = new WWW("http://localhost/livinparis/saisi_sql.php", form);
            yield return www;

            if (www.error != null)
            {
                Debug.Log("Erreur lors de la requ�te : " + www.error);
                yield break;
            }

            string json = www.text;
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Aucune donn�e re�ue du serveur.");
                yield break;
            }

            if (json[0] == '0')
            {
                json = json.Substring(1);
            }

            try
            {
                json = json.Replace("IdCuisinier", "Id").Replace("NomUtilisateur", "NU");

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
                Debug.Log("Erreur lors de la d�s�rialisation du JSON : " + ex.Message);
            }
        }

        /// <summary>
        /// Charge les données des cuisiniers qui sont aussi clients.
        /// </summary>
        IEnumerator ChargerCuisiniersEtClients()
        {
            string query =
                "SELECT NomUtilisateur FROM Utilisateur WHERE IdCuisinier IS NOT NULL AND IdClient IS NOT NULL;";
            WWWForm form = new WWWForm();
            form.AddField("requete", query);
            WWW www = new WWW("http://localhost/livinparis/saisi_sql.php", form);
            yield return www;

            if (www.error != null)
            {
                Debug.Log("Erreur lors de la requ�te : " + www.error);
                yield break;
            }

            string json = www.text;
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Aucune donn�e re�ue du serveur.");
                yield break;
            }

            if (json[0] == '0')
            {
                json = json.Substring(1);
            }

            try
            {
                json = json.Replace("IdCuisinierClient", "Id").Replace("NomUtilisateur", "NU");

                json = "{\"items\":" + json + "}";

                var cuisiniersClients = JsonUtility.FromJson<Wrapper<CuisinierEtClient>>(json);
                foreach (var cuisinierClient in cuisiniersClients.items)
                {
                    noeudIdNomUtilisateur[idCount] = "Cuisinier et Client " + cuisinierClient.NU;
                    graphe.AjouterMembre(
                        new Noeud<string>(idCount, $"Cuisinier et Client: {cuisinierClient.NU}")
                    );
                    idCount++;
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log("Erreur lors de la d�s�rialisation du JSON : " + ex.Message);
            }
        }

        /// <summary>
        /// Crée les relations entre les noeuds du graphe.
        /// </summary>
        IEnumerator CreerRelations()
        {
            ///attention sur plusieurs ligne avec le @ ca marche plus, faut le refactor avant de l'envoyer a requete relation si tu veux le mettre sur une ligne
            string requeteRelations =
                "SELECT DISTINCT ucli.NomUtilisateur AS ClientNU, ucui.NomUtilisateur AS CuisinierNU FROM Commande co JOIN Utilisateur ucli ON co.IdClient = ucli.Id JOIN LigneDeCommande ldc ON co.IdCommande = ldc.IdCommande JOIN Plat p ON ldc.IdPlat = p.IdPlat JOIN Utilisateur ucui ON p.IdCuisinier = ucui.Id;";
            WWWForm form = new WWWForm();
            form.AddField("requete", requeteRelations);
            WWW www = new WWW("http://localhost/livinparis/saisi_sql.php", form);
            yield return www;

            if (www.error != null)
            {
                Debug.Log("Erreur lors de la requ�te : " + www.error);
                yield break;
            }

            string json = www.text;
            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("Aucune donn�e re�ue du serveur.");
                yield break;
            }

            if (json[0] == '0')
            {
                json = json.Substring(1);
            }

            if (string.IsNullOrEmpty(json))
            {
                Debug.Log("JSON apr�s modification est vide.");
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
                        IdClient = noeudIdNomUtilisateur.FirstOrDefault(x => x.Value == nomClient).Key;
                    }
                    else
                    {
                        nomClient = "Client " + relation.ClientNU;
                        IdClient = noeudIdNomUtilisateur.FirstOrDefault(x => x.Value == nomClient).Key;
                    }

                    string nomCuisinier = "Cuisinier et Client " + relation.CuisinierNU;
                    if (noeudIdNomUtilisateur.ContainsValue(nomCuisinier))
                    {
                        IdCuisinier = noeudIdNomUtilisateur
                            .FirstOrDefault(x => x.Value == nomCuisinier)
                            .Key;
                    }
                    else
                    {
                        nomCuisinier = "Cuisinier " + relation.CuisinierNU;
                        IdCuisinier = noeudIdNomUtilisateur
                            .FirstOrDefault(x => x.Value == nomCuisinier)
                            .Key;
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
                Debug.Log("Erreur lors de la d�s�rialisation du JSON : " + ex.Message);
            }
        }

        /// <summary>
        /// Affiche le graphe dans l'interface utilisateur.
        /// </summary>
        void AfficherGrapheDansContent()
        {
            float width = 1920f;
            float height = 980f;
            Vector2 centre = new Vector2(width / 2f, -height / 2f);

            List<Noeud<string>> noeuds = graphe.Noeuds;
            int total = noeuds.Count;

            Dictionary<int, int> degres = new Dictionary<int, int>();
            for (int i = 0; i < total; i++)
            {
                degres[noeuds[i].Id] = graphe.Liens.Count(l => l.Source.Id == noeuds[i].Id || l.Destination.Id == noeuds[i].Id);
            }

            List<Noeud<string>> noeudsTries = noeuds.OrderByDescending(n => degres[n.Id]).ToList();

            float rayonBase = 120f;
            float separationRayon = 150f;
            int currentLayer = 0;
            int placed = 0;

            Color[] couleursPrededefinies = new Color[]
            {
                HexToColor("D2B900"),
                HexToColor("D12800"),
                HexToColor("0052D4"),
                HexToColor("2F9F2A"),
            };
            Dictionary<string, Color> couleurParType = new Dictionary<string, Color>();
            int compteurCouleur = 0;

            while (placed < total)
            {
                int noeudsDansCercle = Mathf.Min(8 + currentLayer * 6, total - placed);
                float rayon = rayonBase + currentLayer * separationRayon;

                for (int i = 0; i < noeudsDansCercle; i++)
                {
                    Noeud<string> noeud = noeudsTries[placed];
                    float angle = 2 * Mathf.PI * i / noeudsDansCercle;

                    float extraDistance = 20f;
                    float x = centre.x + (rayon + extraDistance) * Mathf.Cos(angle);
                    float y = centre.y + (rayon + extraDistance) * Mathf.Sin(angle);

                    Vector2 position = new Vector2(x, y);

                    string couleurType = noeud.Couleur;
                    if (!couleurParType.ContainsKey(couleurType))
                    {
                        couleurParType[couleurType] = couleursPrededefinies[compteurCouleur % couleursPrededefinies.Length];
                        compteurCouleur++;
                    }

                    GameObject nodeObj = Instantiate(nodePrefab, graphContainer);
                    RectTransform rect = nodeObj.GetComponent<RectTransform>();
                    rect.anchoredPosition = position;
                    noeudIdToRect.Add(noeud.Id, rect);

                    TMP_Text txt = nodeObj.GetComponentInChildren<TMP_Text>();
                    if (txt != null)
                        txt.text = noeud.Contenu;

                    Button nodeButton = nodeObj.GetComponentInChildren<Button>();
                    if (nodeButton != null)
                    {
                        Image buttonImage = nodeButton.GetComponent<Image>();
                        if (buttonImage != null)
                            buttonImage.color = couleurParType[couleurType];
                    }

                    placed++;
                    if (placed >= total) break;
                }

                currentLayer++;
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
                image.color = Color.black;

                line.transform.SetAsFirstSibling();
            }
        }

        Color HexToColor(string hex)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString("#" + hex, out color))
                return color;
            return Color.white;
        }

        #endregion
    }
}