using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private Slider loadBar;

    [Header("Canvases")]
    [SerializeField] private GameObject[] allUICanvases;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private GameObject gameOverCanvas;

    [Header("Text")]
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text timeText;

    [SerializeField] private TMP_Text playerTurnText;
    [SerializeField] private TMP_Text playerActionText;

    private GameManager gameManager;
    private ActionController actionController;
    private MovementController movementController;

    private IButtonHandler buttonHandler;
    private ITimeCounter timeCounter;
    private ITextHandler textHandler;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        ValidateReferences();
    }

    private void OnEnable()
    {
        gameManager.OnInitialized += Initialize;
    }

    private void OnDisable()
    {
        gameManager.OnInitialized -= Initialize;

        gameManager.OnGameOver -= HandleGameOverCanvas;
        gameManager.OnUpdatePlayer -= textHandler.UpdatePlayerTurnText;

        actionController.OnActionChosen -= textHandler.HandleActionText;
        actionController.OnActionFailed -= textHandler.HandleActionFailedText;
    }

    private void Start()
    {
        foreach(var canvas in allUICanvases)
        {
            canvas.SetActive(false);
        }

        menuCanvas.SetActive(true);

        timeCounter.StartCounting();
    }

    private void Update()
    {
        timeCounter.UpdateTimeText();

        if (!loadBar) return;
        loadBar.value = (LoaderManager.GetLoaderManager().LoadingProgress);
    }

    private void Initialize()
    {
        actionController = gameManager.ActionController;
        movementController = gameManager.MovementController;

        buttonHandler = new UIButtonHandler(gameManager, movementController, levelManager, loadBar);
        timeCounter = new UITimeCounter(timeText);
        textHandler = new UITextHandler(gameOverText, playerTurnText, playerActionText);

        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        gameManager.OnGameOver += HandleGameOverCanvas;
        gameManager.OnUpdatePlayer += textHandler.UpdatePlayerTurnText;

        actionController.OnActionChosen += textHandler.HandleActionText;
        actionController.OnActionFailed += textHandler.HandleActionFailedText;
    }

    private void HandleGameOverCanvas(GameOverReason reason, Player player)
    {
        timeCounter.StopCounting();

        textHandler.HandleGameOverText(reason, player);

        buttonHandler.OnOpenAndCloseCanvas(gameOverCanvas);
    }

    // UI Button Methods
    public void OnMoveUp() => buttonHandler.OnMoveUp();
    public void OnMoveDown() => buttonHandler.OnMoveDown();
    public void OnMoveLeft() => buttonHandler.OnMoveLeft();
    public void OnMoveRight() => buttonHandler.OnMoveRight();

    public void OnOpenAndCloseCanvas(GameObject canvas) => buttonHandler.OnOpenAndCloseCanvas(canvas);
    public void OnBackToMainMenu(GameObject loadingCanvas) => buttonHandler.OnBackToMainMenu(loadingCanvas);

    private void ValidateReferences()
    {
        if (!levelManager)
        {
            Debug.LogError($"{name}: {nameof(levelManager)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!loadBar)
        {
            Debug.LogError($"{name}: {nameof(loadBar)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!gameOverCanvas)
        {
            Debug.LogError($"{name}: {nameof(gameOverCanvas)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!gameOverText)
        {
            Debug.LogError($"{name}: {nameof(gameOverText)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!timeText)
        {
            Debug.LogError($"{name}: {nameof(timeText)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }
    }
}