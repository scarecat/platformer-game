using UnityEngine;

public class ShieldPickup : Pickup
{
    protected override bool ApplyEffect(GameObject player)
    {
        if (player.TryGetComponent(out PlayerHealth playerHealth))
        {
            if(playerHealth.hasShield)
            {
                return false;
            }

            playerHealth.ActivateShield();
            return true;
        }
        return false;
    }
}