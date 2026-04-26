using UnityEngine;

public class HealthTester : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private bool autoDamage = false;

    private float timer = 0f;

    private void Update()
    {
        if (!autoDamage) return;
        if (playerHealth == null) return;
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            timer = 0f;
            playerHealth.TakeDamage(damagePerSecond);
            Debug.Log("HP: " + playerHealth.CurrentHealth);
        }
    }
}