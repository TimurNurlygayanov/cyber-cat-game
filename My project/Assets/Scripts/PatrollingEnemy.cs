using System.Collections;
using UnityEngine;

public class PatrollingEnemyOnPlatform : MonoBehaviour
{
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;
    [SerializeField] private float pauseDuration = 1f;
    [SerializeField, Range(0f, 0.5f)] private float edgeMarginPercent = 0.1f; // 10% от ширины

    private float direction;
    private float speed;
    private float leftEdge;
    private float rightEdge;
    private bool isPaused = false;

    void Start()
    {
        direction = Random.value < 0.5f ? -1f : 1f;
        speed = Random.Range(minSpeed, maxSpeed);

        BoxCollider2D platformCollider = transform.parent.GetComponent<BoxCollider2D>();
        if (platformCollider != null)
        {
            Vector3 platformPos = transform.parent.position;
            float fullWidth = platformCollider.size.x * transform.parent.localScale.x;
            float margin = fullWidth * edgeMarginPercent;

            float halfWidth = fullWidth / 2f;
            leftEdge = platformPos.x - halfWidth + margin;
            rightEdge = platformPos.x + halfWidth - margin;
        }
        else
        {
            Debug.LogError("Платформа не имеет BoxCollider2D!");
        }
    }

    void Update()
    {
        if (isPaused) return;

        transform.position += Vector3.right * direction * speed * Time.deltaTime;

        if (transform.position.x < leftEdge || transform.position.x > rightEdge)
        {
            StartCoroutine(PauseAndTurn());
        }
    }

    IEnumerator PauseAndTurn()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        direction *= -1f;
        Flip();
        isPaused = false;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction); // поворот строго по направлению
        transform.localScale = scale;
    }
}