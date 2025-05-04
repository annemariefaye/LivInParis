using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StatistiquesPage : MonoBehaviour
{
    public GameObject cuisinier;
    public GameObject client;
    public GameObject commandeOverview;
    public GameObject commandesClient;

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(12);
    }

    public void Cuisinier()
    {
        if (!cuisinier.activeSelf)
        {
            cuisinier.SetActive(true);
            commandeOverview.SetActive(false);
            client.SetActive(false);
            commandesClient.SetActive(false);
        }
    }

    public void CommandeOverview()
    {
        if (!commandeOverview.activeSelf)
        {
            commandeOverview.SetActive(true);
            cuisinier.SetActive(false);
            client.SetActive(false);
            commandesClient.SetActive(false);
        }
    }

    public void Client()
    {
        if (!client.activeSelf)
        {
            client.SetActive(true);
            cuisinier.SetActive(false);
            commandeOverview.SetActive(false);
            commandesClient.SetActive(false);
        }
    }

    public void CommandesClient()
    {
        if (!commandesClient.activeSelf)
        {
            commandesClient.SetActive(true);
            cuisinier.SetActive(false);
            client.SetActive(false);
            commandeOverview.SetActive(false);
        }
    }
}
