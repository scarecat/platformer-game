using System;
using UnityEngine;

public class PermHealthPickup : PermPickup
{
    [SerializeField] private float healthIncreaseAmount;
    protected override bool ApplyEffectPerm(GameObject player)
    {
        if (player.TryGetComponent(out PlayerUpgrades upgrades))
        {
            upgrades.UnlockMaxHealth(healthIncreaseAmount);
            return true;
        }
        return false;
    }
}