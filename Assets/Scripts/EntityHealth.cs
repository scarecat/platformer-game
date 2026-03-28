using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class EntityHealth : MonoBehaviour, IHealth
{
    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool dieOnZeroHealth = true;
    [SerializeField] public float cooldownTime = 0.25f;

    public UnityEvent<float, float> OnHealthChanged;
    public UnityEvent OnDeath;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float HealthPercent => currentHealth / maxHealth;
    public bool IsAlive => currentHealth > 0;

    public bool onCooldown;

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public virtual void TakeDamage(float amount, Vector2 damageDirection)
    {
        if (onCooldown) return;
        TakeDamage(amount); 
    }
    public virtual void TakeDamage(float amount)
    {
        if (onCooldown) return;
        if (!IsAlive) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
            return;
        }
        
        StartCoroutine(HandleCooldown());
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