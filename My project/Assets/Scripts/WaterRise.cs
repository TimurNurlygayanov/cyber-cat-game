using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WaterRiseSimple : MonoBehaviour
{
    [Header("Movement Settings")]
    public float riseSpeed = 2f;

    [Header("Target Settings")]
    public string targetTag = "Cat";

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
        // Поднимаем воду вверх всегда
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag(targetTag))
        {
            hasTriggered = true;

            // Вызываем экран поражения
            GameOverManager gameOver = FindObjectOfType<GameOverManager>();
            if (gameOver != null)
            {
                gameOver.TriggerGameOver();
            }
            else
            {
                Debug.LogError("GameOverScreen не найден в сцене!");
            }
        }
    }
}
