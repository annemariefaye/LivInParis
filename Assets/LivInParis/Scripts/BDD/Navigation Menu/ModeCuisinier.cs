using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeCuisinier : MonoBehaviour
{
    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(9);
    }

    public void CreerPlat()
    {
        SceneManager.LoadScene(4);
    }

    public void VerfierCommandes()
    {
        SceneManager.LoadScene(19);
    }
}
