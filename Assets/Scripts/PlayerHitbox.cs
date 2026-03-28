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
        if (other.CompareTag("Enemy"))
        {
           EntityHealth healthComponent = other.gameObject.GetComponent<EntityHealth>();
           healthComponent.TakeDamage(damage);
        }
        
    }

}