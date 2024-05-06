using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxSpeed;

    [SerializeField] private int rangeDamage;
    [SerializeField] private int meleeDamage;
    [SerializeField] private int cureHP;

    private int playerNumber;

    public Player(int playerNumber)
    {
        this.playerNumber = playerNumber;
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

    public void Heal(HealthController targetHP)
    {
        targetHP.CureHP(cureHP);
    }

    public void PlayTurn(GameController gameController)
    {
        gameController.UpdateCharacterPosition(playerNumber);
        gameController.StoreCharacterPosition(playerNumber);
    }
}
