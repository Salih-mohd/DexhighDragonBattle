using System;
using UnityEngine;

public class DragonHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    [SerializeField] private Collider hurtbox;

    public Collider Hurtbox => hurtbox;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => maxHealth;
    public bool IsDead { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action OnDied;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead)
            return;

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        Debug.Log($"{name} Health: {CurrentHealth}");

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        OnDied?.Invoke();

        Debug.Log($"{name} died.");
    }
}