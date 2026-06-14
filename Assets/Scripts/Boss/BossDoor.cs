using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    private LevelLoadTrigger bossTrigger;
    private List<EntityHealth> enemies;
    private int killCount;
    private int toKill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossTrigger = GetComponent<LevelLoadTrigger>();
        bossTrigger.enabled = false;
        enemies = GameObject.FindGameObjectsWithTag("Enemy").Select(x=>x.GetComponent<EntityHealth>()).ToList();
        toKill = enemies.Count;
        foreach (var enemy in enemies)
        {
            enemy.OnDeath.AddListener(() => { killCount++; CheckKilled();});
        }
        CheckKilled();
    }

    private void CheckKilled()
    {
        if (toKill == 0 || killCount >= toKill || true)
        {
            bossTrigger.enabled = true;
            transform.Find("OpenSprite").GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
