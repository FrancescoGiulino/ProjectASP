using UnityEngine;
using System;

public class HealthController : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] bool hasHealthbar = false;
    [SerializeField] private HealthBarController healthbarController;
    private float currentHealth;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;

    public event Action OnDeathAction;

    void Awake()
    {
        currentHealth = maxHealth;
        if (hasHealthbar && !healthbarController)
            healthbarController.GetComponent<HealthBarController>();
    }

    public void TakeDamage(float amount)
    {
        if (IsDead) return;
        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
        
        if (hasHealthbar && healthbarController)
            healthbarController?.SetValue(currentHealth/maxHealth);
    }

    public void Heal(float amount)
    {
        if (IsDead) return;
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        
        if (hasHealthbar && healthbarController)
            healthbarController?.SetValue(currentHealth / maxHealth);
    }

    protected virtual void Die() {
        OnDeathAction?.Invoke();
    }
}