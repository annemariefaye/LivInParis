using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    public TMP_Text NU;
    public TMP_Text fidelite;

    private void Start()
    {
        NU.text = "Bonjour " + DBManager.prenom;
        fidelite.text = "Vous avez " + DBManager.fidelite + " points";
    }

    public void Deconnexion()
    {
        SceneManager.LoadScene(11);
    }

    public void Settings()
    {
        SceneManager.LoadScene(2);
    }

    public void Cart()
    {
        SceneManager.LoadScene(6);
    }
}
