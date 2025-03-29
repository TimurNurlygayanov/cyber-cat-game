using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class WaterRiseSimple : MonoBehaviour
{
    [Header("Movement Settings")]
    public float riseSpeed = 2f;

    [Header("Target Settings")]
    public string targetTag = "Cat";
    public float restartDelay = 2f;

    private bool hasTriggered = false;

    private void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            Debug.LogWarning("WaterRiseSimple: Collider2D должен быть 'Is Trigger'. Исправляю автоматически.");
            col.isTrigger = true;
        }
    }

    private void Update()
    {
        // Жижа поднимается всегда
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag(targetTag))
        {
            hasTriggered = true;
            Debug.Log("Кот утонул. Перезапуск через " + restartDelay + " секунд...");
            Invoke(nameof(RestartLevel), restartDelay);
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
