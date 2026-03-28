using UnityEngine;

public class MonsterDamage : MonoBehaviour
{
    public int damage;

    private IHealth playerHealth;


    void Start() {
      playerHealth = GameObject.Find("Player").GetComponent<IHealth>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            playerHealth.TakeDamage(damage, -collision.relativeVelocity.normalized);
        }
    }
}
