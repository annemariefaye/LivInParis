using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModifierPanier : MonoBehaviour
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
        SceneManager.LoadScene(6);
    }
}
