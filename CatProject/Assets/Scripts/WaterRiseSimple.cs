using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class WaterRiseSimple : MonoBehaviour
{
    public float initialRiseSpeed = 0.1f; // скорость первые 3 секунды
    public float afterRiseSpeed = 0.25f;   // скорость после 3 секунд
    public float switchTime = 5f;       // время смены скорости
    public string targetTag = "Cat";
    private float timer = 0f;
    private bool hasTriggered = false;
    private void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.Log("WaterRise: IsTrigger включён автоматически.");
        }
    }
    private void Update()
    {
        timer += Time.deltaTime;
        float currentSpeed = timer < switchTime ? initialRiseSpeed : afterRiseSpeed;
        transform.position += Vector3.up * currentSpeed * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag(targetTag))
        {
            hasTriggered = true;
            // Замораживаем кота
            CatController cat = other.GetComponent<CatController>();
            if (cat != null)
            {
                cat.FreezeCat();
            }
            // Скрываем UI-панели, если GameManager найден
            GameManager gm = GameManager.Instance;
            if (gm != null)
            {
                if (gm.livesPanel != null) gm.livesPanel.SetActive(false);
                if (gm.scorePanel != null) gm.scorePanel.SetActive(false);
            }
            // Запускаем GameOver
            GameOverManager manager = Object.FindFirstObjectByType<GameOverManager>();
            if (manager != null)
            {
                manager.TriggerGameOver();
            }
            else
            {
                Debug.LogError("GameOverManager не найден в сцене!");
            }
        }
    }
}