using UnityEngine;

public class LifePickup : MonoBehaviour
{
    public GameObject pickupEffect; // Префаб частицы/свечения
    public AudioClip pickupSound;   // Звук при сборе
    public int healAmount = 1;      // Сколько жизней добавляется

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cat"))
        {
            // Эффект
            if (pickupEffect != null)
            {
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
            }

            // Звук
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Добавить жизнь (если у игрока есть логика жизней)
            GameManager manager = Object.FindFirstObjectByType<GameManager>();
            if (manager != null)
                manager.AddLife();

            // Удалить бонус
            Destroy(gameObject);
        }
    }
}