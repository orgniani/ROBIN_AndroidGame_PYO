using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ActionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject rangeButton;

    [Space(10)]
    [SerializeField] private Transform border;
    [SerializeField] private Transform enemyBorder;

    [Header("Parameters")]
    [SerializeField] private float meleeDistance = 1f;
    [SerializeField] private float enemyTimeoutDuration = 3f;

    [Header("Logs")]
    [SerializeField] private bool enableLogs = true;

    private List<Player> allPlayers = new List<Player>();
    private Player currentPlayer;
    private Player target;

    private Coroutine currentActionCoroutine;

    private IconSetter iconSetter;

    private int round = 0;

    public bool HasChosenAction { private set; get; }

    private void Awake()
    {
        if (!gameManager)
        {
            Debug.LogError($"{name}: {nameof(gameManager)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!rangeButton)
        {
            Debug.LogError($"{name}: {nameof(rangeButton)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!border)
        {
            Debug.LogError($"{name}: {nameof(border)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!enemyBorder)
        {
            Debug.LogError($"{name}: {nameof(enemyBorder)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }
    }

    public void Initialize(List<Player> players)
    {
        allPlayers.Clear();
        allPlayers.AddRange(players);

        iconSetter = new IconSetter(allPlayers, meleeDistance);
    }

    public void SetCurrentPlayer(Player currentPlayer)
    {
        this.currentPlayer = currentPlayer;
        HasChosenAction = false;

        iconSetter.ResetIcons();

        border.gameObject.SetActive(!currentPlayer.IsEnemy);
        enemyBorder.gameObject.SetActive(currentPlayer.IsEnemy);

        if (currentPlayer.IsEnemy)
        {
            enemyBorder.position = currentPlayer.GetStatBoxPosition();
            StartCoroutine(EnemyAction());
            return;
        }

        border.position = currentPlayer.GetStatBoxPosition();
        rangeButton.SetActive(currentPlayer.GetCanRangeAttack());
    }


    public void OnRangeAttack()
    {
        iconSetter.SetIconsInRange(currentPlayer);

        if (currentActionCoroutine != null)
            StopCoroutine(currentActionCoroutine);

        currentActionCoroutine = StartCoroutine(PlayerAction(currentPlayer.RangeAttack));
    }

    public void OnMeleeAttack()
    {
        iconSetter.SetIconsInMeleeRange(currentPlayer);

        if (currentActionCoroutine != null)
            StopCoroutine(currentActionCoroutine);

        currentActionCoroutine = StartCoroutine(PlayerAction(currentPlayer.MeleeAttack));
    }

    public void OnHeal()
    {
        iconSetter.SetIconsInHealRange(currentPlayer);

        if (currentActionCoroutine != null)
            StopCoroutine(currentActionCoroutine);

        currentActionCoroutine = StartCoroutine(PlayerAction(currentPlayer.Heal));
    }

    public void OnChooseTarget(Player target)
    {
        this.target = target;
    }

    private IEnumerator PlayerAction(Action<Player> action)
    {
        target = null;
        gameManager.IsWaitingForMovement = false;

        while (target == null)
        {
            yield return new WaitForEndOfFrame();
        }

        action(target);
        HasChosenAction = true;
        currentActionCoroutine = null;
    }

    private IEnumerator EnemyAction()
    {
        float startTime = Time.time;

        yield return new WaitForSeconds(1);

        gameManager.IsWaitingForMovement = false;

        yield return new WaitForSeconds(1);

        OnMeleeAttack();

        yield return WaitUntilTargetSelectedOrTimeout(enemyTimeoutDuration, startTime);
        yield return new WaitForSeconds(1);
    }

    private IEnumerator WaitUntilTargetSelectedOrTimeout(float duration, float startTime)
    {
        while (target == null && Time.time - startTime < duration)
        {
            ChooseRandomTarget();
            yield return new WaitForEndOfFrame();
        }

        if (target == null)
        {
            if(enableLogs)
                Debug.Log("Target selection timed out.");

            HasChosenAction = true;
            currentActionCoroutine = null;
        }
    }

    private void ChooseRandomTarget()
    {
        Player closestTarget = FindClosestTarget(meleeDistance);

        if (closestTarget == null)
        {
            OnRangeAttack();
            closestTarget = FindClosestTarget(currentPlayer.GetMaxAttackRange());
        }

        if (closestTarget != null)
        {
            round++;

            if (enableLogs)
                Debug.Log(round + " - " + currentPlayer.name + " has Chosen Target: " + closestTarget.name);
        }

        target = closestTarget;
    }

    private Player FindClosestTarget(float maxRange)
    {
        Player closestTarget = null;
        float minDistance = float.MaxValue;
        List<Player> closestTargets = new List<Player>();

        foreach (Player targetPlayer in allPlayers)
        {
            if (!IsValidTarget(targetPlayer))
                continue;

            int distance = CalculateDistance(currentPlayer.GridPosition, targetPlayer.GridPosition);

            if (distance <= maxRange)
            {
                UpdateClosestTargets(distance, targetPlayer, ref minDistance, ref closestTargets);
            }
        }

        if (closestTargets.Count > 0)
        {
            closestTarget = closestTargets[Random.Range(0, closestTargets.Count)];
        }

        return closestTarget;
    }

    private int CalculateDistance(Vector2Int position1, Vector2Int position2)
    {
        int dx = Mathf.Abs(position1.x - position2.x);
        int dy = Mathf.Abs(position1.y - position2.y);

        return Mathf.Max(dx, dy);
    }

    private bool IsValidTarget(Player player)
    {
        return !player.IsEnemy;
    }

    private void UpdateClosestTargets(float distance, Player targetPlayer, ref float minDistance, ref List<Player> closestTargets)
    {
        if (distance < minDistance)
        {
            minDistance = distance;
            closestTargets.Clear();
            closestTargets.Add(targetPlayer);
        }

        else if (distance == minDistance)
        {
            closestTargets.Add(targetPlayer);
        }
    }
}
