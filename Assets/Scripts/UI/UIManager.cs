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
    [SerializeField] private TMP_Text timeText;

    private GameManager gameManager;

    private float startTime;
    private bool isCounting = false;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
    }

    private void OnEnable()
    {
        gameManager.OnGameOver += HandleGameOverCanvas;
    }

    private void OnDisable()
    {
        gameManager.OnGameOver -= HandleGameOverCanvas;
    }

    private void Start()
    {
        mainMenuCanvas.SetActive(true);
        
        creditsCanvas.SetActive(false);
        howToPlayCanvas.SetActive(false);
        gameOverCanvas.SetActive(false);
        warningCanvas.SetActive(false);

        loadingCanvas.SetActive(false);

        startTime = Time.time;
        isCounting = true;
    }

    private void Update()
    {
        if (isCounting)
        {
            float elapsedTime = Time.time - startTime;

            int minutes = (int)((elapsedTime % 3600) / 60);
            int seconds = (int)(elapsedTime % 60);

            timeText.text = $"{minutes:D2}:{seconds:D2}";
        }

        if (!loadBar) return;
        loadBar.value = (LoaderManager.Get().loadingProgress);
    }

    private void HandleGameOverCanvas(GameOverReason reason)
    {
        switch(reason)
        {
            case GameOverReason.WIN:
                gameOverText.text = $"Player " + gameManager.GetPlayerNumber() + " has won!";
                break;

            case GameOverReason.LOSE:
                gameOverText.text = $"One of your players died before the enemies!";
                break;
        }

        OnOpenAndCloseCanvas(gameOverCanvas);
        isCounting = false;
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
