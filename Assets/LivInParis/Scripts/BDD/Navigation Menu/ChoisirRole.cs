using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoisirRole : MonoBehaviour
{
    public void ModeCuisinier()
    {
        SceneManager.LoadScene(15);
    }

    public void ModeClient()
    {
        SceneManager.LoadScene(5);
    }

    public void Deconnexion()
    {
        SceneManager.LoadScene(11);
    }
}
