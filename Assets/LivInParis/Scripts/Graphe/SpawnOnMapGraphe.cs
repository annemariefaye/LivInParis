using UnityEngine;
using Mapbox.Utils;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.Utilities;
using System.Collections.Generic;
using PbSI;

public class SpawnOnMapGraphe : MonoBehaviour
{
	[SerializeField]
	AbstractMap _map;

	[SerializeField]
	[Geocode]
	string[] _locationStrings;

	Vector2d[] _locations;

	[SerializeField]
	float _spawnScale = 100f;

	[SerializeField]
	GameObject _markerPrefab;

	List<GameObject> _spawnedObjects = new List<GameObject>();

	private bool _initialized = false;

	List<Noeud<StationMetro>> _noeuds;

	void Start()
	{
		if (!_initialized)
		{
			InitializeMarkers();
		}
	}

	public void SetLocationStrings(string[] newLocations,  List<Noeud<StationMetro>> noeuds)
	{
		_locationStrings = newLocations;
		_noeuds = noeuds;
		InitializeMarkers();
		_initialized = true;
	}

	private void InitializeMarkers()
	{
		if (_locationStrings == null || _locationStrings.Length == 0)
		{
			Debug.LogWarning("No locations to spawn.");
			return;
		}

		_locations = new Vector2d[_locationStrings.Length];
		_spawnedObjects = new List<GameObject>();

		for (int i = 0; i < _locationStrings.Length; i++)
		{
			var locationString = _locationStrings[i];
			_locations[i] = Conversions.StringToLatLon(locationString);

			var instance = Instantiate(_markerPrefab);
			instance.GetComponent<EventPointer>().eventPos = _locations[i];
			instance.GetComponent<EventPointer>().noeud = _noeuds[i];
			instance.transform.localPosition = _map.GeoToWorldPosition(_locations[i], true);
			instance.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
			_spawnedObjects.Add(instance);
		}
	}

	private void Update()
	{
		if (_spawnedObjects == null || _locations == null) return;

		int count = _spawnedObjects.Count;
		for (int i = 0; i < count; i++)
		{
			var spawnedObject = _spawnedObjects[i];
			var location = _locations[i];
			spawnedObject.transform.localPosition = _map.GeoToWorldPosition(location, true);
			spawnedObject.transform.localScale = new Vector3(_spawnScale, _spawnScale, _spawnScale);
		}
	}

	public List<Vector3> ObtenirPositionsSpawn()
	{
		List<Vector3> positions = new List<Vector3>();

		if (_spawnedObjects == null || _locations == null) return positions;

		int count = _spawnedObjects.Count;
		for (int i = 0; i < count; i++)
		{
			var location = _locations[i];
			Vector3 positionMonde = _map.GeoToWorldPosition(location, true);
			positions.Add(positionMonde);
		}

		return positions;
	}

	public Vector3 ConvertCoordsToPos(string coords)
    {
		return _map.GeoToWorldPosition(Conversions.StringToLatLon(coords), true);
	}


}


