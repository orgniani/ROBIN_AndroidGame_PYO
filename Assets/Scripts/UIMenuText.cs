using TMPro;
using UnityEngine;

public class UIMenuText : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private HealthController HP;

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
}
