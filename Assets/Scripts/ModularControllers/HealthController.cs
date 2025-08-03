using UnityEngine;
using System;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;

    public event Action OnDeathAction;

    void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        currentHealth -= amount;
        //Debug.Log($"Damage taken: {amount}. Current health: {currentHealth}");
        if (currentHealth <= 0f) Die();
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    protected virtual void Die() {
        OnDeathAction?.Invoke();
    }
}