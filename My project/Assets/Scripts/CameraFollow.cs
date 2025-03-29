using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Кот
    private float highestY;

    void Start()
    {
        highestY = transform.position.y;
    }

    void LateUpdate()
    {
        if (target.position.y > highestY)
        {
            highestY = target.position.y;
            transform.position = new Vector3(transform.position.x, highestY, transform.position.z);
        }
    }
}
