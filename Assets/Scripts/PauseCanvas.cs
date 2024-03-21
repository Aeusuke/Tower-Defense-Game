using UnityEngine;
using UnityEngine.UI;

// Handles pausing the game
public class PauseCanvas : MonoBehaviour
{
    // References
    [SerializeField] Button pauseButton;
    [SerializeField] GameObject pauseBackground;
    
    // Variables
    bool isPaused = false;
    public bool IsPaused { get { return isPaused; }}

    // Start is called before the first frame update
    void Start()
    {
        pauseBackground.SetActive(false);
        pauseButton.onClick.AddListener(TogglePause);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    // Toggles between pausing and resuming the game
    void TogglePause()
    {
        isPaused = !isPaused;
        if(isPaused) // Shows the pause screen and pause the music when game is paused
        {
            pauseBackground.SetActive(true);
            MusicPlayer.instance.GetComponent<AudioSource>().Pause();
        } else
        {
            pauseBackground.SetActive(false);
            MusicPlayer.instance.GetComponent<AudioSource>().UnPause();
        }
    }
    
}
