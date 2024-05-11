using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ActionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private GameObject rangeButton;

    [Space(10)]
    [SerializeField] private Transform border;
    [SerializeField] private Transform enemyBorder;

    [Header("Buttons For Enemies Random Choice")]
    [SerializeField] private List<Button> enemyTargetButtons;
    [SerializeField] private List<Button> enemyActionButtons;

    [Header("Parameters")]
    [SerializeField] private float meleeDistance = 1;

    [Header("Logs")]
    [SerializeField] private bool enableLogs = true;

    private Player currentPlayer;
    private List<Player> allPlayers = new List<Player>();

    private Button actionButton;
    private Coroutine currentActionCoroutine;

    private Player target;

    private IconSetter iconSetter;

    private int round = 0;

    public bool HasChosenAction { private set; get; }

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

        currentActionCoroutine = StartCoroutine(ActionSequence(currentPlayer.RangeAttack));
    }

    public void OnMeleeAttack()
    {
        iconSetter.SetIconsInMeleeRange(currentPlayer);

        if (currentActionCoroutine != null)
            StopCoroutine(currentActionCoroutine);

        currentActionCoroutine = StartCoroutine(ActionSequence(currentPlayer.MeleeAttack));
    }

    public void OnHeal()
    {
        iconSetter.SetIconsInHealRange(currentPlayer);

        if (currentActionCoroutine != null)
            StopCoroutine(currentActionCoroutine);

        currentActionCoroutine = StartCoroutine(ActionSequence(currentPlayer.Heal));
    }

    public void OnChooseTarget(Player target)
    {
        this.target = target;
    }

    private IEnumerator ActionSequence(Action<Player> action)
    {
        target = null;
        turnManager.IsWaitingForMovement = false;

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
        float timeoutDuration = 3f;
        float startTime = Time.time;

        yield return new WaitForSeconds(1);

        turnManager.IsWaitingForMovement = false;

        yield return new WaitForSeconds(1);

        ChooseRandomAction();

        yield return WaitUntilTargetSelectedOrTimeout(timeoutDuration, startTime);
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

    private void ChooseRandomAction()
    {
        actionButton = null;

        int randomIndex = Random.Range(0, enemyActionButtons.Count);
        actionButton = enemyActionButtons[randomIndex];

        actionButton.onClick.Invoke();
    }

    private void ChooseRandomTarget()
    {
        int randomIndex = Random.Range(0, enemyTargetButtons.Count);
        Button randomButton = enemyTargetButtons[randomIndex];

        if (randomButton.gameObject.activeSelf)
        {
            round++;

            if (enableLogs)
            {
                Debug.Log(round + " - " + currentPlayer.name + " has Chosen Action Button: " + actionButton.name);
                Debug.Log(round + " - " + currentPlayer.name + " has Chosen Target Button: " + randomButton.name);
            }

            randomButton.onClick.Invoke();
            return;
        }

        ChooseRandomAction();
    }
}
