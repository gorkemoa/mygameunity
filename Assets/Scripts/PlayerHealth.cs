using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 10f;

    public float CurrentHealth { get; private set; }

    // current, max
    public event Action<float, float> OnHealthChanged;

    void Awake()
    {
        CurrentHealth = maxHealth;
        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
    }

    public void TakeDamage(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0f, maxHealth);
        Debug.Log($"[PlayerHealth] Damage: {amount} | Current: {CurrentHealth}");

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("[PlayerHealth] Player öldü.");
        // Şimdilik sadece log, ileride respawn vs. ekleriz
    }
}
