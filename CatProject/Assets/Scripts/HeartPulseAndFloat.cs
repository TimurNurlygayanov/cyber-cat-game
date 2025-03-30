using UnityEngine;

public class HeartPulseAndFloat : MonoBehaviour
{
    public float pulseSpeed = 2f;       // Скорость пульсации
    public float scaleAmount = 0.1f;    // Амплитуда пульсации

    public float floatSpeed = 1f;       // Скорость левитации
    public float floatAmount = 0.1f;    // Амплитуда левитации

    private Vector3 initialScale;
    private Vector3 initialPosition;

    void Start()
    {
        initialScale = transform.localScale;
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float time = Time.time;

        // Пульсация
        float scaleFactor = 1f + Mathf.Sin(time * pulseSpeed) * scaleAmount;
        transform.localScale = initialScale * scaleFactor;

        // Левитация вверх-вниз
        float floatOffset = Mathf.Sin(time * floatSpeed) * floatAmount;
        transform.localPosition = initialPosition + new Vector3(0f, floatOffset, 0f);
    }
}