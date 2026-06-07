using System;
using UnityEngine;

public class BossLevel : MonoBehaviour
{
    
    private EntityHealth bossHealth;
    [SerializeField] private WinScreen winScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossHealth = GameObject.Find("Boss").GetComponent<EntityHealth>();
        bossHealth.OnDeath.AddListener(OnBossDeath);
        bossHealth.OnHealthChanged.AddListener(OnBossHealthChanged);
    }

    private void OnBossHealthChanged(float health, float maxHealth)
    {
        Debug.Log($"Boss health: {health}/{maxHealth}");
    }

    private void OnBossDeath()
    {
        if(winScreen != null)
        {
            winScreen.ShowWinScreen();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
