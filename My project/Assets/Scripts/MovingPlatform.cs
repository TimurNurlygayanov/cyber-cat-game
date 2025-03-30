using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 4f; // ��������� ������ ��������
    public float moveSpeed = 2f;    // ��������
    public bool horizontal = true;  // �����������: true = X, false = Y

    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;

        this.moveSpeed = Random.Range(0.5f, 3.0f);

        if (Random.Range(0f, 1f) < 0.5)
        {
            this.direction = -1;
        }
    }

    void Update()
    {
        Vector3 offset = horizontal ? Vector3.right : Vector3.up;
        transform.Translate(offset * moveSpeed * direction * Time.deltaTime);

        if (Vector3.Distance(startPos, transform.position) >= moveDistance)
        {
            direction *= -1;
        }
    }
}
