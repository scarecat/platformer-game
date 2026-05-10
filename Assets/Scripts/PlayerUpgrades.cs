using System;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;

    [Header("Current Upgrades")]
    public int totalExtraJumps = 0;
    public float totalExtraHealth = 0f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    void Start()
    {
        LoadUpgrades();
    }

    public void UnlockExtraJump(int amount)
    {
        totalExtraJumps += amount;
        playerMovement.AddPermanentExtraJump(amount);
        SaveUpgrades();
    }

    public void UnlockMaxHealth(float amount)
    {
        totalExtraHealth += amount;
        playerHealth.IncreaseMaxHealth(amount);
        SaveUpgrades();
    }

    private void SaveUpgrades()
    {
        PlayerPrefs.SetInt("Perm_ExtraJumps", totalExtraJumps);
        PlayerPrefs.SetFloat("Perm_BonusHealth", totalExtraHealth);
        PlayerPrefs.Save();
    }

    private void LoadUpgrades()
    {
        totalExtraJumps = PlayerPrefs.GetInt("Perm_ExtraJumps", 0);
        if(totalExtraJumps > 0)
        {
            playerMovement.AddPermanentExtraJump(totalExtraJumps);
        }

        totalExtraHealth = PlayerPrefs.GetFloat("Perm_BonusHealth", 0);
        if(totalExtraHealth > 0)
        {
            playerHealth.IncreaseMaxHealth(totalExtraHealth);
        }
    }

    [ContextMenu("Reset Upgrades")]
    public void ResetUpgrades()
    {
        PlayerPrefs.DeleteKey("Perm_ExtraJumps");
        PlayerPrefs.DeleteKey("Perm_BonusHealth");
        PlayerPrefs.DeleteKey("ExtraJump");
    }
}
