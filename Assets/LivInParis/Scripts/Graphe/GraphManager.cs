using PbSI;
using UnityEngine;

public class GraphManager : MonoBehaviour
{
    private static GraphManager _instance;
    private Graphe<StationMetro> graphe;

    public static GraphManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("GraphManager");
                _instance = go.AddComponent<GraphManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
    public Graphe<StationMetro> Graphe
    {
        get { return graphe; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        ReseauMetro reseau = new ReseauMetro();
        graphe = reseau.Graphe;
    }
}
