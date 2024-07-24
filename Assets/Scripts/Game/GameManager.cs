using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private List<Player> players;

    [Header("References")]
    [SerializeField] private ActionController actionController;
    [SerializeField] private GameView gameView;

    [Header("Logs")]
    [SerializeField] private bool enableLogs = false;

    private Player currentPlayer;

    private bool[] playerDeadFlags;

    private int playersTotal = 0;
    private int playerCounter = 0;
    private int enemiesCounter = 0;

    private int turn = 1;
    private int maxTurns = 0;

    private MovementController movementController;

    public MovementController MovementController => movementController;

    public bool IsWaitingForMovement { set; get; } 

    public bool GameOver { get; private set; }

    public event Action<Player> OnUpdatePlayer;
    public event Action<GameOverReason, Player> OnGameOver;
    public event Action OnPlayerDeath;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        ValidateReferences();
    }

    private void OnEnable()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].onDead += KillCounter;
        }
    }

    private void Start()
    {
        currentPlayer = players[0];

        gameView.SetCurrentPlayer(currentPlayer.gameObject);
        gameView.SetPlayers(players.ConvertAll(p => p.gameObject));

        actionController.Initialize(players);
        actionController.SetCurrentPlayer(currentPlayer);

        maxTurns = players.Count;

        playerDeadFlags = new bool[players.Count];

        for (int i = 0; i < playerDeadFlags.Length; i++)
        {
            playerDeadFlags[i] = false;

            if (players[i].IsEnemy) enemiesCounter++;
            else
            {
                playerCounter++;
                playersTotal++;
            }
        }

        IsWaitingForMovement = true;

        movementController = new MovementController(gameView, new MapBuilder(), players.Count, players);
        StartCoroutine(PlayerTurn());
    }

    private void Update()
    {
        if (!IsWaitingForMovement || GameOver) return;

        if (currentPlayer.IsEnemy)
        {
            movementController.MoveEnemyRandomly();
            return;
        }


#if UNITY_EDITOR

        Dictionary<KeyCode, Action> movementActions = new Dictionary<KeyCode, Action>
        {
        {KeyCode.LeftArrow, movementController.MoveCharacterLeft},
        {KeyCode.A, movementController.MoveCharacterLeft},
        {KeyCode.RightArrow, movementController.MoveCharacterRight},
        {KeyCode.D, movementController.MoveCharacterRight},
        {KeyCode.UpArrow, movementController.MoveCharacterUp},
        {KeyCode.W, movementController.MoveCharacterUp},
        {KeyCode.DownArrow, movementController.MoveCharacterDown},
        {KeyCode.S, movementController.MoveCharacterDown}
        };

        foreach (var kvp in movementActions)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                kvp.Value.Invoke();
                break;
            }
        }
#endif
    }

    private IEnumerator PlayerTurn()
    {
        if (GameOver) yield break;

        bool currentPlayerIsDead = IsPlayerDead(turn);

        if (!currentPlayerIsDead)
        {
            UpdateCharacter();
            OnUpdatePlayer?.Invoke(currentPlayer);

            movementController.UpdateCharacterPosition(turn);

            yield return WaitForMovement();
            yield return WaitForAction();

            movementController.StoreCharacterPosition(turn);

            yield return new WaitForSeconds(1f);
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
        currentPlayer = players[turn - 1];

        gameView.SetCurrentPlayer(currentPlayer.gameObject);
        actionController.SetCurrentPlayer(currentPlayer);
    }

    private IEnumerator WaitForMovement()
    {
        IsWaitingForMovement = true;

        while (movementController.Speed < players[turn - 1].GetMaxSpeed() && IsWaitingForMovement == true)
        {
            yield return new WaitForEndOfFrame();
        }

        IsWaitingForMovement = false;
    }

    private IEnumerator WaitForAction()
    {
        while (!actionController.HasChosenAction)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    private void CheckIfGameOver()
    {
        if (playerCounter == 1)
        {
            GameOver = true;
            OnGameOver?.Invoke(GameOverReason.WIN, currentPlayer);

            actionController.gameObject.SetActive(false);
        }

        else if (playerCounter < playersTotal && enemiesCounter > 0)
        {
            GameOver = true;
            OnGameOver?.Invoke(GameOverReason.LOSE, currentPlayer);

            actionController.gameObject.SetActive(false);
        }
    }

    private void KillCounter()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].Health == 0)
            {
                if (enableLogs)
                    Debug.Log("Player " + (i + 1) + " has died.");

                OnPlayerDeath?.Invoke();
                playerDeadFlags[i] = true;

                if (players[i].IsEnemy)
                {
                    enemiesCounter--;
                }

                else
                {
                    playerCounter--;
                }

                movementController.RemovePositionAfterDeath(i, turn);
            }
        }
    }

    private void ValidateReferences()
    {
        if (!actionController)
        {
            Debug.LogError($"{name}: {nameof(actionController)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (!gameView)
        {
            Debug.LogError($"{name}: {nameof(gameView)} is null!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }

        if (players.Count <= 0)
        {
            Debug.LogError($"{name}: There are no players in the players list!" +
                           $"\nDisabling object to avoid errors.");
            enabled = false;
            return;
        }
    }
}
