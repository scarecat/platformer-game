using UnityEngine;

public class MiniBossDamage : MonoBehaviour
{
    public int damage;
    public float knockbackForce = 1.0f;

    private PlayerHealth playerHealth;


    void Start() {
      playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            var dirToPlayer = collider.gameObject.transform.position - transform.position;
            playerHealth.TakeDamage(damage, dirToPlayer.normalized, knockbackForce);
        }
    }
}