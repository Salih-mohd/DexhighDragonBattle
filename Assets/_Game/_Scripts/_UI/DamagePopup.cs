using System;
using TMPro;
using UnityEngine;

public class DamagePopup : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float moveSpeed = 1.5f;
    [SerializeField] private float lifetime = 0.8f;

    private float timer;
    private Color originalColor;

    private Action<DamagePopup> returnToPool;

    private void Awake()
    {
        originalColor = damageText.color;
    }

    public void Show(
        float damage,
        Action<DamagePopup> releaseCallback)
    {
        timer = 0f;

        returnToPool = releaseCallback;

        damageText.text =
            $"-{Mathf.RoundToInt(damage)}";

        damageText.color = originalColor;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        transform.position +=
            Vector3.up *
            moveSpeed *
            Time.deltaTime;

        float normalizedTime =
            Mathf.Clamp01(timer / lifetime);

        Color color = originalColor;

        color.a = 1f - normalizedTime;

        damageText.color = color;

        if (timer >= lifetime)
        {
            returnToPool?.Invoke(this);
        }
    }
}