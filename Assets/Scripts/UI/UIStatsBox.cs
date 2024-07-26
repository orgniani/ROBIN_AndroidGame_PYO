using TMPro;
using UnityEngine;

public class UIStatsBox : MonoBehaviour
{
    [SerializeField] private GameObject deadScreen;

    [SerializeField] private TMP_Text HPText;
    [SerializeField] private Player HP;

    private void Awake()
    {
        ValidateReferences();

        deadScreen.SetActive(false);
    }

    private void OnEnable()
    {
        HP.onHPChange += HandleUpdateHPText;
        HP.onDead += HandleDeadScreen;
    }

    private void OnDisable()
    {
        HP.onHPChange -= HandleUpdateHPText;
        HP.onDead -= HandleDeadScreen;
    }

    private void HandleUpdateHPText()
    {
        HPText.text = "HP: " + HP.Health;
    }

    private void HandleDeadScreen()
    {
        deadScreen.SetActive(true);
    }

    private void ValidateReferences()
    {
        if (!deadScreen)
        {
            Debug.LogError($"{name}: {nameof(deadScreen)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!HPText)
        {
            Debug.LogError($"{name}: {nameof(HPText)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!HP)
        {
            Debug.LogError($"{name}: {nameof(HP)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }
    }
}
