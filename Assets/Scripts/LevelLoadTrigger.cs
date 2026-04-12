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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      if (other.gameObject.CompareTag("Player")) {
        levelManager.LoadLevel(levelToLoad);
      }
    }
}
