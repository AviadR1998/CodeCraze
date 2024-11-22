using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    // Public reference to GetNPostGameState
    private GameStateController gameStateController;

    private void Start()
    {
        gameStateController = gameObject.GetComponent<GameStateController>();
        // Check if the reference has been set
        if (gameStateController != null)
        {
            // Use the GetState function
            StartCoroutine(gameStateController.GetState(OnGameStateReceived));
        }
        else
        {
            Debug.LogError("GetNPostGameState reference is not set in the Inspector.");
        }
    }

    // Callback function to handle the received GameState
    private void OnGameStateReceived(GameState gameState)
    {
        if (gameState != null)
        {
            Debug.Log("GameState received successfully!");
            Debug.Log($"World: {gameState.world}, Task: {gameState.task}, State: {gameState.state}");
        }
        else
        {
            Debug.LogError("Failed to retrieve GameState.");
        }
    }
}
