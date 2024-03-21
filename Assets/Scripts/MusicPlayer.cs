using UnityEngine;

// Manages the background music
public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance; // Stores one copy of music player for the whole game
    // References
    [SerializeField] AudioClip mainMusic;
    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] AudioClip victoryMusic;
    AudioSource audioSource;

    // Awake is called when this script object is initialized
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayMainMusic();
        if(!instance) 
        {
            instance = this;
            DontDestroyOnLoad(this); // Makes the music player persist across different scenes
        } else // Destroy the new music player if one already exists
        {
            Destroy(gameObject); 
        }
        
    }

    public void PlayMainMusic()
    {
        audioSource.clip = mainMusic;
        audioSource.Play();
    }

    public void PlayGameOverMusic()
    {
        audioSource.clip = gameOverMusic;
        audioSource.Play();
    }

    public void PlayVictoryMusic()
    {
        audioSource.clip = victoryMusic;
        audioSource.Play();
    }
}
