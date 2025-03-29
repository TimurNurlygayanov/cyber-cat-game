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
            Debug.LogWarning("WaterRiseSimple: Collider2D ������ ���� 'Is Trigger'. ��������� �������������.");
            col.isTrigger = true;
        }
    }

    private void Update()
    {
        // ��������� ���� ����� ������
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag(targetTag))
        {
            hasTriggered = true;

            // �������� ����� ���������
            GameOverManager gameOver = FindObjectOfType<GameOverManager>();
            if (gameOver != null)
            {
                gameOver.TriggerGameOver();
            }
            else
            {
                Debug.LogError("GameOverScreen �� ������ � �����!");
            }
        }
    }
}
