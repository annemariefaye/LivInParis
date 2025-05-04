using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public TMP_Text NU;
    public TMP_Text fidelite;

    private void Start()
    {
        fidelite.text = "Vous avez " + DBManager.fidelite + " points";

        if (DBManager.nomEntreprise != null && DBManager.nomEntreprise.Length > 0)
        {
            NU.text = "Bonjour " + DBManager.prenom;
            fidelite.text = "Vous avez " + DBManager.fidelite + " points";
        }
        else
        {
            NU.text = "Bonjour " + DBManager.nomEntreprise;
        }
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
