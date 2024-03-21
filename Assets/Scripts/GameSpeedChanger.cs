using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Handles the game speed slider
public class GameSpeedChanger : MonoBehaviour
{
    // References
    [SerializeField] TextMeshProUGUI speedText; // Displays the current game speed
    [SerializeField] Slider speedSlider;
    [SerializeField] PauseCanvas pauseCanvas;
    [SerializeField] GameOverCanvas gameOverCanvas;
    [SerializeField] VictoryCanvas victoryCanvas;
    // Variables
    float currentGameSpeed;
    public float CurrentGameSpeed { get { return currentGameSpeed; }}

    // Update is called once per frame
    void Update()
    {
        currentGameSpeed = speedSlider.value / 4f + 0.25f;
        speedText.text = "Game Speed\n" + currentGameSpeed + "x";
        if(pauseCanvas.IsPaused || gameOverCanvas.IsGameOver || victoryCanvas.IsVictory) // Freezes the game if the user pauses the game or gets a game over
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = currentGameSpeed;
        }   
    }

    
}
