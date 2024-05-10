using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxSpeed;

    [SerializeField] private int rangeDamage;
    [SerializeField] private int meleeDamage;
    [SerializeField] private int cureHP;

    [SerializeField] private float maxRangeAttackDistance = 1.55f;
    [SerializeField] private float maxRangeHealDistance = 1.3f;

    [SerializeField] private bool canOnlyHealSelf = false;
    [SerializeField] private bool canRangeAttack = false;
    [SerializeField] private bool isEnemy = false;

    [SerializeField] private GameObject icon;
    [SerializeField] private Transform statBox;
    [SerializeField] private HealthController HP;


    public bool IsEnemy => isEnemy;


    private void OnEnable()
    {
        HP.onDead += HandleDeath;
    }

    private void OnDisable()
    {
        HP.onDead -= HandleDeath;
    }

    public int GetMaxSpeed()
    {
        return maxSpeed;
    }

    public void MeleeAttack(HealthController targetHP)
    {
        targetHP.ReceiveDamage(meleeDamage);
    }

    public void RangeAttack(HealthController targetHP)
    {
        targetHP.ReceiveDamage(rangeDamage);
    }

    public float GetMaxAttackRange()
    {
        return maxRangeAttackDistance;
    }

    public float GetMaxHealRange()
    {
        return maxRangeHealDistance;
    }

    public void Heal(HealthController targetHP)
    {
        targetHP.CureHP(cureHP);
    }

    public HealthController GetHP()
    {
        return HP;
    }

    public bool GetCanOnlyHealSelf()
    {
        return canOnlyHealSelf;
    }

    public bool CanRangeAttack()
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

    private void HandleDeath()
    {
        Destroy(icon);
    }
}
