using System.Collections.Generic;
using UnityEngine;

public class IconSetter
{
    private List<Player> allPlayers = new List<Player>();
    private float meleeDistance;

    public int Speed { private set; get; }

    public IconSetter(List<Player> allPlayers, float meleeDistance)
    {
        this.allPlayers = allPlayers;
        this.meleeDistance = meleeDistance;
    }

    public void ResetIcons()
    {
        foreach (Player targetPlayer in allPlayers)
        {
            targetPlayer.SetIconActive(false);
        }
    }

    public void SetIconsInRange(Player currentPlayer)
    {
        float maxRange = currentPlayer.GetMaxAttackRange();

        SetIconsInRangeOrMelee(currentPlayer, maxRange, maxRange);
        currentPlayer.SetIconActive(false);
    }

    public void SetIconsInMeleeRange(Player currentPlayer)
    {
        SetIconsInRangeOrMelee(currentPlayer, 0f, meleeDistance);
        currentPlayer.SetIconActive(false);
    }

    public void SetIconsInHealRange(Player currentPlayer)
    {
        foreach (Player targetPlayer in allPlayers)
        {
            bool inHealRange = IsClose(currentPlayer.transform.position, targetPlayer.transform.position, currentPlayer.GetMaxHealRange()) && !currentPlayer.GetCanOnlyHealSelf();
            targetPlayer.SetIconActive(inHealRange);

            if (targetPlayer.IsEnemy)
            {
                targetPlayer.SetIconActive(false);
            }
        }

        currentPlayer.SetIconActive(true);
    }

    private void SetIconsInRangeOrMelee(Player currentPlayer, float maxRange, float maxDistance)
    {
        foreach (Player targetPlayer in allPlayers)
        {
            bool inRange = IsInRange(currentPlayer.transform.position, targetPlayer.transform.position, maxRange);
            bool inMeleeRange = IsClose(currentPlayer.transform.position, targetPlayer.transform.position, maxDistance);

            targetPlayer.SetIconActive(maxRange > 0f ? inRange : inMeleeRange);
        }
    }

    private bool IsClose(Vector2 position1, Vector2 position2, float maxDistance)
    {
        float distance = Vector2.Distance(position1, position2);
        return distance <= maxDistance;
    }

    private bool IsInRange(Vector2 position1, Vector2 position2, float maxRange)
    {
        float distance = Vector2.Distance(position1, position2);
        return distance > meleeDistance && distance <= maxRange;
    }
}
