using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [Header("UI")]
    public Image fadeOverlay;
    public Image rotatingImage; // ⬅️ Картинка, которая будет вращаться
    public float fadeDuration = 1f;

    [Header("Rotation")]
    public float rotationSpeed = 180f; // скорость в градусах/сек

    [Header("Timing")]
    public float rotationDuration = 3f;

    [Header("Audio")]
    public AudioClip gameOverMusic;
    public AudioSource musicSource;


    public void TriggerGameOver()
    {
        AudioListener.pause = true;

        // Играем музыку поражения
        if (musicSource != null && gameOverMusic != null)
        {
            musicSource.ignoreListenerPause = true; // ⬅️ это ключ
            musicSource.clip = gameOverMusic;
            musicSource.Play();
        }

        StartCoroutine(FadeAndRotate());
    }


    private IEnumerator FadeAndRotate()
    {
        fadeOverlay.gameObject.SetActive(true);
        rotatingImage.gameObject.SetActive(true);

        // Прозрачность в 0
        SetImageAlpha(rotatingImage, 0f);

        // Затемнение
        yield return StartCoroutine(FadeOverlay(0f, 1f));

        // Проявление картинки
        yield return StartCoroutine(FadeImageIn(rotatingImage, 1f, 0.5f));


        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            rotatingImage.transform.Rotate(Vector3.forward, -rotationSpeed * Time.unscaledDeltaTime);
            yield return null;
        }


        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        AudioListener.pause = false;
    }

    private IEnumerator FadeOverlay(float from, float to)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(from, to, t / fadeDuration);
            fadeOverlay.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
    }

    private IEnumerator FadeImageIn(Image image, float toAlpha, float duration)
    {
        float t = 0f;
        Color c = image.color;
        float fromAlpha = c.a;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Lerp(fromAlpha, toAlpha, t / duration);
            image.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        image.color = new Color(c.r, c.g, c.b, toAlpha);
    }

    private void SetImageAlpha(Image img, float alpha)
    {
        var c = img.color;
        img.color = new Color(c.r, c.g, c.b, alpha);
    }
}
