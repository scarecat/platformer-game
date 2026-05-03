using UnityEngine;

public class HealthPickup : Pickup
{
    [SerializeField] private float healAmount = 25f;
    public AudioClip healPickup;

    protected override bool ApplyEffect(GameObject player)
    {
        //PlayerHealth playerHealth = player.GetComponentInParent<PlayerHealth>();

        if(player.TryGetComponent(out PlayerHealth playerHealth))
        {
            if(playerHealth.CurrentHealth < playerHealth.MaxHealth)
            {
                AudioManager.Instance.PlaySFX(healPickup);
                playerHealth.Heal(healAmount);
                return true;
            }
        }
        return false;
    }
}
