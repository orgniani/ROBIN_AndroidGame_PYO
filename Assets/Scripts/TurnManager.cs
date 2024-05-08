using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Player player;
    [SerializeField] private List<Player> players;
    [SerializeField] private List<HealthController> playersHP;
    private bool[] playerDeadFlags;

    private int playerCounter = 0;

    [SerializeField] private ActionMenuController menuController;
    [SerializeField] private GameView gameView;

    private int turn = 1;
    public bool gameOver = false;

    private int maxTurns = 3;
    public bool waitingForMovement = true;

    public GameController gameController;

    private void Awake()
    {
        player = players[0];

        gameView.SetCurrentPlayer(player.gameObject);
        gameView.SetPlayers(players.ConvertAll(p => p.gameObject));

        playerCounter = players.Count;

        playerDeadFlags = new bool[players.Count];

        for (int i = 0; i < playerDeadFlags.Length; i++)
        {
            playerDeadFlags[i] = false;
        }
    }

    private void OnEnable()
    {
        for (int i = playersHP.Count - 1; i >= 0; i--)
        {
            playersHP[i].onDead += KillCounter;
        }
    }

    private void KillCounter()
    {
        for (int i = 0; i < playersHP.Count; i++)
        {
            if (playersHP[i].Health <= 0)
            {
                Debug.Log("Player " + (i + 1) + " has died.");
                playerDeadFlags[i] = true;

                gameController.RemovePositionAfterDeath(i, turn);
            }
        }

        playerCounter--;
    }

    private void Start()
    {
        gameController = new GameController(gameView, new MapBuilder());
        StartCoroutine(PlayTurn());
    }

    private void Update()
    {
        if (!waitingForMovement) return;
        if (gameOver) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            gameController.MoveCharacterLeft();
        else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            gameController.MoveCharacterRight();
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            gameController.MoveCharacterUp();
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            gameController.MoveCharacterDown();
    }

    private IEnumerator PlayTurn()
    {
        if (gameOver) yield break;

        bool currentPlayerIsDead = IsPlayerDead(turn);

        if (!currentPlayerIsDead)
        {
            UpdateCharacter();

            gameController.UpdateCharacterPosition(turn);

            yield return WaitForMovement();
            yield return WaitForAction();

            gameController.StoreCharacterPosition(turn);
        }

        ShowWinFeedback();
        turn = (turn % maxTurns) + 1;

        StartCoroutine(PlayTurn());
    }

    private bool IsPlayerDead(int playerTurn)
    {
        return playerDeadFlags[playerTurn - 1];
    }

    private void UpdateCharacter()
    {
        player = players[turn - 1];
        gameView.SetCurrentPlayer(player.gameObject);
    }

    private IEnumerator WaitForMovement()
    {
        waitingForMovement = true;

        while (gameController.speed < players[turn - 1].GetMaxSpeed())
        {
            yield return new WaitForEndOfFrame();
        }

        waitingForMovement = false;
    }

    private IEnumerator WaitForAction()
    {
        while (!menuController.chooseAction)
        {
            yield return new WaitForEndOfFrame();
        }

        gameController.speed = 0;
        menuController.chooseAction = false;
    }

    public void ShowWinFeedback()
    {
        if (playerCounter == 1)
        {
            Debug.Log("YOU WIN!!!");
            gameOver = true;
        }
    }
}
