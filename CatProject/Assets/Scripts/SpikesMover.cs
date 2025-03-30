using UnityEngine;

public class SpikesMover : MonoBehaviour
{
    public float moveDistance = 0.5f;     // Насколько вверх/вниз двигаться
    public float moveSpeed = 1f;          // Скорость движения
    public float pauseTime = 1f;          // Задержка на верхней/нижней точке

    private float real_pause_time = 0.1f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingUp = true;
    private float timer = 0f;
    private bool isPaused = false;

    void Start()
    {
        startPos = transform.localPosition;
        targetPos = startPos + Vector3.up * moveDistance;
    }

    void Update()
    {
        if (isPaused)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                isPaused = false;
                movingUp = !movingUp;
            }
            return;
        }

        Vector3 destination = movingUp ? targetPos : startPos;
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, destination) < 0.01f)
        {
            timer = pauseTime;

            if (movingUp)
            {
                timer = real_pause_time;
            }
            
            isPaused = true;
        }
    }
}