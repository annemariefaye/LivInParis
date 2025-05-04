using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdminCuisinier : MonoBehaviour
{
    public Button returnButton;

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(12);
    }
}
