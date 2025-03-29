using UnityEngine;

public class SpikeActivator : MonoBehaviour
{
    public GameObject spikeObject;

    public float delta_h = 0.2f;
    private bool activated = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!activated && other.CompareTag("Cat"))
        {
            activated = true;

            if (spikeObject != null)
            {
                spikeObject.SetActive(true);
            }
        }
    }
}