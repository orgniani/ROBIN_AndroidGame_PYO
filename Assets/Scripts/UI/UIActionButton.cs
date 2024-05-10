using System.Collections.Generic;
using UnityEngine;

public class UIActionButton : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private ActionMenuController menuController;

    [SerializeField] private float meleeDistance = 1;

    [SerializeField] private List<Player> players;

    public void OnRangeAttack()
    {
        SetIconsInRange(player.GetMaxAttackRange());
    }

    public void OnMeleeAttack()
    {
        SetIconsInMeleeRange();

    }

    public void OnHeal()
    {
        SetIconsInHealRange();
    }

    private void SetIconsInRange(float maxRange)
    {
        foreach (Player targetPlayer in players)
        {
            bool inRange = IsInRange(player.transform.position, targetPlayer.transform.position, maxRange);
            targetPlayer.SetIconActive(inRange);
        }

        player.SetIconActive(false);
    }

    private void SetIconsInMeleeRange()
    {
        foreach (Player targetPlayer in players)
        {
            bool inMeleeRange = IsClose(player.transform.position, targetPlayer.transform.position, meleeDistance);
            targetPlayer.SetIconActive(inMeleeRange);
        }

        player.SetIconActive(false);
    }

    private void SetIconsInHealRange()
    {
        foreach (Player targetPlayer in players)
        {
            bool inHealRange = IsClose(player.transform.position, targetPlayer.transform.position, player.GetMaxHealRange()) && !player.GetCanOnlyHealSelf();
            targetPlayer.SetIconActive(inHealRange);
        }

        player.SetIconActive(true);
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
