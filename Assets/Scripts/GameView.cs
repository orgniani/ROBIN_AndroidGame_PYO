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
    [SerializeField] private GameObject healer;
    [SerializeField] private GameObject ranger;
    [SerializeField] private GameObject fighter;

    private List<List<GameObject>> grid;
    private GameController gameController;

    private int turn = 1;
    private bool gameOver = false;

    private int maxSpeed = 2;
    private bool waitingForDice = false;

    private void Awake()
    {
        player = fighter;
    }

    private void Start()
    {
        gameController = new GameController(this, new MapBuilder());
        StartCoroutine(PlayTurn());
    }

    private void Update()
    {
        if (waitingForDice) return;

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

        UpdateCharacter();
        gameController.UpdateCharacterPosition(turn);

        yield return WaitForMovement();

        gameController.StoreCharacterPosition(turn);
        //turn = (turn == 1) ? 2 : 1;
        turn = (turn % 3) + 1;

        yield return new WaitForSeconds(2);
        StartCoroutine(PlayTurn());
    }

    private void UpdateCharacter()
    {
        if (turn == 1)
            player = fighter;
        else if (turn == 2)
            player = healer;
        else if (turn == 3)
            player = ranger;
    }
    private IEnumerator WaitForMovement()
    {
        waitingForDice = false;

        while (gameController.speed < maxSpeed )
        {
            yield return new WaitForEndOfFrame();
        }

        //while (diceResult == 0)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        gameController.speed = 0;
        waitingForDice = true;
    }

    public void OnChooseAction()
    {
        if (waitingForDice)
            return;

        //System.Random r = new System.Random();
        //
        //int resultado = r.Next(1, 7);
        //labelDiceResult.text = resultado.ToString();
        //
        //diceResult = resultado;
    }

    public void ShowWinFeedback()
    {
        Debug.Log("YOU WIN!!!");
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

        player = healer;
        MovePlayerToCell(healerPosition.x, healerPosition.y);

        player = ranger;
        MovePlayerToCell(rangerPosition.x, rangerPosition.y);

        player = fighter;
    }   
}
