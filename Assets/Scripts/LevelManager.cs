using System.Collections;
using System.Runtime.InteropServices;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
  [SerializeField] private GameObject player;
  [SerializeField] private CinemachineConfiner2D cinemachineConfiner2D;

  [Header("Fade Transition")]
  [SerializeField] private CanvasGroup fadeCanvasGroup;
  [SerializeField] private float fadeDuration = 0.5f;
  
  private string currentLevel = null;
  private bool isLoading = false;

  private IEnumerator StartLoadLevel(string levelName, string entryPointName)
  {
    isLoading = true;
    yield return StartCoroutine(Fade(1f));

    player.SetActive(false);
    if (currentLevel != null)
    {
      yield return SceneManager.UnloadSceneAsync(currentLevel);
    }
    var loadOperation = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
    
    


    yield return loadOperation;
    var confinementObject = GameObject.FindWithTag("CinemachineConfinement");
    cinemachineConfiner2D.BoundingShape2D = confinementObject.GetComponent<Collider2D>();
    currentLevel = levelName;
    
    


    var spawnPos = GameObject.Find(entryPointName).transform;
    player.transform.SetPositionAndRotation(spawnPos.position, Quaternion.identity);
    player.SetActive(true);

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
    StartCoroutine(StartLoadLevel(levelName, spawnPointName));
  }

  private IEnumerator Fade(float targetAlpha)
  {
    if (fadeCanvasGroup == null) yield break;

    fadeCanvasGroup.blocksRaycasts = true;

    float StartAlpha = fadeCanvasGroup.alpha;
    float time = 0;

    while(time < fadeDuration)
    {
        time += Time.deltaTime;
        fadeCanvasGroup.alpha = Mathf.Lerp(StartAlpha, targetAlpha, time / fadeDuration);
        yield return null;
    }

    fadeCanvasGroup.alpha = targetAlpha;

    if(targetAlpha == 0f)
    {
        fadeCanvasGroup.blocksRaycasts = false;
    }
  }





  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    if (fadeCanvasGroup != null)
    {
        fadeCanvasGroup.alpha = 1f;
        StartCoroutine(Fade(0f));
    }
    
    if (SceneManager.sceneCount < 2) // ignore if there is a scene preloaded
    {
        LoadLevel("Level1", "PlayerStart");
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
