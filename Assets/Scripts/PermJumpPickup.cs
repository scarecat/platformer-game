using UnityEngine;

public class PermJumpPickup : Pickup
{
    public int extraJumpsAmount = 1;
    //cokolwiek wpisać, byleby żeby było unikatowe
    [Header("Unique ID")]
    public string uniquePickupID;

    private void Start()
    {
        if (PlayerPrefs.GetInt(uniquePickupID, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    protected override bool ApplyEffect(GameObject player)
    {
        if (player.TryGetComponent(out PlayerUpgrades upgrades))
        {
            upgrades.UnlockExtraJump(extraJumpsAmount);

            PlayerPrefs.SetInt(uniquePickupID, 1);
            PlayerPrefs.Save();

            return true;
        }
        return false;
    }
}