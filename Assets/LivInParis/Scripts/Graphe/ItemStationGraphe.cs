using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStationGraphe
{
    public int stationId;
    public string stationNom;
    public string precedentId;
    public string suivantId;
    public int tempsEntreDeuxStations;
    public int tempsDeChangement;
    public string sensUnique;

    public ItemStationGraphe(
        int stationId,
        string stationNom,
        string precedentId,
        string suivantId,
        int tempsEntreDeuxStations,
        int tempsDeChangement,
        string sensUnique
    )
    {
        this.stationId = stationId;
        this.stationNom = stationNom;
        this.precedentId = precedentId;
        this.suivantId = suivantId;
        this.tempsEntreDeuxStations = tempsEntreDeuxStations;
        this.tempsDeChangement = tempsDeChangement;
        this.sensUnique = sensUnique;
    }
}
