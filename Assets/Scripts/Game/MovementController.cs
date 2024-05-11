using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovementController
{
    private Vector2Int currentPlayerPosition;
    private List<Vector2Int> positions = new List<Vector2Int>();

    private List<List<TerrainType>> map;

    private GameView gameView;

    public int Speed { private set; get; }

    public MovementController(GameView view, MapBuilder mapBuilder, int playerAmount)
    {
        gameView = view;

        map = mapBuilder.GenerateMap();
        gameView.InitializeMap(map);

        for (int i = 0; i < playerAmount; i++)
        {
            Vector2Int newPosition = mapBuilder.GetStartPosition();
            positions.Add(newPosition);
            map[newPosition.y][newPosition.x] = TerrainType.CHARACTER;
        }

        currentPlayerPosition = positions[0];
        gameView.InitializeCharacterPositions(positions);
    }

    public void MoveCharacterRight()
    {
        if (IsValidPosition(currentPlayerPosition.x + 1, currentPlayerPosition.y))
        {
            MoveCharacterToPosition(currentPlayerPosition.x + 1, currentPlayerPosition.y);
        }
    }

    public void MoveCharacterLeft()
    {
        if (IsValidPosition(currentPlayerPosition.x - 1, currentPlayerPosition.y))
        {
            MoveCharacterToPosition(currentPlayerPosition.x - 1, currentPlayerPosition.y);
        }
    }

    public void MoveCharacterUp()
    {
        if (IsValidPosition(currentPlayerPosition.x, currentPlayerPosition.y + 1))
        {
            MoveCharacterToPosition(currentPlayerPosition.x, currentPlayerPosition.y + 1);
        }
    }

    public void MoveCharacterDown()
    {
        if (IsValidPosition(currentPlayerPosition.x, currentPlayerPosition.y - 1))
        {
            MoveCharacterToPosition(currentPlayerPosition.x, currentPlayerPosition.y - 1);
        }
    }

    public void MoveEnemyRandomly()
    {
        int randomDirection = Random.Range(0, 4);

        switch (randomDirection)
        {
            case 0:
                MoveCharacterRight();
                break;
            case 1:
                MoveCharacterLeft();
                break;
            case 2:
                MoveCharacterUp();
                break;
            case 3:
                MoveCharacterDown();
                break;
        }
    }

    public void StoreCharacterPosition(int turn)
    {
        positions[turn-1] = currentPlayerPosition;
    }

    public void UpdateCharacterPosition(int turn)
    {
        currentPlayerPosition = positions[turn-1];
        Speed = 0;
    }

    public void RemovePositionAfterDeath(int index, int turn)
    {
        if (index == turn - 1)
        {
            positions[index] = currentPlayerPosition;
        }

        map[positions[index].y][positions[index].x] = TerrainType.GRASS;
    }

    private void MoveCharacterToPosition(int newX, int newY)
    {
        if (newX == currentPlayerPosition.x && newY == currentPlayerPosition.y)
        {
            return;
        }

        map[currentPlayerPosition.y][currentPlayerPosition.x] = TerrainType.GRASS;
        map[newY][newX] = TerrainType.CHARACTER;

        currentPlayerPosition = new Vector2Int(newX, newY);

        gameView.MovePlayerToCell(currentPlayerPosition.x, currentPlayerPosition.y);
        Speed++;
    }

    private bool IsValidPosition(int x, int y)
    {
        return PositionExistsInMap(x, y) && ThereIsNoCharacterInPosition(x, y);
    }

    private bool ThereIsNoCharacterInPosition(int x, int y)
    {
        return map[y][x] == TerrainType.GRASS;
    }

    private bool PositionExistsInMap(int x, int y)
    {
        return y >= 0 &&
               y < map.Count &&
               x >= 0 &&
               x < map[y].Count;
    }
}
