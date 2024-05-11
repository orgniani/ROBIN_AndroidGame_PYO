using TMPro;
using UnityEngine;

public class UIMenuText : MonoBehaviour
{
    [SerializeField] private TMP_Text HPText;
    [SerializeField] private Player HP;

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
