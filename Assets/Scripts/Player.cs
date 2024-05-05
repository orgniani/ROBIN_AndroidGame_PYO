using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxSpeed;
    [SerializeField] private int HP;

    [SerializeField] private int rangeDamage;
    [SerializeField] private int meleeDamage;
    [SerializeField] private int cureHP;

    private int playerNumber;

    public Player(int playerNumber)
    {
        this.playerNumber = playerNumber;
    }

    private void RecieveDamage(int damage)
    {
        HP -= damage;
    }

    private void CureHP(int addedHP)
    {
        HP += addedHP;
    }

    public void Attack(Player target, int attackType)
    {
        if(attackType == 1) //RANGE
        {
            target.RecieveDamage(rangeDamage);
        }

        if (attackType == 2) //MELEE
        {
            target.RecieveDamage(meleeDamage);
        }
    }

    public void Heal(Player target)
    {
        target.CureHP(cureHP);
    }

    public void PlayTurn(GameController gameController)
    {
        gameController.UpdateCharacterPosition(playerNumber);
        gameController.StoreCharacterPosition(playerNumber);
    }
}
