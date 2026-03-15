using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Referencje UI")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image fillImage;

    [Header("Kolory paska")]
    [SerializeField] private Color colorFull   = new Color(0.2f, 0.8f, 0.2f);
    [SerializeField] private Color colorMedium = new Color(0.9f, 0.7f, 0.1f);
    [SerializeField] private Color colorLow    = new Color(0.9f, 0.2f, 0.2f);

    [Header("Progi kolorów")]
    [SerializeField] private float mediumThreshold = 0.5f;
    [SerializeField] private float lowThreshold    = 0.25f;

    [SerializeField] private PlayerHealth playerHealth;

    private void Awake()
    {
        if (playerHealth == null)
            playerHealth = FindAnyObjectByType<PlayerHealth>();

        if (playerHealth != null)
            playerHealth.OnHealthChanged.AddListener(UpdateBar);
    }

    private void Start()
    {
        if (playerHealth != null)
            UpdateBar(playerHealth.CurrentHealth, playerHealth.MaxHealth);
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnHealthChanged.RemoveListener(UpdateBar);
    }

    private void UpdateBar(float current, float max)
    {
        float percent = current / max;

        if (healthSlider != null)
            healthSlider.value = percent;

        if (fillImage != null)
            fillImage.color = GetHealthColor(percent);
    }

    private Color GetHealthColor(float percent)
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