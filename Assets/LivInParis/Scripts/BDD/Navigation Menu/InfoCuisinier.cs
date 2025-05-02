using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InfoCuisinier : MonoBehaviour
{
    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(12);
    }
}
