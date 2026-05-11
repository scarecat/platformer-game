using UnityEngine;

public abstract class PermPickup : Pickup
{
    public string uniquePickupID;

    private LevelManager manager;
    private void Start()
    {
        manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        uniquePickupID = $"{manager.Level}_{transform.position.x}_{transform.position.y}";

        if (manager.PickedUpPersistentItemIds.Contains(uniquePickupID))
        {
            Destroy(gameObject);
        }
    }


    protected abstract bool ApplyEffectPerm(GameObject player) ;

    protected override bool ApplyEffect(GameObject player)
    {
        bool result = ApplyEffectPerm(player);
        if (result)
        {
           manager.PickedUpPersistentItemIds.Add(uniquePickupID);
        }
        return result;
    }

    /*
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
    */
    
    
}