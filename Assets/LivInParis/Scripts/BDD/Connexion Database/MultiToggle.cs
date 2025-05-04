using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MultiToggle : MonoBehaviour
{
    public Toggle cuisinierToggle;
    public Toggle clientToggle;
    public GameObject inputFieldCuisinier;
    public GameObject inputFieldClient;

    private CanvasGroup canvasCuisinier;
    private CanvasGroup canvasClient;

    void Start()
    {
        canvasCuisinier = GetCanvasGroup(inputFieldCuisinier);
        canvasClient = GetCanvasGroup(inputFieldClient);

        cuisinierToggle.onValueChanged.AddListener(OnCuisinierToggleChanged);
        clientToggle.onValueChanged.AddListener(OnClientToggleChanged);

        inputFieldCuisinier.SetActive(false);
        inputFieldClient.SetActive(false);
        canvasCuisinier.alpha = 0f;
        canvasClient.alpha = 0f;
    }

    CanvasGroup GetCanvasGroup(GameObject go)
    {
        CanvasGroup cg = go.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = go.AddComponent<CanvasGroup>();
        return cg;
    }

    void OnCuisinierToggleChanged(bool isOn)
    {
        StopAllCoroutines();
        if (isOn)
        {
            inputFieldCuisinier.SetActive(true);
            StartCoroutine(FadeCanvasGroup(canvasCuisinier, 0f, 1f, 0.3f));
        }
        else
        {
            StartCoroutine(FadeOutAndDisable(canvasCuisinier, 1f, 0f, 0.3f, inputFieldCuisinier));
        }
    }

    void OnClientToggleChanged(bool isOn)
    {
        StopAllCoroutines();
        if (isOn)
        {
            inputFieldClient.SetActive(true);
            StartCoroutine(FadeCanvasGroup(canvasClient, 0f, 1f, 0.3f));
        }
        else
        {
            StartCoroutine(FadeOutAndDisable(canvasClient, 1f, 0f, 0.3f, inputFieldClient));
        }
    }

    IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            cg.alpha = Mathf.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        cg.alpha = end;
    }

    IEnumerator FadeOutAndDisable(
        CanvasGroup cg,
        float start,
        float end,
        float duration,
        GameObject go
    )
    {
        yield return StartCoroutine(FadeCanvasGroup(cg, start, end, duration));
        go.SetActive(false);
    }
}
