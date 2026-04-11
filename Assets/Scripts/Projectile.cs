using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    public Vector2 direction;

    [SerializeField]
    private float damage = 4.0f;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float lifetime = 4.0f;

    private SpriteRenderer spriteRenderer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Invoke(nameof(Kill), lifetime);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = (Vector2)transform.position + (speed * Time.deltaTime * direction);
        spriteRenderer.flipX = direction.x < 0;
    }

    void Handle(GameObject hitObject)
    {
        if (hitObject.CompareTag("Player"))
        {
            if (!hitObject.TryGetComponent(out EntityHealth health)) { return; }
            health.TakeDamage(damage, hitObject.transform.position - transform.position);
        }
        if (!hitObject.CompareTag("Enemy"))
        {
            Kill();
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        Handle(collision.collider.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Handle(collider.gameObject);
    }
}

