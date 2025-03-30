using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;
    public GameObject livesPanel; // ← Панель жизней
    public GameObject scorePanel; // ← Панель очков

    [Header("Gameplay")]
    public int startingLives = 3;

    private int lives;
    private int score;
    private bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        lives = startingLives;
        score = 0;
        isGameOver = false;
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateUI();
    }

    public void LoseLife()
    {
        if (isGameOver) return;

        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            isGameOver = true;

            // Скрыть панели
            if (livesPanel != null) livesPanel.SetActive(false);
            if (scorePanel != null) scorePanel.SetActive(false);

            Debug.Log("Game Over!");
            GameOverManager manager = FindFirstObjectByType<GameOverManager>();
            if (manager != null)
                manager.TriggerGameOver();
        }
    }

    public void AddLife()
    {
        if (isGameOver) return;

        lives++;
        if (lives > 9) lives = 9;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();

        if (livesText != null)
            livesText.text = lives.ToString();
    }
}
