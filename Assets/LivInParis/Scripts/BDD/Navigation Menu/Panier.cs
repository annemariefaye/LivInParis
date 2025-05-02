using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Panier : MonoBehaviour
{
    public TMP_Text NU;
    public TMP_Text fidelite;

    private void Start()
    {
        NU.text = DBManager.prenom;
        fidelite.text = "Vous avez " + DBManager.fidelite + " points";
    }
    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(5);
    }

    public void Modif()
    {
        SceneManager.LoadScene(16);
    }

}
