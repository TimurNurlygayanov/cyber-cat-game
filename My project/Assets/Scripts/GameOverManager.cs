using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    public Image fadeOverlay;
    public TMP_Text countdownText;
    public float fadeDuration = 1f;
    public float countdownInterval = 1f;

    public void TriggerGameOver()
    {
        StartCoroutine(FadeAndRestart());
    }

    private IEnumerator FadeAndRestart()
    {
        fadeOverlay.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);

        yield return StartCoroutine(FadeOverlay(0f, 1f));

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(countdownInterval);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator FadeOverlay(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(from, to, t / fadeDuration);
            fadeOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeOverlay.color = new Color(0, 0, 0, to);
    }
}
