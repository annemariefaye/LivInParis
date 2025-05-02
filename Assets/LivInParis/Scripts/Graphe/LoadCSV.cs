using System.Collections.Generic;
using UnityEngine;

public class LoadCSV : MonoBehaviour
{
    private List<ItemStationGraphe> stationGrapheList = new List<ItemStationGraphe>();
    private List<ItemStationData> stationDataList = new List<ItemStationData>();

    public LoadCSV()
    {
        LoadItemData();
    }
    
    private void LoadItemData()
    {
        List<Dictionary<string, object>> donneesData = CSVReader.Read("MetroParisData");
        List<Dictionary<string, object>> donneesGraphe = CSVReader.Read("MetroParisGraphe");
        
        for (int i = 0; i < donneesData.Count; i++)
        {
            int id = int.Parse(donneesData[i]["ID Station"].ToString());
            string libelle = donneesData[i]["Libelle station"].ToString();
            string ligne = donneesData[i]["Libelle Line"].ToString();
            double longitude = double.Parse(donneesData[i]["Longitude"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
            double latitude = double.Parse(donneesData[i]["Latitude"].ToString(), System.Globalization.CultureInfo.InvariantCulture);
            string commune = donneesData[i]["Commune nom"].ToString();
            int codeInsee = int.Parse(donneesData[i]["Commune code Insee"].ToString());

            stationDataList.Add(new ItemStationData(id, ligne, libelle, longitude, latitude, commune, codeInsee));
        }

        for (int i = 0; i < donneesGraphe.Count; i++)
        {
            int stationId = GetIntValue(donneesGraphe[i], "Station Id");
            string stationNom = GetStringValue(donneesGraphe[i], "Station");
            string precedentId = GetStringValue(donneesGraphe[i], "Précédent");
            string suivantId = GetStringValue(donneesGraphe[i], "Suivant");
            int tempsEntreDeuxStations = GetIntValue(donneesGraphe[i], "Temps entre 2 stations");
            int tempsDeChangement = GetIntValue(donneesGraphe[i], "Temps de Changement");
            string sensUnique = GetStringValue(donneesGraphe[i], "Sens Unique");

            stationGrapheList.Add(new ItemStationGraphe(stationId, stationNom, precedentId, suivantId, tempsEntreDeuxStations, tempsDeChangement, sensUnique));
        }

    }

    private int GetIntValue(Dictionary<string, object> dict, string key)
    {
        if (dict.ContainsKey(key) && int.TryParse(dict[key].ToString(), out int result))
        {
            return result;
        }
        return -1;
    }

    private string GetStringValue(Dictionary<string, object> dict, string key)
    {
        if (dict.ContainsKey(key) && !string.IsNullOrEmpty(dict[key].ToString()))
        {
            return dict[key].ToString();
        }
        return "-1";
    }

    public List<ItemStationGraphe> StationGrapheList => stationGrapheList;
    public List<ItemStationData> StationDataList => stationDataList;
}