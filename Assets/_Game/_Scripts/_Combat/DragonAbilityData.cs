using System;
using UnityEngine;

[Serializable]
public class DragonAbilityData
{
    [SerializeField] private float damage = 20f;
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private float range = 5f;

    //remembers when the ability can be used again.
    private float nextReadyTime;

    public float Damage => damage;
    public float Cooldown => cooldown;
    public float Range => range;

    public bool IsReady => Time.time >= nextReadyTime;

    public float CooldownRemaining =>
        Mathf.Max(0f, nextReadyTime - Time.time);

    public float CooldownNormalized
    {
        get
        {
            if (cooldown <= 0f)
                return 0f;

            return CooldownRemaining / cooldown;
        }
    }

    public bool TryUse()
    {
        if (!IsReady)
            return false;

        nextReadyTime = Time.time + cooldown;

        return true;
    }
}