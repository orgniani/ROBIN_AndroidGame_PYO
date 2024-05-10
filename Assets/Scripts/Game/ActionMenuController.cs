using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ActionMenuController : MonoBehaviour
{
    private Player currentPlayer;
    private List<Player> allPlayers = new List<Player>();

    [SerializeField] private TurnManager turnManager;

    [SerializeField] private GameObject rangeButton;

    [SerializeField] private Transform border;
    [SerializeField] private Transform enemyBorder;

    [SerializeField] private List<Button> targetButtons;
    [SerializeField] private List<Button> actionButtons;

    [SerializeField] private float meleeDistance = 1;

    private Button actionButton;

    public bool chooseAction = false;

    private Coroutine currentActionCoroutine;

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

        if(turnManager.isEnemyTurn)
        {
            border.gameObject.SetActive(false);
            enemyBorder.gameObject.SetActive(true);
            enemyBorder.position = currentPlayer.GetStatBoxPosition();

            StartCoroutine(EnemyAction());
        }

        else
        {
            border.gameObject.SetActive(true);
            enemyBorder.gameObject.SetActive(false);
            border.position = currentPlayer.GetStatBoxPosition();
        }
    }

    private void Update()
    {
        if (currentPlayer == null) return;

        rangeButton.SetActive(currentPlayer.CanRangeAttack());
    }

    public void OnRangeAttack()
    {
        SetIconsInRange(currentPlayer.GetMaxAttackRange());

        if (currentActionCoroutine != null)
            StopCoroutine(currentActionCoroutine);

        currentActionCoroutine = StartCoroutine(ActionSequence(currentPlayer.RangeAttack));
    }

    public void OnMeleeAttack()
    {
        SetIconsInMeleeRange();

        if (currentActionCoroutine != null)
            StopCoroutine(currentActionCoroutine);

        currentActionCoroutine = StartCoroutine(ActionSequence(currentPlayer.MeleeAttack));
    }

    public void OnHeal()
    {
        SetIconsInHealRange();

        if (currentActionCoroutine != null)
            StopCoroutine(currentActionCoroutine);

        currentActionCoroutine = StartCoroutine(ActionSequence(currentPlayer.Heal));
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

        currentActionCoroutine = null;
    }

    private IEnumerator EnemyAction()
    {
        while (turnManager.waitingForMovement)
        {
            yield return new WaitForSeconds(1);
        }

        turnManager.waitingForMovement = false;

        yield return new WaitForSeconds(1);

        ChooseRandomAction();

        while (targetHP == null)
        {
            ChooseRandomTarget();
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);
    }

    public void OnChooseTarget(HealthController targetHP)
    {

        this.targetHP = targetHP;
    }

    private void ChooseRandomAction()
    {
        actionButton = null;

        int randomIndex = Random.Range(0, actionButtons.Count);
        actionButton = actionButtons[randomIndex];

        actionButton.onClick.Invoke();
    }

    private void ChooseRandomTarget()
    {
        int randomIndex = Random.Range(0, targetButtons.Count);
        Button randomButton = targetButtons[randomIndex];

        Player targetPlayer = allPlayers[randomIndex];
        bool isTargetInRange;

        if (actionButton.name == rangeButton.name)
        {
            isTargetInRange = IsInRange(currentPlayer.transform.position, targetPlayer.transform.position, currentPlayer.GetMaxAttackRange());
        }

        else
        {
            isTargetInRange = IsClose(currentPlayer.transform.position, targetPlayer.transform.position, meleeDistance);
        }

        if (isTargetInRange)
        {
            Debug.Log(currentPlayer.name + " has Chosen Action Button: " + actionButton.name);
            Debug.Log(currentPlayer.name + " has Chosen Target Button: " + randomButton.name);
            randomButton.onClick.Invoke();
        }

        else
        {
            ChooseRandomAction();
        }
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
        SetIconsInRangeOrMelee(maxRange, currentPlayer.GetMaxAttackRange());
        currentPlayer.SetIconActive(false);
    }

    private void SetIconsInMeleeRange()
    {
        SetIconsInRangeOrMelee(0f, meleeDistance);
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

    private void SetIconsInRangeOrMelee(float maxRange, float maxDistance)
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
