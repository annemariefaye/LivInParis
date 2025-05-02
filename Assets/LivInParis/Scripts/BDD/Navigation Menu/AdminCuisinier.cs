using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdminCuisinier: MonoBehaviour
{
    public Button returnButton;

    public void RetourPagePrecedente()
    {
        SceneManager.LoadScene(12);
    }

}
