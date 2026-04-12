using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool dieOnZeroHealth = true;
    [SerializeField] public float cooldownTime = 0.25f;
    [SerializeField] public GameObject onHitObject;

    public UnityEvent<float, float> OnHealthChanged;
    public UnityEvent OnDeath;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float HealthPercent => currentHealth / maxHealth;
    public bool IsAlive => currentHealth > 0;
    public bool onCooldown;

    private SpriteRenderer spriteRenderer;

    protected virtual void SpawnOnHitObject() {
        Instantiate(onHitObject, gameObject.transform);
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }


    protected IEnumerator ColorHurtSprite() {
      spriteRenderer.color = Color.red;
      yield return new WaitForSeconds(0.25f);
      spriteRenderer.color = Color.white;
    }

    void Start() {
      spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public virtual bool TakeDamage(float amount, Vector2 damageDirection)
    {
        if (onCooldown) return false;
        return TakeDamage(amount); 
    }
    public virtual bool TakeDamage(float amount)
    {
        if (onCooldown) return false;
        if (!IsAlive) return false;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        SpawnOnHitObject();
        StartCoroutine(ColorHurtSprite());
        if (currentHealth <= 0f)
        {
            Die();
            return true;
        }
        StartCoroutine(HandleCooldown());
        return true;
    }

    private IEnumerator HandleCooldown()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        onCooldown = false;
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

    protected void Die()
    {
        OnDeath?.Invoke();

        if (dieOnZeroHealth)
        {
            Debug.Log("Dead!");
            gameObject.SetActive(false);
        }
    }
}
