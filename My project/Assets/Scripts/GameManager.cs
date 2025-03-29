using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public TextMeshProUGUI livesText;
    public TextMeshProUGUI scoreText;

    [Header("Gameplay")]
    public int startingLives = 3;

    private int lives;
    private int score;

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
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            Debug.Log("Game Over!");
            GameOverManager manager = Object.FindFirstObjectByType<GameOverManager>();
            if (manager != null)
                manager.TriggerGameOver();
        }
    }

    void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;

        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }
}
