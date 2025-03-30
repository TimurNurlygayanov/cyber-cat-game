using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WaterRiseSimple : MonoBehaviour
{
    public float riseSpeed = 2f;
    public string targetTag = "Cat";
    private bool hasTriggered = false;

    private void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
        {
            col.isTrigger = true;
            Debug.Log("WaterRise: IsTrigger ������� �������������.");
        }
    }

    private void Update()
    {
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag(targetTag))
        {
            hasTriggered = true;

            // ������������ ����
            CatController cat = other.GetComponent<CatController>();
            if (cat != null)
            {
                cat.FreezeCat();
            }

            // �������� UI-������, ���� GameManager ������
            GameManager gm = GameManager.Instance;
            if (gm != null)
            {
                if (gm.livesPanel != null) gm.livesPanel.SetActive(false);
                if (gm.scorePanel != null) gm.scorePanel.SetActive(false);
            }

            // ��������� GameOver
            GameOverManager manager = Object.FindFirstObjectByType<GameOverManager>();
            if (manager != null)
            {
                manager.TriggerGameOver();
            }
            else
            {
                Debug.LogError("GameOverManager �� ������ � �����!");
            }
        }
    }
}
