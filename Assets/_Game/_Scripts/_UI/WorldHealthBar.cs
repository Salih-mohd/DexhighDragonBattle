using UnityEngine;
using UnityEngine.UI;

public class WorldHealthBar : MonoBehaviour
{
    [SerializeField] private DragonHealth dragonHealth;
    [SerializeField] private Slider healthSlider;

    private void OnEnable()
    {
        dragonHealth.OnHealthChanged += UpdateHealth;
    }

    private void OnDisable()
    {
        dragonHealth.OnHealthChanged -= UpdateHealth;
    }

    private void Start()
    {
        UpdateHealth(
            dragonHealth.CurrentHealth,
            dragonHealth.MaxHealth
        );
    }

    private void UpdateHealth(float currentHealth, float maxHealth)
    {
        if (maxHealth <= 0f)
            return;

        healthSlider.value =
            currentHealth / maxHealth;
    }
}