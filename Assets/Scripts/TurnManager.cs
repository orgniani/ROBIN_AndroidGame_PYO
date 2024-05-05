using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> players; // Changed from characters to players
    [SerializeField] private GameController gameController;

    private int currentPlayerIndex; // Keep track of the current player index

    private void Start()
    {
        currentPlayerIndex = 0;
        //gameController = new GameController(FindObjectOfType<GameView>(), new MapBuilder(), players[currentPlayerIndex]);
    }

    private void Update()
    {
        // Check for player input here if needed
    }

    // If players move automatically, call this method to move to the next player's turn
    public void EndTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count; // Move to the next player
        //gameController.SetCurrentPlayer(players[currentPlayerIndex]); // Set the current player in GameController
    }
}
