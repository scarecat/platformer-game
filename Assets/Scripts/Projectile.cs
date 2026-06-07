using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Projectile : MonoBehaviour
{
    public Vector2 direction;
    public bool isPlayerProjectile = false;
    public bool allowPlayerKill = true;

    [SerializeField]
    private float damage = 4.0f;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float lifetime = 4.0f;

    private SpriteRenderer spriteRenderer;
    private bool invertSpriteFlip = false;

    private Animator animator;
    private bool isDestroyed = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        invertSpriteFlip = spriteRenderer.flipX;
        spriteRenderer.flipX = invertSpriteFlip ? direction.x > 0 : direction.x < 0;
        //Invoke(nameof(Kill), lifetime);
        Destroy(gameObject, lifetime);
    }

    public void Kill()
    {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDestroyed) { return; }
        transform.position = (Vector2)transform.position + (speed * Time.deltaTime * direction);
        //spriteRenderer.flipX = invertSpriteFlip ? direction.x > 0 : direction.x < 0;
    }

    void Handle(GameObject hitObject)
    {
        if (isPlayerProjectile)
        {
            animator = GetComponent<Animator>();
            animator.SetBool("isDestroyed", false);
            if (hitObject.CompareTag("Enemy"))
            {
                if (hitObject.TryGetComponent(out EntityHealth health))
                health.TakeDamage(damage, hitObject.transform.position - transform.position);
                animator.SetBool("isDestroyed", true);
                StartCoroutine(DestroyFireball());
            }
            else if (!hitObject.CompareTag("Player"))
            {
                animator.SetBool("isDestroyed", true);
                StartCoroutine(DestroyFireball());
            }
        }
        else
        {
            if (hitObject.CompareTag("Player"))
            {
                if (!hitObject.TryGetComponent(out EntityHealth health)) { return; }
                health.TakeDamage(damage, hitObject.transform.position - transform.position);
                Kill();
            }
            else if (!hitObject.CompareTag("Enemy"))
            {
                Kill();
            }
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

    public void PlayerKill()
    {
        if (allowPlayerKill)
        {
            Kill();
        }

    }

    private IEnumerator DestroyFireball()
    {
        isDestroyed = true;
        yield return new WaitForSeconds(0.73f);
        Kill();
    }
}