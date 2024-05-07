using System.Collections;
using UnityEngine;

public class ActionMenuController : MonoBehaviour
{
    [SerializeField] private GameView gameView;
    [SerializeField] private GameObject buttonBlockerScreen;

    public bool chooseAction = false;

    private HealthController targetHP;

    public void OnRangeAttack(Player player)
    {
        if (gameView.waitingForMovement) return;

        buttonBlockerScreen.SetActive(true);
        StartCoroutine(RangeAttackSequence(player));
    }

    private IEnumerator RangeAttackSequence(Player player)
    {
        targetHP = null;

        while (targetHP == null)
        {
            yield return new WaitForEndOfFrame();
        }

        player.RangeAttack(targetHP);
        chooseAction = true;
    }

    public void OnMeleeAttack(Player player)
    {
        if (gameView.waitingForMovement) return;

        buttonBlockerScreen.SetActive(true);

        StartCoroutine(MeleeAttackSequence(player));
    }

    private IEnumerator MeleeAttackSequence(Player player)
    {
        targetHP = null;

        while (targetHP == null)
        {
            yield return new WaitForEndOfFrame();
        }

        player.MeleeAttack(targetHP);
        chooseAction = true;
    }

    public void OnHeal(Player player)
    {
        if (gameView.waitingForMovement) return;

        buttonBlockerScreen.SetActive(true);

        StartCoroutine(HealSequence(player));
    }

    private IEnumerator HealSequence(Player player)
    {
        targetHP = null;

        while (targetHP == null)
        {
            yield return new WaitForEndOfFrame();
        }

        player.Heal(targetHP);
        chooseAction = true;
    }

    public void OnChooseTarget(HealthController targetHP)
    {
        if (gameView.waitingForMovement) return;
        this.targetHP = targetHP;
        buttonBlockerScreen.SetActive(false);
    }
}
