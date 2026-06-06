using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage;
    public float knockbackForce = 1.0f;

    private PlayerHealth playerHealth;


    void Start() {
      playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            var dirToPlayer = collision.gameObject.transform.position - transform.position;
            playerHealth.TakeDamage(damage, dirToPlayer.normalized, knockbackForce);
        }
    }
}