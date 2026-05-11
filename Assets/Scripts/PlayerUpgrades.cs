using System;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    [Header("Current Upgrades")]
    public int totalExtraJumpUpgrades = 0;
    public float totalExtraHealthAmount = 0f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void UnlockExtraJump(int amount)
    {
        totalExtraJumpUpgrades += amount;
        playerMovement.AddExtraJumps(amount);
    }

    public void UnlockMaxHealth(float amount)
    {
        totalExtraHealthAmount += amount;
        playerHealth.IncreaseMaxHealth(amount);
    }
}
