using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private GameObject player;

  [SerializeField] private string forceLevelToStartIn = null;
  [SerializeField] private string forceEntryPointToStartIn = null;
  private string currentLevel = null;

  private IEnumerator StartLoadLevel(string levelName, string entryPointName)
  {
    player.GetComponent<PlayerHealth>().enabled = false;
    player.GetComponent<PlayerMovement>().enabled = false;
    player.GetComponent<SpriteRenderer>().enabled = false;
    if (currentLevel != null)
    {
      yield return SceneManager.UnloadSceneAsync(currentLevel);
    }
    var loadOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
    yield return loadOperation;
    currentLevel = levelName;

    var spawnPos = GameObject.Find(entryPointName).transform;
    player.transform.SetPositionAndRotation(spawnPos.position, Quaternion.identity);

    player.GetComponent<PlayerHealth>().enabled = true;
    player.GetComponent<PlayerMovement>().enabled = true;
    player.GetComponent<SpriteRenderer>().enabled = true;
  }



  public void LoadLevel(string levelName, string entryPointName = null)
  {
    string spawnPointName;

    if (string.IsNullOrEmpty(entryPointName))
    {
      spawnPointName = $"From{currentLevel}";

    }
    else
    {
      spawnPointName = entryPointName;
    }
    StartCoroutine(StartLoadLevel(levelName, spawnPointName));
  }





  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    if (!string.IsNullOrEmpty(forceLevelToStartIn))
    {
      LoadLevel(forceLevelToStartIn, forceEntryPointToStartIn);
    }
    else
    {
      LoadLevel("Level1", "PlayerStart");
    }
  }

  // Update is called once per frame
  void Update()
  {

  }
}
