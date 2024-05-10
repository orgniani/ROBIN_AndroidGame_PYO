using System;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int health = 1;

    public event Action onHPChange = delegate { };
    public event Action onDead = delegate { };

    public int Health => health;

    public void ReceiveDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Die();
        }

        onHPChange?.Invoke();
    }

    public void CureHP(int addedHP)
    {
        health += addedHP;
        onHPChange?.Invoke();
    }

    private void Die()
    {
        onDead?.Invoke();
        gameObject.SetActive(false);
    }
}
