using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkingAnimation : MonoBehaviour
{
    TextMeshProUGUI blinkingText;
    float minOpacity = 0f;
    float maxOpacity = 1f;

    [SerializeField]
    float fadeSpeed = 1f;

    private void Start()
    {
        blinkingText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float opacity = Mathf.PingPong(Time.time * fadeSpeed, maxOpacity - minOpacity) + minOpacity;
        Color color = blinkingText.color;
        color.a = opacity;
        blinkingText.color = color;
    }
}
