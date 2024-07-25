using UnityEngine;
using UnityEngine.UI;

public class UIButtonHandler : IButtonHandler
{
    private readonly GameManager gameManager;
    private readonly MovementController movementController;
    private readonly LevelManager levelManager;

    private readonly Slider loadBar;

    public UIButtonHandler(GameManager gameManager, MovementController movementController, LevelManager levelManager, Slider loadBar)
    {
        this.gameManager = gameManager;
        this.movementController = movementController;

        this.levelManager = levelManager;
        this.loadBar = loadBar;
    }

    public void OnMoveUp()
    {
        if (!gameManager.IsWaitingForMovement || gameManager.GameOver) return;
        movementController.MoveCharacterUp();
    }

    public void OnMoveDown()
    {
        if (!gameManager.IsWaitingForMovement || gameManager.GameOver) return;
        movementController.MoveCharacterDown();
    }

    public void OnMoveLeft()
    {
        if (!gameManager.IsWaitingForMovement || gameManager.GameOver) return;
        movementController.MoveCharacterLeft();
    }

    public void OnMoveRight()
    {
        if (!gameManager.IsWaitingForMovement || gameManager.GameOver) return;
        movementController.MoveCharacterRight();
    }

    public void OnBackToMainMenu(GameObject loadingCanvas)
    {
        loadBar.value = 0;
        OnOpenAndCloseCanvas(loadingCanvas);
        levelManager.RestartGame();
    }

    public void OnOpenAndCloseCanvas(GameObject canvas)
    {
        if (canvas.activeSelf)
        {
            canvas.SetActive(false);
        }
        else
        {
            canvas.SetActive(true);
        }
    }
}