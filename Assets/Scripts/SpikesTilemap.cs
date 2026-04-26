using UnityEngine;

public class SpikesTilemap : MonoBehaviour
{
    
    [SerializeField] private float damage = 10.0f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.TryGetComponent(out PlayerHealth health)) return;
        
        health.TrapDamage(damage);
    }
}
