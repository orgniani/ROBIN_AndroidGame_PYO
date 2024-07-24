using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private string titleTag = "Fighter";
    [SerializeField] private int maxSpeed;
    [SerializeField] private int health = 1;

    [Space(10)]
    [SerializeField] private int rangeDamage;
    [SerializeField] private int meleeDamage;
    [SerializeField] private int cureHP;

    [Header("Max Distances")]
    [SerializeField] private float maxRangeAttackDistance = 1.55f;
    [SerializeField] private float maxRangeHealDistance = 1.3f;

    [Header("Behaviors")]
    [SerializeField] private bool canOnlyHealSelf = false;
    [SerializeField] private bool canRangeAttack = false;
    [SerializeField] private bool isEnemy = false;

    [Header("References")]
    [SerializeField] private GameObject icon;
    [SerializeField] private Transform statBox;

    public event Action onHPChange = delegate { };
    public event Action onDead = delegate { };

    public bool IsEnemy => isEnemy;

    public string TitleTag => titleTag;

    public int Health => health;

    public Vector2Int GridPosition { get; set; }

    private void Awake()
    {
        ValidateReferences();
    }

    public int GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetMaxAttackRange()
    {
        return maxRangeAttackDistance;
    }

    public float GetMaxHealRange()
    {
        return maxRangeHealDistance;
    }

    public bool GetCanOnlyHealSelf()
    {
        return canOnlyHealSelf;
    }

    public bool GetCanRangeAttack()
    {
        return canRangeAttack;
    }

    public Vector2 GetStatBoxPosition()
    {
        return statBox.position;
    }

    public void SetIconActive(bool active)
    {
        if (!icon) return;
        icon.SetActive(active);
    }

    public void MeleeAttack(Player targetHP)
    {
        targetHP.ReceiveDamage(meleeDamage);
    }

    public void RangeAttack(Player targetHP)
    {
        targetHP.ReceiveDamage(rangeDamage);
    }

    public void Heal(Player targetHP)
    {
        targetHP.CureHP(cureHP);
    }

    private void ReceiveDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            onHPChange?.Invoke();

            Die();

            return;
        }

        onHPChange?.Invoke();
    }

    private void CureHP(int addedHP)
    {
        health += addedHP;
        onHPChange?.Invoke();
    }

    private void Die()
    {
        onDead?.Invoke();
        health = -1;

        Destroy(icon);

        gameObject.SetActive(false);
    }

    private void ValidateReferences()
    {
        if (!icon)
        {
            Debug.LogError($"{name}: {nameof(icon)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!statBox)
        {
            Debug.LogError($"{name}: {nameof(statBox)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }
    }
}
