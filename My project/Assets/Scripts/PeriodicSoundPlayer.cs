using UnityEngine;

public class PeriodicSoundPlayer : MonoBehaviour
{
    public AudioSource mainAudioSource;     // Источник звука
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
}
