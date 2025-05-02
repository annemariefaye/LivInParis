using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TemplatePlatCuisinier : MonoBehaviour
{
    public Image platImage;
    public TMP_Text titreText;
    public TMP_Text prixText;
    public TMP_Text descriptionText;
    public TMP_Text noteText;

    public int idPlat;

    public void Init(int idPlatInitial)
    {
        idPlat = idPlatInitial;
    }

    public void ModifierButton()
    {
        DBManager.idPlatModif = idPlat;
        SceneManager.LoadScene(18);
    }
}
