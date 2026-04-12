using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public int damage = 1;
    
    private Collider2D colliderComponent;
    void Start()
    {
        colliderComponent = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
           EntityHealth healthComponent = collision.collider.gameObject.GetComponent<EntityHealth>();
           healthComponent.TakeDamage(damage);
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile") && other.TryGetComponent(out Projectile projectile))
        {
            projectile.Kill();
        }
        else if (other.CompareTag("Enemy") && other.TryGetComponent(out EntityHealth health))
        {
           health = other.gameObject.GetComponent<EntityHealth>();
           health.TakeDamage(damage);
        }
    }

}