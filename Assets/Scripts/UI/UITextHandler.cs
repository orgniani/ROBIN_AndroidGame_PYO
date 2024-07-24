using TMPro;
using UnityEngine;

public class UITextHandler : ITextHandler
{
    private TMP_Text gameOverText;
    private TMP_Text playerTurnText;
    private TMP_Text playerActionText;

    public UITextHandler(TMP_Text gameOverText, TMP_Text playerTurnText, TMP_Text playerActionText)
    {
        this.gameOverText = gameOverText;
        this.playerTurnText = playerTurnText;
        this.playerActionText = playerActionText;
    }

    public void HandleGameOverText(GameOverReason reason, Player player)
    {
        switch (reason)
        {
            case GameOverReason.WIN:
                gameOverText.text = $"The {player.TitleTag} has won!";
                break;

            case GameOverReason.LOSE:
                gameOverText.text = $"One of your players died before the enemies!";
                break;
        }
    }

    public void UpdatePlayerTurnText(Player player)
    {
        playerTurnText.text = $"{player.TitleTag}'s turn";
        playerActionText.text = $"";
    }

    public void HandleActionText(Player attacker, ActionType actionType, Player target)
    {
        string actionDescription = actionType.ToString().Replace("_", " ").ToLower();
        string targetDescription = (attacker.TitleTag == target.TitleTag) ? "self" : target.TitleTag;

        playerActionText.text = $"{attacker.TitleTag} used {actionDescription} on {targetDescription}.";
    }

    public void HandleActionFailedText()
    {
        playerActionText.text = "Targets out of range! No action.";
    }
}
