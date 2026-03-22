using UnityEngine;

public class LevelLoadTrigger : MonoBehaviour
{

    [SerializeField] public string levelToLoad;
    [SerializeField] public string entryPointName;

    private LevelManager levelManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
      Debug.Log("Trigger in scene");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      Debug.Log("Trigger entered");
      if (other.gameObject.CompareTag("Player")) {
        Debug.Log("Player is here.");
        levelManager.LoadLevel(levelToLoad);
      }
    }
}
