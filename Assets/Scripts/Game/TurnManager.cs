using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private Player player;
    [SerializeField] private List<Player> players;
    [SerializeField] private List<HealthController> playersHP;

    private bool[] playerDeadFlags;

    [SerializeField] private ActionMenuController menuController;
    [SerializeField] private GameView gameView;

    [SerializeField] private int enemiesAmount = 2;
    [SerializeField] private int playersAmount = 3;
    private int playerCounter;

    private int turn = 1;
    public bool gameOver = false;

    [SerializeField] private int maxTurns = 3;
    public bool waitingForMovement = true;

    public bool isEnemyTurn = false;

    [SerializeField] private bool enableLogs = false;

    public GameController gameController;

    private void Awake()
    {
        player = players[0];

        gameView.SetCurrentPlayer(player.gameObject);
        gameView.SetPlayers(players.ConvertAll(p => p.gameObject));

        menuController.SetCurrentPlayer(player);
        menuController.SetPlayers(players);

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
                if(enableLogs)
                    Debug.Log("Player " + (i + 1) + " has died.");

                playerDeadFlags[i] = true;

                if (i == 3 || i == 4)
                {
                    enemiesAmount--;
                    CheckIfGameOver();

                    if (enableLogs)
                        Debug.Log("ENEMIES: " + enemiesAmount);
                }

                else
                {
                    playersAmount--;
                    if (enableLogs)
                        Debug.Log("PLAYERS: " + playersAmount);
                }

                gameController.RemovePositionAfterDeath(i, turn);
            }
        }

        playerCounter--;
    }

    private void Start()
    {
        gameController = new GameController(gameView, new MapBuilder());
        StartCoroutine(PlayerTurn());
    }

    private void Update()
    {
        //if (!waitingForMovement || gameOver) return;
        if (!waitingForMovement) return;
        if (gameOver) return;

        Dictionary<KeyCode, Action> movementActions = new Dictionary<KeyCode, Action>
        {
        {KeyCode.LeftArrow, gameController.MoveCharacterLeft},
        {KeyCode.A, gameController.MoveCharacterLeft},
        {KeyCode.RightArrow, gameController.MoveCharacterRight},
        {KeyCode.D, gameController.MoveCharacterRight},
        {KeyCode.UpArrow, gameController.MoveCharacterUp},
        {KeyCode.W, gameController.MoveCharacterUp},
        {KeyCode.DownArrow, gameController.MoveCharacterDown},
        {KeyCode.S, gameController.MoveCharacterDown}
        };

        if (!isEnemyTurn)
        {
            foreach (var kvp in movementActions)
            {
                if (Input.GetKeyDown(kvp.Key))
                {
                    kvp.Value.Invoke();
                    break;
                }
            }
        }

        else
        {
            gameController.MoveEnemyRandomly();
        }
    }

    private IEnumerator PlayerTurn()
    {
        if (gameOver) yield break;

        if(turn == 4) isEnemyTurn = true;
        if (turn == 1) isEnemyTurn = false;

        bool currentPlayerIsDead = IsPlayerDead(turn);

        if (!currentPlayerIsDead)
        {
            UpdateCharacter();

            gameController.UpdateCharacterPosition(turn);

            yield return WaitForMovement();
            yield return WaitForAction();

            gameController.StoreCharacterPosition(turn);
        }

        CheckIfGameOver();
        turn = (turn % maxTurns) + 1;

        StartCoroutine(PlayerTurn());
    }

    private bool IsPlayerDead(int playerTurn)
    {
        return playerDeadFlags[playerTurn - 1];
    }

    private void UpdateCharacter()
    {
        player = players[turn - 1];

        gameView.SetCurrentPlayer(player.gameObject);
        menuController.SetCurrentPlayer(player);
    }

    private IEnumerator WaitForMovement()
    {
        waitingForMovement = true;

        while (gameController.speed < players[turn - 1].GetMaxSpeed() && waitingForMovement == true)
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

    public void CheckIfGameOver()
    {
        if (playerCounter == 1)
        {
            Debug.Log("YOU WIN!!!");
            gameOver = true;
        }

        else if (playersAmount < 3 && enemiesAmount >= 0)
        {
            Debug.Log("EVERYBODY LOSES!!!");
            gameOver = true;
        }

    }
}
