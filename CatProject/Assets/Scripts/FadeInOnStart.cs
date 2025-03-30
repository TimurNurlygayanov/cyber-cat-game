using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOnStart : MonoBehaviour
{
    public Image fadeOverlay;
    public float fadeDuration = 1f;

    void Start()
    {
        if (fadeOverlay != null)
            StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeOverlay.color = new Color(0, 0, 0, 0);
    }
}
