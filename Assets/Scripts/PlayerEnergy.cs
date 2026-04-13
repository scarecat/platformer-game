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

    private bool canBlock = true;
    public bool CanBlock => canBlock;

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

        if (!canBlock && currentEnergy >= maxEnergy * 0.25f)
            canBlock = true;
    }

    public bool UseBlockEnergy(float deltaTime)
    {
        if (currentEnergy <= 0f)
        {
            canBlock = false;
            OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
            return false;
        }

        currentEnergy = Mathf.Clamp(currentEnergy - blockCostPerSecond * deltaTime, 0f, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
        return true;
    }

    public void ConsumeEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy - amount, 0f, maxEnergy);
        if (currentEnergy <= 0f)
            canBlock = false;
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }

    public void RestoreEnergy(float amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0f, maxEnergy);
        OnEnergyChanged?.Invoke(currentEnergy, maxEnergy);
    }
}