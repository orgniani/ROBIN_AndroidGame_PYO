using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private float gridCellSize = 1f;
    [SerializeField] private float spaceBetweenCells = 0.1f;

    [SerializeField] private List<GameObject> terrainPrefabs;
    private GameObject player;

    [Header("Players")]
    [SerializeField] private List<Player> players;
    [SerializeField] private List<HealthController> playersHP;
    private bool[] playerDeadFlags;

    private int playerCounter = 0;

    [SerializeField] private ActionMenuController menuController;

    private List<List<GameObject>> grid;
    private GameController gameController;

    private int turn = 1;
    private bool gameOver = false;

    private int maxTurns = 3;
    public bool waitingForMovement = true;

    private void Awake()
    {
        player = players[0].gameObject;
        playerCounter = players.Count;

        Debug.Log(playerCounter);

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
                gameController.RemovePositionAfterDeath(i);
            }
        }

        playerCounter--;
    }

    private void Start()
    {
        gameController = new GameController(this, new MapBuilder());
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
        player = players[turn - 1].gameObject;
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
        if(playerCounter == 1)
        {
            Debug.Log("YOU WIN!!!");
            gameOver = true;
        }
    }

    private void MovePlayerToCell(GameObject gridCell)
    {
        player.transform.SetParent(gridCell.transform);
        player.transform.localPosition = Vector3.zero;
    }

    public void MovePlayerToCell(int row, int column)
    {
        MovePlayerToCell(grid[column][row]);
    }

    public void InitializeMap(List<List<TerrainType>> map)
    {
        grid = new List<List<GameObject>>();

        for (var row = 0; row < map.Count; row++)
        {
            var gridRow = new List<GameObject>();
            for (var column = 0; column < map[row].Count; column++)
            {
                var terrainType = map[row][column];

                var xPos = column * (gridCellSize + spaceBetweenCells);
                var yPos = row * (gridCellSize + spaceBetweenCells);

                var gridCell = Instantiate(terrainPrefabs[(int)terrainType], transform);

                gridCell.transform.localPosition = new Vector3(xPos, yPos, 1);
                gridRow.Add(gridCell);
            }
            grid.Add(gridRow);
        }
    }

    public void InitializeCharacterPositions(Vector2Int fighterPosition, Vector2Int healerPosition, Vector2Int rangerPosition)
    {
        MovePlayerToCell(fighterPosition.x, fighterPosition.y);

        player = players[1].gameObject;
        MovePlayerToCell(healerPosition.x, healerPosition.y);

        player = players[2].gameObject;
        MovePlayerToCell(rangerPosition.x, rangerPosition.y);

        player = players[0].gameObject;
    }   
}
