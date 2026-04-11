using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : EntityHealth
{
    public override void TakeDamage(float amount, Vector2 damageDirection)
    {
        if (onCooldown) return;

        PlayerMovement player = GetComponent<PlayerMovement>();
        player.Knockback(damageDirection);


        if (player != null && player.playerState == PlayerState.Blocking)
        {
            float facingDir = player.IsFacingRight() ? 1f : -1f;
            float dot = Vector2.Dot(new Vector2(facingDir, 0), damageDirection.normalized);

            if (dot > 0f)
            {
                Debug.Log("block");
                return;
            }
        }

        TakeDamage(amount); 
    }
    public override void TakeDamage(float amount)
    {
        if (onCooldown) return;
        if (!IsAlive) return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            //Die();
            FindAnyObjectByType<GameOverScreen>().ShowGameOverScreen();
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

}