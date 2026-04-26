using UnityEngine;

public class MechanicalSpike : MonoBehaviour
{

    public bool spikesUp = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spikesUp && collision.gameObject.TryGetComponent(out PlayerHealth player))
        {
            player.TrapDamage(20);
        }
    }
}
