using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Ustawienia zdrowia")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [SerializeField] private bool dieOnZeroHealth = true;

    public UnityEvent<float, float> OnHealthChanged;
    public UnityEvent OnDeath;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float HealthPercent => currentHealth / maxHealth;
    public bool IsAlive => currentHealth > 0;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount, Vector2 damageDirection)
    {
        PlayerMovement movement = GetComponent<PlayerMovement>();

        if (movement != null && movement.IsBlocking())
        {
            float facingDir = movement.IsFacingRight() ? 1f : -1f;
            float dot = Vector2.Dot(new Vector2(facingDir, 0), damageDirection.normalized);

            if (dot > 0f)
            {
                Debug.Log("block");
                return;
            }
        }

        TakeDamage(amount); 
    }
    public void TakeDamage(float amount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void SetHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        OnDeath?.Invoke();

        if (dieOnZeroHealth)
        {
            Debug.Log("Gracz zginął!");
            gameObject.SetActive(false);
        }
    }
}