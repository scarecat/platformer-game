using UnityEngine;

public class PermJumpPickup : PermPickup
{
    [SerializeField] private int extraJumpsAmount;

    protected override bool ApplyEffectPerm(GameObject player)
    {
        if (player.TryGetComponent(out PlayerUpgrades upgrades))
        {
            upgrades.UnlockExtraJump(extraJumpsAmount);
            return true;
        }
        return false;
    }
}