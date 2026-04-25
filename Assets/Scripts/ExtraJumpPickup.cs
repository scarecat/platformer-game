using UnityEngine;

public class ExtraJumpPickup : Pickup
{
    [Header("Effect settings")]
    public int extraJumps = 1;  //Daje triple jump (na tê chwilê)
    public float duration = 30f; //30 sekund

    protected override bool ApplyEffect(GameObject player)
    {
        if (player.TryGetComponent(out PlayerMovement movement))
        {
            movement.ApplyExtraJump(extraJumps, duration);
            return true;
        }
        return false;
    }
}