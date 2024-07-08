using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Slider loadBar;

    [Header("Objects")]
    [SerializeField] private GameObject mainMenuCanvas;

    [SerializeField] private GameObject creditsCanvas;
    [SerializeField] private GameObject howToPlayCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject warningCanvas;

    [SerializeField] private GameObject loadingCanvas;

    [Header("Text")]
    [SerializeField] private TMP_Text gameOverText;

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        gameManager.OnGameWon += HandleGameWonCanvas;
        gameManager.OnGameLost += HandleGameLostCanvas;
    }

    private void OnDisable()
    {
        gameManager.OnGameWon -= HandleGameWonCanvas;
        gameManager.OnGameLost -= HandleGameLostCanvas;
    }

    private void Start()
    {
        mainMenuCanvas.SetActive(true);
        
        creditsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        warningCanvas.SetActive(false);

        loadingCanvas.SetActive(false);
    }

    private void Update()
    {
        if (!loadBar) return;
        loadBar.value = (LoaderManager.Get().loadingProgress);
    }

    private void HandleGameWonCanvas()
    {
        OnOpenAndCloseCanvas(gameOverCanvas);
        gameOverText.text = $"Player " + gameManager.GetPlayerNumber() + " has won!";
    }

    private void HandleGameLostCanvas()
    {
        OnOpenAndCloseCanvas(gameOverCanvas);
        gameOverText.text = $"One of your players died before the enemies!";
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

    //HowToPlay Button
    public void OnOpenAndCloseHowToPlay()
    {
        OnOpenAndCloseCanvas(howToPlayCanvas);
    }

    //Play Button
    public void OnPlay()
    {
        OnOpenAndCloseCanvas(mainMenuCanvas);
    }

    //Menu Button
    public void OnOpenWarning()
    {
        OnOpenAndCloseCanvas(warningCanvas);
    }

    public void OnBackToMainMenu()
    {
        loadBar.value = 0;
        OnOpenAndCloseCanvas(loadingCanvas);
        levelManager.RestartGame();
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
