using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Panier : MonoBehaviour
{
    public TMP_Text NU;
    public TMP_Text fidelite;

    private void Start()
    {
        if(DBManager.nomEntreprise != null && DBManager.nomEntreprise.Length > 0)
        {
            NU.text = DBManager.prenom;
        }
        else
        {
            NU.text = DBManager.nomEntreprise;
        }
        fidelite.text = "Vous avez " + DBManager.fidelite + " points";


    }

    public void RetourPagePrecedente()
    {
        if (DBManager.fideliteActivee)
        {
            DBManager.fidelite += 100;
            DBManager.fideliteActivee = false;
        }
        SceneManager.LoadScene(5);
        
    }

    public void Modif()
    {
        SceneManager.LoadScene(16);
    }
}
