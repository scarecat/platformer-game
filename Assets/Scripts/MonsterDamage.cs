using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage;

    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 damageDirection = (collision.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(damage, damageDirection);
        }
    }
}