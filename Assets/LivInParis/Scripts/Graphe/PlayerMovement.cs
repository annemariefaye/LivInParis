using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public VisuelSimu visu;
    public List<Vector3> cheminSousGraphe = null;
    public float vitesse = 2.0f;

    private int indexPosition = 0;
    private bool estEnDeplacement = false;

    void Update()
    {
        if (cheminSousGraphe == null || cheminSousGraphe.Count <= 0)
        {
            cheminSousGraphe = visu.GetPosSousGraphe();
            Debug.Log("nb arrets : " + cheminSousGraphe.Count);
        }

        if (cheminSousGraphe.Count > 1 && !estEnDeplacement)
        {
            StartCoroutine(SuivreChemin());
        }
    }

    IEnumerator SuivreChemin()
    {
        estEnDeplacement = true;
        transform.position = cheminSousGraphe[indexPosition];
        while (indexPosition < cheminSousGraphe.Count)
        {
            Vector3 targetPosition = cheminSousGraphe[indexPosition];
            float step = vitesse * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (transform.position == targetPosition)
            {
                indexPosition++;
            }

            yield return null;
        }

        estEnDeplacement = false;
    }

    public void SetChemin(List<Vector3> nouveauChemin)
    {
        cheminSousGraphe = nouveauChemin;
        indexPosition = 0;
    }
}
