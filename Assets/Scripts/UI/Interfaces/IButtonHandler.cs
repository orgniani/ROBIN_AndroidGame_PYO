using UnityEngine;

public interface IButtonHandler
{
    public void OnMoveUp();
    public void OnMoveDown();
    public void OnMoveLeft();
    public void OnMoveRight();

    public void OnOpenAndCloseCanvas(GameObject canvas);
    public void OnBackToMainMenu(GameObject loadingCanvas);
}
