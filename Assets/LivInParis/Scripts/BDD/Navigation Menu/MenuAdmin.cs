using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuAdmin : MonoBehaviour
{
    public void Deconnexion()
    {
        SceneManager.LoadScene(11);
    }

    public void DataCusiniers()
    {
        SceneManager.LoadScene(8);
    }

    public void DataClients()
    {
        SceneManager.LoadScene(7);
    }

    public void SaisiSQL()
    {
        SceneManager.LoadScene(13);
    }

    public void Stats()
    {
        SceneManager.LoadScene(14);
    }

    public void Export()
    {
        //SceneManager.LoadScene(11);
        Debug.Log("partie de valentin à intégrer");
    }

    public void Graphe2D()
    {
        SceneManager.LoadScene(21);
    }

    public void Graphe3D()
    {
        SceneManager.LoadScene(3);
    }

    public void GrapheColore()
    {
        SceneManager.LoadScene(22);
    }
}
