using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    [Header("Referencje UI")]
    [SerializeField] private Slider energySlider;
    [SerializeField] private Image fillImage;

    [Header("Kolory paska")]
    [SerializeField] private Color colorFull   = new Color(0.2f, 0.5f, 1.0f);  // niebieski
    [SerializeField] private Color colorMedium = new Color(0.9f, 0.7f, 0.1f);  // żółty
    [SerializeField] private Color colorLow    = new Color(0.9f, 0.2f, 0.2f);  // czerwony

    [Header("Progi kolorów")]
    [SerializeField] private float mediumThreshold = 0.5f;
    [SerializeField] private float lowThreshold    = 0.25f;

    [SerializeField] private PlayerEnergy playerEnergy;

    private void Awake()
    {
        if (playerEnergy == null)
            playerEnergy = FindAnyObjectByType<PlayerEnergy>();

        if (playerEnergy != null)
            playerEnergy.OnEnergyChanged.AddListener(UpdateBar);
    }

    private void Start()
    {
        if (playerEnergy != null)
            UpdateBar(playerEnergy.CurrentEnergy, playerEnergy.MaxEnergy);
    }

    private void OnDestroy()
    {
        if (playerEnergy != null)
            playerEnergy.OnEnergyChanged.RemoveListener(UpdateBar);
    }

    private void UpdateBar(float current, float max)
    {
        float percent = current / max;

        if (energySlider != null)
            energySlider.value = percent;

        if (fillImage != null)
            fillImage.color = GetEnergyColor(percent);
    }

    private Color GetEnergyColor(float percent)
    {
        if (percent > mediumThreshold)
            return Color.Lerp(colorMedium, colorFull,
                (percent - mediumThreshold) / (1f - mediumThreshold));
        else if (percent > lowThreshold)
            return Color.Lerp(colorLow, colorMedium,
                (percent - lowThreshold) / (mediumThreshold - lowThreshold));
        else
            return colorLow;
    }
}