using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject creditsCanvas;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void Start()
    {
        creditsCanvas.SetActive(false);
    }

    //Movement Buttons
    public void OnMoveUp()
    {
        if (!gameManager.IsWaitingForMovement || gameManager.GameOver) return;
        gameManager.MovementController.MoveCharacterUp();
    }

    public void OnMoveDown()
    {
        if (!gameManager.IsWaitingForMovement || gameManager.GameOver) return;
        gameManager.MovementController.MoveCharacterDown();
    }

    public void OnMoveLeft()
    {
        if (!gameManager.IsWaitingForMovement || gameManager.GameOver) return;
        gameManager.MovementController.MoveCharacterLeft();
    }

    public void OnMoveRight()
    {
        if (!gameManager.IsWaitingForMovement || gameManager.GameOver) return;
        gameManager.MovementController.MoveCharacterRight();
    }

    //Credits Button
    public void OnOpenAndCloseCredits()
    {
        OnOpenAndCloseCanvas(creditsCanvas);
    }

    private void OnOpenAndCloseCanvas(GameObject canvas)
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
