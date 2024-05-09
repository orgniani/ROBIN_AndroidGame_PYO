using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController
{
    private Vector2Int characterPosition;

    private Vector2Int healerPosition;
    private Vector2Int rangerPosition;
    private Vector2Int fighterPosition;

    private List<Vector2Int> positions;

    public List<List<TerrainType>> map;

    private GameView gameView;

    public int speed = 0;

    public GameController(GameView view, MapBuilder mapBuilder)
    {
        gameView = view;

        map = mapBuilder.GenerateMap();

        fighterPosition = mapBuilder.GetStartPosition();
        healerPosition = mapBuilder.GetStartPosition();
        rangerPosition = mapBuilder.GetStartPosition();

        positions = new List<Vector2Int>() { fighterPosition , healerPosition, rangerPosition };
        characterPosition = positions[0];

        gameView.InitializeMap(map);
        gameView.InitializeCharacterPositions(positions);

        map[fighterPosition.y][fighterPosition.x] = TerrainType.CHARACTER;
        map[healerPosition.y][healerPosition.x] = TerrainType.CHARACTER;
        map[rangerPosition.y][rangerPosition.x] = TerrainType.CHARACTER;
    }

    public void MoveCharacterRight()
    {
        if (IsValidPosition(characterPosition.x + 1, characterPosition.y))
        {
            MoveCharacterToPosition(characterPosition.x + 1, characterPosition.y);
        }
    }

    public void MoveCharacterLeft()
    {
        if (IsValidPosition(characterPosition.x - 1, characterPosition.y))
        {
            MoveCharacterToPosition(characterPosition.x - 1, characterPosition.y);
        }
    }

    public void MoveCharacterUp()
    {
        if (IsValidPosition(characterPosition.x, characterPosition.y + 1))
        {
            MoveCharacterToPosition(characterPosition.x, characterPosition.y + 1);
        }
    }

    public void MoveCharacterDown()
    {
        if (IsValidPosition(characterPosition.x, characterPosition.y - 1))
        {
            MoveCharacterToPosition(characterPosition.x, characterPosition.y - 1);
        }
    }

    /*
    public void CheckDistancesBetweenCharacter(int turn)
    {
        Vector2Int currentPlayerPosition = positions[turn - 1];

        for (int i = 0; i < positions.Count; i++)
        {
            if (i != turn - 1)
            {
                Vector2Int otherPosition = positions[i];

                int manhattanDistance = ManhattanDistance(currentPlayerPosition, otherPosition);

                Debug.Log("Manhattan distance between character " + turn + " and character " + (i + 1) + ": " + manhattanDistance);
            }
        }
    }

    private int ManhattanDistance(Vector2Int position1, Vector2Int position2)
    {
        return Mathf.Abs(position1.x - position2.x) + Mathf.Abs(position1.y - position2.y);
    }

    private bool IsAdjacent(Vector2Int player, Vector2Int target, int maxDistance)
    {
        float distance = Vector2.Distance(player, target);

        Debug.Log("MELEE DISTANCE: " + distance);

        return distance <= maxDistance;
    }

    private bool IsInRange(Vector2Int player, Vector2Int target, int maxRange)
    {
        float distance = Vector2.Distance(player, target);

        Debug.Log("RANGE DISTANCE: " + distance);

        return distance > 1 && distance <= maxRange;
    }

    */

    public void StoreCharacterPosition(int turn)
    {
        positions[turn-1] = characterPosition;
        //map[positions[turn-1].y][positions[turn-1].x] = TerrainType.CHARACTER;
    }

    public void UpdateCharacterPosition(int turn)
    {
        characterPosition = positions[turn-1];
    }

    public void RemovePositionAfterDeath(int index, int turn)
    {
        if (index == turn - 1)
        {
            positions[index] = characterPosition;
        }

        map[positions[index].y][positions[index].x] = TerrainType.GRASS;
    }

    private void MoveCharacterToPosition(int newX, int newY)
    {
        if (newX == characterPosition.x && newY == characterPosition.y)
        {
            return;
        }

        map[characterPosition.y][characterPosition.x] = TerrainType.GRASS;
        map[newY][newX] = TerrainType.CHARACTER;

        characterPosition = new Vector2Int(newX, newY);

        gameView.MovePlayerToCell(characterPosition.x, characterPosition.y);
        speed++;

        CheckIfWin();
    }

    private void CheckIfWin()
    {
        //if (map[characterPosition.y][characterPosition.x] == TerrainType.FINISH)
          //  gameView.ShowWinFeedback();
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
