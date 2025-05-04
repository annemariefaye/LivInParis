using System;
using System.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using PbSI;
using UnityEngine;

public static class DBManager
{
    public static string nomutilisateur;
    public static string email;
    public static string nom;
    public static string prenom;
    public static string adresse;
    public static string telephone;
    public static string? platDuJour;
    public static string? nomEntreprise;
    public static int fidelite;
    public static int? idClient;
    public static int? idCuisinier;


    public static int idPlatModif;
    public static int idPlatOffert;
    public static int idCuisinierNote;

    public static bool fideliteActivee = false;

    public static Dictionary<int, int> quantitesDansPanier = new Dictionary<int, int>();

    private static Dictionary<int, List<int>> commandesStationsProches =
        new Dictionary<int, List<int>>();

    public static bool Connecte
    {
        get { return nomutilisateur != null; }
    }

    public static void Deconnexion()
    {
        nomutilisateur = null;
        email = null;
        nom = null;
        prenom = null;
        adresse = null;
        telephone = null;
        platDuJour = null;
        nomEntreprise = null;
        fidelite = 0;

        idPlatModif = 0;
        idPlatOffert = 0;
        idCuisinierNote = 0;

        quantitesDansPanier.Clear();
        commandesStationsProches.Clear();
    }

    public static IEnumerator MettreAJourFidelite()
    {
        WWWForm form = new WWWForm();
        form.AddField("nomutilisateur", nomutilisateur);
        form.AddField("fidelite", fidelite);

        using (WWW www = new WWW("http://localhost/livinparis/maj_fidelite.php", form))
        {
            yield return www;

            if (www.text == "0")
            {
                Debug.Log("Fidélité mise à jour avec succès !");
            }
            else
            {
                Debug.LogError("Erreur lors de la mise à jour de la fidélité : " + www.text);
            }
        }
    }

    public static async Task<bool> AdresseValide(string adresse)
    {
        if (string.IsNullOrWhiteSpace(adresse))
        {
            return false;
        }

        string url =
            $"https://nominatim.openstreetmap.org/search?format=json&q={Uri.EscapeDataString(adresse)}";

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("User-Agent", "UnityApp");

            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JArray data = JArray.Parse(json);

                    if (data.Count > 0)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log($"Erreur lors de la vérification d'adresse : {e.Message}");
            }
        }

        return false;
    }
}
