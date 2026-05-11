using System;

[Serializable]
public class SaveData
{
    public float playerHealth;
    public string currentLevel;
    public string entryPoint;
    public string[] killedPersistentEnemyIds;
    public string[] pickedUpPersistentItemIds;

    public int totalExtraJumpUpgrades = 0;
    public float totalExtraHealthAmount = 0f;
}
