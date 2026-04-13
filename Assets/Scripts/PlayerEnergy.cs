using UnityEngine;
using UnityEngine.Events;

public class PlayerEnergy : MonoBehaviour
{
    [Header("Ustawienia energii")]
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float currentEnergy;

    [Header("Koszt blokowania")]
    [SerializeField] private float blockCostPerSecond = 25f;

    [Header("Regeneracja")]
    [SerializeField] private float regenPerSecond = 10f;

    public UnityEvent<float, float> OnEnergyChanged;

    public float CurrentEnergy => currentEnergy;
    public float MaxEnergy => maxEnergy;
    public float EnergyPercent => currentEnergy / maxEnergy;
    public bool CanBlock => currentEnergy >= maxEnergy * 0.25f;

    private void Awake()
    {
        currentEnergy = maxEnergy;
    }

    private void Update()
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Clamp(currentEnergy + regenPerSecond * Time.deltaTime, 0f, maxEnergy);
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        }
    }

    public void UseBlockEnergy()
    {
        currentEnergy = Mathf.Clamp(currentEnergy - blockCostPerSecond * Time.deltaTime, 0f, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    public void ConsumeEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy - amount, 0f, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    public void RestoreEnergy(float amount)
    {
        ConsumeEnergy(-amount);
    }
}