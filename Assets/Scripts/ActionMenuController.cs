using System;
using System.Collections;
using UnityEngine;

public class ActionMenuController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TurnManager turnManager;

    [SerializeField] private GameObject rangeButton;

    public bool chooseAction = false;

    private HealthController targetHP;

    private void Update()
    {
        if(!player.CanRangeAttack())
        {
            rangeButton.SetActive(false);
            return;
        }

        rangeButton.SetActive(true);
    }

    public void OnRangeAttack()
    {
        turnManager.waitingForMovement = false;

        StopAllCoroutines();
        StartCoroutine(ActionSequence(player.RangeAttack));
    }
    public void OnMeleeAttack()
    {
        turnManager.waitingForMovement = false;

        StopAllCoroutines();
        StartCoroutine(ActionSequence(player.MeleeAttack));
    }

    public void OnHeal()
    {
        turnManager.waitingForMovement = false;

        StopAllCoroutines();
        StartCoroutine(ActionSequence(player.Heal));
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
    }


    public void OnChooseTarget(HealthController targetHP)
    {
        if (turnManager.waitingForMovement) return;

        this.targetHP = targetHP;
    }
}
