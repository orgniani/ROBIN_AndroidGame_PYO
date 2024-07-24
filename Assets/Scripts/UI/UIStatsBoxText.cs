using TMPro;
using UnityEngine;

public class UIStatsBoxText : MonoBehaviour
{
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private Player HP;

    private void Awake()
    {
        ValidateReferences();
    }

    private void OnEnable()
    {
        HP.onHPChange += HandleUpdateHPText;
    }

    private void OnDisable()
    {
        HP.onHPChange -= HandleUpdateHPText;
    }

    private void HandleUpdateHPText()
    {
        HPText.text = "HP: " + HP.Health;
    }

    private void ValidateReferences()
    {
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
