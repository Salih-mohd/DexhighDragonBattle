using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownUI : MonoBehaviour
{
    [SerializeField] private DragonCombat playerCombat;

    [Header("Cooldown Sliders")]
    [SerializeField] private Slider fireSlider;
    [SerializeField] private Slider tailSlider;
    [SerializeField] private Slider flySlider;

    private void Update()
    {
        UpdateSlider(
            fireSlider,
            playerCombat.FireAttack
        );

        UpdateSlider(
            tailSlider,
            playerCombat.TailAttack
        );

        UpdateSlider(
            flySlider,
            playerCombat.FlyAttack
        );
    }

    private void UpdateSlider(
        Slider slider,
        DragonAbilityData ability)
    {
        if (ability.IsReady)
        {
            slider.value = 1f;
            return;
        }

        slider.value =
            1f - ability.CooldownNormalized;
    }
}