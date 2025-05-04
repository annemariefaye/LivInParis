using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LandingPage : MonoBehaviour
{
    public void Connexion()
    {
        SceneManager.LoadScene(1);
    }

    public void CreerCompte()
    {
        SceneManager.LoadScene(0);
    }
}
