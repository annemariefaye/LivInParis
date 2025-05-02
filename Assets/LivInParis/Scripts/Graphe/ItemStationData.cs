using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStationData
{
    public int id;
    public string ligne;
    public string libelle;
    public double longitude;
    public double latitude;
    public string commune;
    public int codeInsee;

    public ItemStationData(int id, string ligne, string libelle, double longitude, double latitude, string commune, int codeInsee)
    {
        this.id = id;
        this.ligne = ligne;
        this.libelle = libelle;
        this.longitude = longitude;
        this.latitude = latitude;
        this.commune = commune;
        this.codeInsee = codeInsee;
    }
}
