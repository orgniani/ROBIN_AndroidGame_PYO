public interface ITextHandler
{
    public void UpdatePlayerTurnText(Player player);
    public void HandleActionText(Player attacker, ActionType actionType, Player target);
    public void HandleActionFailedText();
    public void HandleGameOverText(GameOverReason reason, Player player);
}
