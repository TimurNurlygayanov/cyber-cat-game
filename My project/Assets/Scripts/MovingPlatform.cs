using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveDistance = 2f; // ��������� ������ ��������
    public float moveSpeed = 2f;    // ��������
    public bool horizontal = true;  // �����������: true = X, false = Y

    private Vector3 startPos;
    private int direction = 1;

    void Start()
    {
        startPos = transform.position;
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
