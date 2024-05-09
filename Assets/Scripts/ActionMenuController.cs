using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMenuController : MonoBehaviour
{
    private Player currentPlayer;
    private List<Player> allPlayers = new List<Player>();

    [SerializeField] private TurnManager turnManager;

    [SerializeField] private GameObject rangeButton;
    [SerializeField] private Transform border;

    [SerializeField] private float meleeDistance = 1;

    public bool chooseAction = false;

    private HealthController targetHP;

    public void SetPlayers(List<Player> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            allPlayers.Clear();
            allPlayers.AddRange(players);
        }
    }

    public void SetCurrentPlayer(Player currentPlayer)
    {
        this.currentPlayer = currentPlayer;

        ResetIcons();
        border.position = currentPlayer.GetStatBoxPosition();
    }

    private void Update()
    {
        if (currentPlayer == null) return;

        if(!currentPlayer.CanRangeAttack())
        {
            rangeButton.SetActive(false);
            return;
        }

        rangeButton.SetActive(true);
    }

    public void OnRangeAttack()
    {
        SetIconsInRange(currentPlayer.GetMaxAttackRange());

        StopAllCoroutines();
        StartCoroutine(ActionSequence(currentPlayer.RangeAttack));
    }

    public void OnMeleeAttack()
    {
        SetIconsInMeleeRange();

        StopAllCoroutines();
        StartCoroutine(ActionSequence(currentPlayer.MeleeAttack));
    }

    public void OnHeal()
    {
        SetIconsInHealRange();

        StopAllCoroutines();
        StartCoroutine(ActionSequence(currentPlayer.Heal));
    }


    private IEnumerator ActionSequence(Action<HealthController> action)
    {
        targetHP = null;

        while (targetHP == null)
        {
            yield return new WaitForEndOfFrame();
        }

        action(targetHP);
        chooseAction = true;
        turnManager.waitingForMovement = false;
    }

    public void OnChooseTarget(HealthController targetHP)
    {
        //if (turnManager.waitingForMovement) return;

        this.targetHP = targetHP;
    }

    private void ResetIcons()
    {
        foreach (Player targetPlayer in allPlayers)
        {
            targetPlayer.SetIconActive(false);
        }
    }

    private void SetIconsInRange(float maxRange)
    {
        foreach (Player targetPlayer in allPlayers)
        {
            bool inRange = IsInRange(currentPlayer.transform.position, targetPlayer.transform.position, maxRange);
            targetPlayer.SetIconActive(inRange);
        }

        currentPlayer.SetIconActive(false);
    }

    private void SetIconsInMeleeRange()
    {
        foreach (Player targetPlayer in allPlayers)
        {
            bool inMeleeRange = IsClose(currentPlayer.transform.position, targetPlayer.transform.position, meleeDistance);
            targetPlayer.SetIconActive(inMeleeRange);
        }

        currentPlayer.SetIconActive(false);
    }

    private void SetIconsInHealRange()
    {
        foreach (Player targetPlayer in allPlayers)
        {
            bool inHealRange = IsClose(currentPlayer.transform.position, targetPlayer.transform.position, currentPlayer.GetMaxHealRange()) && !currentPlayer.GetCanOnlyHealSelf();
            targetPlayer.SetIconActive(inHealRange);
        }

        currentPlayer.SetIconActive(true);
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
