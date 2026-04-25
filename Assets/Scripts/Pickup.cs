using UnityEngine;

public abstract class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            bool wasUsed = ApplyEffect(other.gameObject);

            if (wasUsed)
            {
                Destroy(gameObject);
            }
        }
    }

    protected abstract bool ApplyEffect(GameObject player);
}