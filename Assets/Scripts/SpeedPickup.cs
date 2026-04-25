using UnityEngine;

public class SpeedPickup : Pickup
{
    [Header("Effect settings")]
    public float speedMultiplier = 1.5f; //50% szybszy
    public float duration = 5f; //5 sekund

    protected override bool ApplyEffect(GameObject player)
    {
        if(player.TryGetComponent(out PlayerMovement movement))
        {
            movement.ApplySpeedBoost(speedMultiplier, duration);
            return true;
        }
        return false;
    }
}
