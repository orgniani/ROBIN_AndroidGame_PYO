using System.Collections.Generic;
using UnityEngine;

public class IconSetter
{
    private List<Player> allPlayers = new List<Player>();
    private float meleeDistance;

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
            bool inHealRange = IsClose(currentPlayer.GridPosition, targetPlayer.GridPosition, currentPlayer.GetMaxHealRange()) && !currentPlayer.GetCanOnlyHealSelf();
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
            bool inRange = IsInRange(currentPlayer.GridPosition, targetPlayer.GridPosition, maxRange);
            bool inMeleeRange = IsClose(currentPlayer.GridPosition, targetPlayer.GridPosition, maxDistance);

            targetPlayer.SetIconActive(maxRange > 0f ? inRange : inMeleeRange);
        }
    }

    private bool IsClose(Vector2Int position1, Vector2Int position2, float maxDistance)
    {
        int distance = CalculateDistance(position1, position2);
        return distance <= maxDistance;
    }

    private bool IsInRange(Vector2Int position1, Vector2Int position2, float maxRange)
    {
        int distance = CalculateDistance(position1, position2);
        return distance > meleeDistance && distance <= maxRange;
    }

    private int CalculateDistance(Vector2Int position1, Vector2Int position2)
    {
        int dx = Mathf.Abs(position1.x - position2.x);
        int dy = Mathf.Abs(position1.y - position2.y);

        return Mathf.Max(dx, dy);
    }
}
