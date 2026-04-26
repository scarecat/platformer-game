using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerEnergy))]
[RequireComponent(typeof(PlayerSafePositionRecorder))]
[RequireComponent(typeof(PlayerMovement))] // TODO: too many dependencies
public class PlayerHealth : EntityHealth
{
    private PlayerEnergy playerEnergy;
    private PlayerSafePositionRecorder safe;
    private PlayerMovement player;
    
    public bool hasShield = false;

    protected override void Start()
    {
        base.Start();
        playerEnergy = GetComponent<PlayerEnergy>();
        safe = GetComponent<PlayerSafePositionRecorder>();
        player = GetComponent<PlayerMovement>();
    }

    protected override void SpawnOnHitObject() {
      Instantiate(onHitObject, transform.position + Vector3.up * 0.5f, transform.rotation);
    }

    public override bool TakeDamage(float amount, Vector2 damageDirection)
    {
        if (onCooldown) return false;

        PlayerMovement player = GetComponent<PlayerMovement>();
        player.Knockback(damageDirection);


        if (player != null && player.playerState == PlayerState.Blocking)
        {
            float facingDir = player.IsFacingRight() ? 1f : -1f;
            float dot = Vector2.Dot(new Vector2(facingDir, 0), damageDirection.normalized);

            if (dot < 0f)
            {
                playerEnergy.ConsumeEnergy(25f);
                return false;
            }
        }

        return TakeDamage(amount); 
    }

    public void TrapDamage(float amount)
    {
        TakeDamage(amount);
        player.SetPosition(safe.SafePosition);
    }


    public override bool TakeDamage(float amount)
    {
        if (onCooldown) return false;
        if (!IsAlive) return false;

        if(hasShield)
        {
            hasShield = false;

            StartCoroutine(HandleCooldown());
            return false;
        }

        SpawnOnHitObject();
        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0f)
        {
            //Die();
            FindAnyObjectByType<GameOverScreen>().ShowGameOverScreen();
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

    public void ActivateShield()
    {
        hasShield = true;
    }
}
