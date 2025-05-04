using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CuisinierCommandes : MonoBehaviour
{
    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(15);
    }
}
