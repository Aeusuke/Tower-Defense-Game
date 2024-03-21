using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Handle the game over screen
public class GameOverCanvas : MonoBehaviour
{
    // References
    [SerializeField] GameObject gameOverBackground;
    [SerializeField] Button mainMenuButton; // Button for returning to the main menu

    // Variables
    bool isGameOver = false;
    public bool IsGameOver { get { return isGameOver; }}

    // Start is called before the first frame update
    void Start()
    {
        gameOverBackground.SetActive(false);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    // Trigger the game over screen and player the game over music
    public void TriggerGameOver()
    {
        isGameOver = true;
        MusicPlayer.instance.PlayGameOverMusic();
        gameOverBackground.SetActive(true);
    }

    // Return to the title screen and play the main music
    void ReturnToMainMenu()
    {
        MusicPlayer.instance.PlayMainMusic();
        SceneManager.LoadScene(0);
    }

}
