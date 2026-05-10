using UnityEngine;

[RequireComponent(typeof(EntityHealth))]
public class PersistentEnemy : MonoBehaviour
{
    public void Start()
    {
        var manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        var id = $"{manager.Level}_{transform.position.x}_{transform.position.y}";

        if(manager.KilledPersistentEnemyIds.Contains(id))
        {
            Destroy(gameObject);
            return;
        }

        var health = GetComponent<EntityHealth>();
        health.OnDeath.AddListener(() =>
        {
            manager.KilledPersistentEnemyIds.Add(id);
        });
    }
}