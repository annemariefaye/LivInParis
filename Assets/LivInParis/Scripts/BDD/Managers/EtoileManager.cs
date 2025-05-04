using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EtoileManager : MonoBehaviour
{
    public Button etoile1;
    public Button etoile2;
    public Button etoile3;
    public Button etoile4;
    public Button etoile5;

    public Sprite etoile_vide;
    public Sprite etoile_rempli;

    private Image[] etoiles;

    public int note = 1;

    private void Start()
    {
        etoiles = new Image[5];
        etoiles[0] = etoile1.GetComponent<Image>();
        etoiles[1] = etoile2.GetComponent<Image>();
        etoiles[2] = etoile3.GetComponent<Image>();
        etoiles[3] = etoile4.GetComponent<Image>();
        etoiles[4] = etoile5.GetComponent<Image>();

        etoile1.onClick.AddListener(() => RemplirEtoile(1));
        etoile2.onClick.AddListener(() => RemplirEtoile(2));
        etoile3.onClick.AddListener(() => RemplirEtoile(3));
        etoile4.onClick.AddListener(() => RemplirEtoile(4));
        etoile5.onClick.AddListener(() => RemplirEtoile(5));
    }

    public void RemplirEtoile(int etoileCliquee)
    {
        note = etoileCliquee;

        for (int i = 0; i < etoiles.Length; i++)
        {
            if (i < etoileCliquee)
            {
                etoiles[i].sprite = etoile_rempli;
            }
            else
            {
                etoiles[i].sprite = etoile_vide;
            }
        }
    }
}
