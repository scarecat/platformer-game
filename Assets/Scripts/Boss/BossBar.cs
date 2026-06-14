using UnityEngine;
using UnityEngine.UI;

public class BossBar : MonoBehaviour
{
    [SerializeField] private EntityHealth trackedHealth = null;
    private GameObject bossBarSliderObj;
    private Slider bossBarSlider;
    
    public EntityHealth TrackedHealth
    {
        get => trackedHealth;
        set
        {
            if (trackedHealth != null)
            {
                trackedHealth.OnHealthChanged.RemoveListener(OnTrackedHealthChanged);
                trackedHealth.OnDeath.RemoveListener(OnTrackedDeath);
            }

            trackedHealth = value;

            bossBarSliderObj.SetActive(trackedHealth != null);
            
            bossBarSlider.maxValue = trackedHealth.MaxHealth;
            bossBarSlider.value = trackedHealth.CurrentHealth;

            if (trackedHealth != null)
            {
                trackedHealth.OnHealthChanged.AddListener(OnTrackedHealthChanged);
                trackedHealth.OnDeath.AddListener(OnTrackedDeath);
            }
        }
    }


    protected void OnTrackedHealthChanged(float health, float maxHealth)
    {
        bossBarSlider.value = health;
        bossBarSlider.maxValue = maxHealth;
    }

    protected void OnTrackedDeath()
    {
        TrackedHealth = null;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bossBarSliderObj = transform.Find("BossBarSlider").gameObject;
        bossBarSlider = bossBarSliderObj.GetComponent<Slider>();
        TrackedHealth = trackedHealth;
    }

}
