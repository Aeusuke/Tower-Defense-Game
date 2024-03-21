using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

// Handles the main menu
public class MainMenuManager : MonoBehaviour
{
    // References
    [SerializeField] Canvas mainCanvas;
    [SerializeField] Canvas tutorialCanvas;
    [SerializeField] Button startButton; // Press this button to start the game
    [SerializeField] Button tutorialButton; // Press this button to view tutorial
    [SerializeField] Button returnButton; // Press this button to return to the title screen from the tutorial screen
    [SerializeField] Slider difficultySlider; // A slider which sets the enemy difficulty
    [SerializeField] TextMeshProUGUI difficultyDescriptionText; // Text which describes the difficulty
    
    // Start is called before the first frame update
    void Start()
    {
        SetToMainScreen();
        startButton.onClick.AddListener(StartMainGame);
        tutorialButton.onClick.AddListener(SetToTutorialScreen);
        returnButton.onClick.AddListener(SetToMainScreen);
        difficultySlider.onValueChanged.AddListener(SetDifficultyDescriptionText);
        difficultySlider.value = 0;
        difficultyDescriptionText.text = "EASY";
    }

    void SetDifficultyDescriptionText(float value)
    {
        if (value < 2) difficultyDescriptionText.text = "EASY";
        else if (value < 5) difficultyDescriptionText.text = "MODERATE";
        else if (value < 7) difficultyDescriptionText.text = "HARD";
        else if (value < 9) difficultyDescriptionText.text = "VERY HARD";
        else difficultyDescriptionText.text = "EXTREME";
    }
    void StartMainGame()
    {
        PlayerPrefs.SetInt("Difficulty", Mathf.RoundToInt(difficultySlider.value)); // Save the difficulty the player chose into the registry
        SceneManager.LoadScene(1); // Load the main scene
    }

    void SetToMainScreen()
    {
        mainCanvas.enabled = true;
        tutorialCanvas.enabled = false;
    }

    void SetToTutorialScreen()
    {
        mainCanvas.enabled = false;
        tutorialCanvas.enabled = true;
    }
}
