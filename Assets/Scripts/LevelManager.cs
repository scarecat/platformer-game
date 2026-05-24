using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using Unity.Cinemachine;
using UnityEditor.Networking.PlayerConnection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private bool newGame = false;

    [Header("Fade Transition")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    
    private string currentLevel = null;
    private string currentEntryPoint = null;
    public string Level => currentLevel;
    public string EntryPoint => currentEntryPoint;
    private bool isLoading = false;
    
    private List<string> killedPersistentEnemyIds;
    private List<string> pickedUpPersistentItemIds;
    public List<string> KilledPersistentEnemyIds => killedPersistentEnemyIds;
    public List<string> PickedUpPersistentItemIds => pickedUpPersistentItemIds;

    private IEnumerator LoadLevelCoroutine(string levelName, string entryPointName)
    {
        isLoading = true;

        //player.SetActive(false);
        if (currentLevel != null)
        {
            yield return StartCoroutine(Fade(1f));
            yield return SceneManager.UnloadSceneAsync(currentLevel);
        }
        else
        {
            SetFade(1f);
        }
        player.SetActive(false);
        var loadOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        yield return loadOperation;
        var confinementObject = GameObject.FindWithTag("CinemachineConfinement");
        cinemachineConfiner2D.BoundingShape2D = confinementObject.GetComponent<Collider2D>();


        currentLevel = levelName;
        currentEntryPoint = entryPointName;
        
        SaveSystem.SaveGame(player.GetComponent<PlayerHealth>(), player.GetComponent<PlayerUpgrades>(), this);

        var spawnPos = GameObject.Find(entryPointName).transform;

        var playerCollider = player.GetComponent<Collider2D>();
        player.transform.SetPositionAndRotation(spawnPos.position + Vector3.up * 0.2f, Quaternion.identity);
        player.SetActive(true);
        //player.SetActive(true);
        cinemachineCamera.CancelDamping();


        yield return StartCoroutine(Fade(0f));
        isLoading = false;
    }

    public void LoadLevel(string levelName, string entryPointName = null)
    {
        if (isLoading) return;

        string spawnPointName;

        if (string.IsNullOrEmpty(entryPointName))
        {
            spawnPointName = $"From{currentLevel}";

        }
        else
        {
            spawnPointName = entryPointName;
        }
        StartCoroutine(LoadLevelCoroutine(levelName, spawnPointName));
    }
    
    private void SetFade(float alpha)
    {
        if (fadeCanvasGroup == null) return;
        fadeCanvasGroup.alpha = alpha;
        fadeCanvasGroup.blocksRaycasts = alpha != 0f;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        if (fadeCanvasGroup == null) yield break;

        fadeCanvasGroup.blocksRaycasts = true;

        float StartAlpha = fadeCanvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(StartAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = targetAlpha;

        if (targetAlpha == 0f)
        {
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        var player = GameObject.Find("Player");
        var playerHealth = player.GetComponent<PlayerHealth>();
        var playerUpgrades = player.GetComponent<PlayerUpgrades>();


        var currentSaveData = SaveSystem.LoadGame();
        if(currentSaveData == null || newGame) {
            currentSaveData = new SaveData {
                currentLevel = "Level1",
                entryPoint = "PlayerStart",
                playerHealth = playerHealth.MaxHealth,
                killedPersistentEnemyIds = new string[0],
                pickedUpPersistentItemIds = new string[0],
                totalExtraHealthAmount = 0,
                totalExtraJumpUpgrades = 0,
            };
        }

        playerHealth.SetHealth(currentSaveData.playerHealth);
        playerUpgrades.UnlockExtraJump(currentSaveData.totalExtraJumpUpgrades);
        playerUpgrades.UnlockMaxHealth(currentSaveData.totalExtraHealthAmount);

        killedPersistentEnemyIds = currentSaveData.killedPersistentEnemyIds.ToList();
        pickedUpPersistentItemIds = currentSaveData.pickedUpPersistentItemIds.ToList();
         

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
            StartCoroutine(Fade(0f));
        }

        if (SceneManager.sceneCount < 2) // ignore if there is a scene preloaded
        {
            LoadLevel(currentSaveData.currentLevel, currentSaveData.entryPoint);
        }
        else
        {
            currentLevel = SceneManager.GetSceneAt(1).name;
            cinemachineConfiner2D.BoundingShape2D = GameObject.FindWithTag("CinemachineConfinement").GetComponent<Collider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
