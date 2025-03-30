using UnityEngine;
using System.Collections;

public class PeriodicSoundPlayer : MonoBehaviour
{
    public AudioSource mainAudioSource;     // Источник звука
    public AudioSource effectAudioSource;  // Для временного звука
    public AudioClip backgroundMusic;       // Основная музыка
    public AudioClip periodicSound;         // Звук по таймеру
    public float interval = 60f;            // Интервал в секундах

    private float timer;


    void Start()
    {
        if (mainAudioSource == null)
            mainAudioSource = GetComponent<AudioSource>();

        mainAudioSource.clip = backgroundMusic;
        mainAudioSource.loop = true;
        mainAudioSource.volume = 0.3f;
        mainAudioSource.Play();

        timer = 1f; // ⏱ Первый запуск периодического звука через 1 секунду
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            mainAudioSource.PlayOneShot(periodicSound);
            timer = interval; // ⏱ Следующий запуск через N секунд
        }
    }
    
    public void PlayTemporarySound()
    {
        StartCoroutine(PlayEffectAndFadeBackground());
    }

    IEnumerator PlayEffectAndFadeBackground()
    {
        // Уменьшаем громкость фоновой музыки
        mainAudioSource.volume = 0.2f;

        // Воспроизводим временный звук
        effectAudioSource.clip = periodicSound;
        effectAudioSource.Play();

        // Ждём пока временный звук не закончится
        yield return new WaitForSeconds(periodicSound.length);

        // Возвращаем громкость фоновой музыки
        mainAudioSource.volume = 0.9f;
    }
}
