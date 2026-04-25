using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private float healAmount = 25f;

    protected override bool ApplyEffect(GameObject player)
    {
        //PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();

        if(player.TryGetComponent(out PlayerHealth playerHealth))
        {
            if(playerHealth.CurrentHealth < playerHealth.MaxHealth)
            {
                playerHealth.Heal(healAmount);
                return true;
            }
        }
        return false;
    }
}
