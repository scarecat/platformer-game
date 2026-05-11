using UnityEngine;

public class PermHealthPickup : Pickup
{
    public float healthIncreaseAmount = 25f;
    //cokolwiek wpisaæ, byleby ¿eby by³o unikatowe
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
            upgrades.UnlockMaxHealth(healthIncreaseAmount);

            PlayerPrefs.SetInt(uniquePickupID, 1);
            PlayerPrefs.Save();

            return true;
        }
        return false;
    }
}