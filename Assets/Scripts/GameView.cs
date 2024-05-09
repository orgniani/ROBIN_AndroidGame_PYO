using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour
{
    [SerializeField] private float gridCellSize = 1f;
    [SerializeField] private float spaceBetweenCells = 0.1f;

    [SerializeField] private List<GameObject> terrainPrefabs;

    private GameObject player;
    private List<GameObject> playerObjects = new List<GameObject>();

    private List<List<GameObject>> grid;

    public void SetPlayers(List<GameObject> players)
    {
        for (int i = 0; i < players.Count; i++)
        {
            playerObjects.Clear();
            playerObjects.AddRange(players);
        }
    }

    public void SetCurrentPlayer(GameObject currentPlayer)
    {
        player = currentPlayer;
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

    public void InitializeCharacterPositions(List<Vector2Int> playerPositions)
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            player = playerObjects[i];
            MovePlayerToCell(playerPositions[i].x, playerPositions[i].y);
        }

        player = playerObjects[0];
    }

}
