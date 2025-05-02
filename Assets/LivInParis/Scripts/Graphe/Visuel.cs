using System.Collections.Generic;
using UnityEngine;
using PbSI;
using Mapbox.Utils;
using System;
using Mapbox.Unity.Location;

public class Visuel : MonoBehaviour
{
	ReseauMetro reseau;
	Graphe<StationMetro> graphe;

	[SerializeField] SpawnOnMapGraphe spawnOnMap;
	public Material materialLigne;

	private Dictionary<(int, int), LineRenderer> lignes = new Dictionary<(int, int), LineRenderer>();

	[SerializeField] float amplitude = 2.0f;
	[SerializeField] float frequency = 0.50f;

	List<Vector3> positionsSousGraphe = new List<Vector3>();

	string departStringCoords;
	string arriveeStringCoords;

	[SerializeField]
	[Range(0, 359)]
	float _heading;

	async void Start()
	{
		reseau = new ReseauMetro();
		graphe = reseau.Graphe;

		string[] localisations = new string[graphe.Noeuds.Count];

		for (int i = 0; i < graphe.Noeuds.Count; i++)
		{
			StationMetro station = graphe.Noeuds[i].Contenu;
			localisations[i] = station.Latitude.ToString(System.Globalization.CultureInfo.InvariantCulture)
							 + ", "
							 + station.Longitude.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}

		/// Setup du grand graphe
		spawnOnMap.SetLocationStrings(localisations, graphe.Noeuds);

		foreach (Lien<StationMetro> lien in graphe.Liens)
		{
			GameObject ligneObj = TracerLiens(Vector3.zero, Vector3.zero, materialLigne);
			lignes[(lien.Source.Id, lien.Destination.Id)] = ligneObj.GetComponent<LineRenderer>();
		}

		RechercheStationProche recherche = new RechercheStationProche(" 33 Av. du Maine, 75015 Paris", graphe);
		RechercheStationProche recherche2 = new RechercheStationProche("75 rue des Martyrs, 75018 Paris", graphe);
		await recherche.InitialiserAsync();
		await recherche2.InitialiserAsync();

		departStringCoords = recherche.CoordonneesString;
		arriveeStringCoords = recherche2.CoordonneesString;

		try
		{
			List<int> depart = recherche.IdStationsProches;
			List<int> arrivee = recherche2.IdStationsProches;


			float tempsDeplacementDepart = recherche.TempsDeplacement;
			float tempsDeplacementArrivee = recherche2.TempsDeplacement;

			var resultat = RechercheChemin<StationMetro>.DijkstraListe(graphe, depart, arrivee);

			if (resultat != null)
			{
				double tempsTotal = tempsDeplacementDepart + tempsDeplacementArrivee + resultat.PoidsTotal;
				Debug.Log("Temps total de déplacement : " + (int)tempsTotal + " minutes");
			}
			else
			{
				Debug.Log("Aucun chemin trouvé.");
			}

			List<Vector3> positions = spawnOnMap.ObtenirPositionsSpawn();

			foreach (int id in resultat.Chemin)
			{
				positionsSousGraphe.Add(positions[id]);
			}

			DessinerChemin(positionsSousGraphe, materialLigne);
		}
		catch (Exception e)
		{
			Debug.LogError($"Erreur : {e.Message}");
		}



	}

	void Update()
	{
		UpdateGraphComplet();
		UpdateSousGraphe();
	}

	void UpdateGraphComplet()
	{
		List<Vector3> positions = spawnOnMap.ObtenirPositionsSpawn();

		foreach (Lien<StationMetro> lien in graphe.Liens)
		{
			Vector3 posDebut = positions[lien.Source.Id];
			Vector3 posFin = positions[lien.Destination.Id];

			posDebut = AppliquerFlottement(posDebut);
			posFin = AppliquerFlottement(posFin);

			if (lignes.TryGetValue((lien.Source.Id, lien.Destination.Id), out LineRenderer ligne))
			{
				ligne.SetPosition(0, posDebut);
				ligne.SetPosition(1, posFin);
			}
		}
	}

	private void UpdateSousGraphe()
	{
		GameObject cheminSousGraphe = GameObject.Find("CheminSousGraphe");

		if (cheminSousGraphe != null)
		{
			LineRenderer ligne = cheminSousGraphe.GetComponent<LineRenderer>();

			if (ligne != null && positionsSousGraphe.Count > 0)
			{
				for (int i = 0; i < positionsSousGraphe.Count; i++)
				{
					Vector3 position = positionsSousGraphe[i];
					position = AppliquerFlottement(position);

					ligne.SetPosition(i, position);
				}
			}
		}
	}



	Vector3 AppliquerFlottement(Vector3 position)
	{
		position.y = (Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude) + 15 + position.y;
		return position;
	}

	public GameObject TracerLiens(Vector3 positionDebut, Vector3 positionFin, Material materiauLigne, float largeur = 1f)
	{
		GameObject objetLigne = new GameObject("Lien");
		LineRenderer ligne = objetLigne.AddComponent<LineRenderer>();

		ligne.material = materiauLigne;
		ligne.startWidth = largeur;
		ligne.endWidth = largeur;
		ligne.positionCount = 2;
		ligne.useWorldSpace = true;

		ligne.SetPosition(0, positionDebut);
		ligne.SetPosition(1, positionFin);

		return objetLigne;
	}

	public GameObject DessinerChemin(List<Vector3> positionsNoeuds, Material materiauLigne, float largeur = 1f)
	{
		GameObject objetChemin = new GameObject("CheminSousGraphe");
		LineRenderer ligne = objetChemin.AddComponent<LineRenderer>();

		ligne.material = materiauLigne;
		ligne.startWidth = largeur;
		ligne.endWidth = largeur;
		ligne.positionCount = positionsNoeuds.Count;
		ligne.useWorldSpace = true;

		for (int i = 0; i < positionsNoeuds.Count; i++)
		{
			ligne.SetPosition(i, positionsNoeuds[i]);
		}

		return objetChemin;
	}

	public List<Vector3> GetPosSousGraphe()
	{
		if (positionsSousGraphe != null)
		{
			List<Vector3> temp = new List<Vector3>(positionsSousGraphe);
			temp.Add(spawnOnMap.ConvertCoordsToPos(arriveeStringCoords));
			return temp;
		}
		return null;
	}


	/// Servira peut etre dans un futur + - proche
	public Vector3 DepartCoords { get{ return spawnOnMap.ConvertCoordsToPos(departStringCoords); }}
	/*public Vector3 ArriveeCoords { get{ return spawnOnMap.ConvertCoordsToPos(arriveeStringCoords); }}*/

}
