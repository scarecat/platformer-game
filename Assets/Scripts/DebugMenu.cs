using UnityEngine;
using UnityEngine.InputSystem;

public class DebugMenu : MonoBehaviour
{
    [Header("Menu Settings")]
    [SerializeField] private Key toggleKey = Key.F3;
    [SerializeField] private bool showMenu = false;
    [SerializeField] private float guiScale = 1.5f;
    [SerializeField] private Vector2 menuPosition = new Vector2(10, 10);

    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerEnergy playerEnergy;
    [SerializeField] private PlayerUpgrades playerUpgrades;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Spawning enemies")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnDistance = 3f;

    [Header("Other")]
    [SerializeField] private float healthStep = 10f;
    [SerializeField] private float energyStep = 25f;
    [SerializeField] private int jumpUpgradeAmount = 1;
    [SerializeField] private float healthUpgradeAmount = 20f;

    [Header("Teleport")]
    [SerializeField] private string[] levelsToTeleport = { "Level1", "Level2" };

    private Vector2 scrollPosition;

    private void Start()
    {
        if (playerHealth == null) playerHealth = FindAnyObjectByType<PlayerHealth>();
        if (playerEnergy == null) playerEnergy = FindAnyObjectByType<PlayerEnergy>();
        if (playerUpgrades == null) playerUpgrades = FindAnyObjectByType<PlayerUpgrades>();
        if (levelManager == null) levelManager = FindAnyObjectByType<LevelManager>();
        if (playerMovement == null) playerMovement = FindAnyObjectByType<PlayerMovement>();
    }

    private void Update()
    {
        var keyboard = Keyboard.current;

        if (keyboard[toggleKey].wasPressedThisFrame)
        {
            showMenu = !showMenu;
        }

        if (!showMenu) return;

        HandleDebugInputs(keyboard);
    }

    private void HandleDebugInputs(Keyboard keyboard)
    {
        if (keyboard.digit1Key.wasPressedThisFrame && playerHealth != null) playerHealth.TakeDamage(healthStep);
        if (keyboard.digit2Key.wasPressedThisFrame && playerHealth != null) playerHealth.Heal(healthStep);
        if (keyboard.digit3Key.wasPressedThisFrame && playerEnergy != null) playerEnergy.ConsumeEnergy(energyStep);
        if (keyboard.digit4Key.wasPressedThisFrame && playerEnergy != null) playerEnergy.RestoreEnergy(energyStep);
        if (keyboard.digit5Key.wasPressedThisFrame && playerUpgrades != null) playerUpgrades.UnlockExtraJump(jumpUpgradeAmount);
        if (keyboard.digit6Key.wasPressedThisFrame && playerUpgrades != null) playerUpgrades.UnlockMaxHealth(healthUpgradeAmount);
        if (keyboard.digit7Key.wasPressedThisFrame) SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0) return;

        GameObject randomEnemy = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        float direction = (playerMovement != null && playerMovement.IsFacingRight()) ? 1f : -1f;

        Vector3 spawnPos = playerHealth.transform.position + new Vector3(spawnDistance * direction, 0, 0);
        Instantiate(randomEnemy, spawnPos, Quaternion.identity);
    }

    private void OnGUI()
    {
        if (!showMenu) return;

        Matrix4x4 originalMatrix = GUI.matrix;
        GUI.matrix = Matrix4x4.Scale(new Vector3(guiScale, guiScale, 1f));

        int width = 280;
        int height = 480;

        GUI.Box(new Rect(menuPosition.x, menuPosition.y, width, height), "DEBUG MENU");

        GUILayout.BeginArea(new Rect(menuPosition.x + 10, menuPosition.y + 30, width - 20, height - 20));

        GUILayout.Label($"[{toggleKey}] Zamknij menu");
        GUILayout.Space(10);
        GUILayout.Label($"[1] Zabierz {healthStep} HP");
        GUILayout.Label($"[2] Dodaj {healthStep} HP");
        GUILayout.Space(5);
        GUILayout.Label($"[3] Zabierz {energyStep} Energii");
        GUILayout.Label($"[4] Dodaj {energyStep} Energii");
        GUILayout.Space(5);
        GUILayout.Label($"[5] Ulepszenie: +{jumpUpgradeAmount} Skok");
        GUILayout.Label($"[6] Ulepszenie: +{healthUpgradeAmount} Max HP");
        GUILayout.Space(5);
        GUILayout.Label($"[7] Zespawnuj przeciwnika");
        GUILayout.Space(10);
        if (playerHealth != null) GUILayout.Label($"Obecne HP: {playerHealth.CurrentHealth}/{playerHealth.MaxHealth}");
        if (playerEnergy != null) GUILayout.Label($"Obecna Energia: {(int)playerEnergy.CurrentEnergy}/{(int)playerEnergy.MaxEnergy}");
        GUILayout.Space(15);
        GUILayout.Label("TELEPORTACJA");

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(120));

        foreach (string levelName in levelsToTeleport)
        {
            if (GUILayout.Button($"{levelName}"))
            {
                if (levelManager != null)
                {
                    levelManager.LoadLevel(levelName);
                }
            }
        }

        GUILayout.EndScrollView();

        GUILayout.EndArea();
        GUI.matrix = originalMatrix;
    }
}