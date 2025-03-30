using UnityEngine;

public class AutoShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootInterval = 2f;
    public float bulletSpeed = 5f;
    public Vector3 fireOffset = Vector3.zero;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    void Shoot()
    {
        Vector3 spawnPos = transform.position + fireOffset;

        // Спавним пулю без поворота (или поворачиваем, если спрайт смотрит влево)
        GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);

        // Устанавливаем движение вправо
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.right * bulletSpeed;
        }
    }
}