using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VictoryCanvas : MonoBehaviour
{
    // References
    [SerializeField] GameObject victoryBackground;
    [SerializeField] Button mainMenuButton; // Button for returning to the main menu

    // Variables
    bool isVictory = false;
    public bool IsVictory { get { return isVictory; } }

    // Start is called before the first frame update
    void Start()
    {
        victoryBackground.SetActive(false);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    // Trigger the game over screen and player the game over music
    public void TriggerVictory()
    {
        isVictory = true;
        MusicPlayer.instance.PlayVictoryMusic();
        victoryBackground.SetActive(true);
    }

    // Return to the title screen and play the main music
    void ReturnToMainMenu()
    {
        MusicPlayer.instance.PlayMainMusic();
        SceneManager.LoadScene(0);
    }
}
