using UnityEngine;
using TMPro;

// Manages the player resources
public class PlayerResourceManager : MonoBehaviour
{
    // References
    [SerializeField] EnemySpawner spawner;
    [SerializeField] Transform towerStorage; // The location where the built towers are stored
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI powerText;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] GameOverCanvas gameOverCanvas;
    // Constants
    const int initialCoins = 100;
    const int initialPower = 100;
    const int initialHealth = 100;
    const int coinGeneratorDelay = 4; // The time in seconds between each coin generation cycle
    const int powerGeneratorDelay = 4;
    const int healthGeneratorDelay = 100;
    // Variables
    [SerializeField] int currentCoins = 0;
    [SerializeField] int currentPower = 0;
    [SerializeField] int currentHealth = 0;
    float coinGeneratorDelayedTime = 0; // Time in seconds that have passed so far since the start of coin generation cycle
    float powerGeneratorDelayedTime = 0;
    float healthGeneratorDelayedTime = 0;
    
    public int CurrentCoins { get { return currentCoins; }}
    public int CurrentPower { get { return currentPower; }}
    public int CurrentHealth { get { return currentHealth; }}

    // Start is called before the first frame update
    void Start()
    {
        InitializePlayerResources();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateResourceText();
        ManageGenerators();

        if (currentHealth <= 0 && !gameOverCanvas.IsGameOver)
        {
            gameOverCanvas.TriggerGameOver();   
        }

    }
    
    // Initialize the player's resource count
    void InitializePlayerResources()
    {
        currentCoins = initialCoins;
        currentPower = initialPower;
        currentHealth = initialHealth;
    }

    // Update the resource text on UI to represent the current resource count
    void UpdateResourceText()
    {
        coinText.text = currentCoins.ToString();
        powerText.text = currentPower.ToString();
        healthText.text = currentHealth.ToString();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
 
    }
    public void AddPower(int amount)
    {
        currentPower += amount;
     
    }
    public void AddHealth(int amount)
    {
        currentHealth += amount;  
    }

    // Manage the resource generation towers
    void ManageGenerators()
    {
        int totalCoinGeneratorAmount = 0;
        int totalPowerGeneratorAmount = 0;
        int totalHealthGeneratorAmount = 0;
        Tower[] towers = towerStorage.GetComponentsInChildren<Tower>();
        foreach (Tower tower in towers)
        {
            totalCoinGeneratorAmount += tower.CurrentCoinGeneratedPerCycle;
            totalPowerGeneratorAmount += tower.CurrentPowerGeneratredPerCycle;
            totalHealthGeneratorAmount += tower.CurrentHealthGeneratedPerCycle;
        }
        
        if (!spawner.IsStandby) // Makes sure that resources can only be generated when enemies are present on map
        {
            coinGeneratorDelayedTime += Time.deltaTime;
            powerGeneratorDelayedTime += Time.deltaTime;
            healthGeneratorDelayedTime += Time.deltaTime;
            if (coinGeneratorDelayedTime >= coinGeneratorDelay)
            {
                currentCoins += totalCoinGeneratorAmount;
               
                coinGeneratorDelayedTime = 0;
            }
            if (powerGeneratorDelayedTime >= powerGeneratorDelay)
            {
                currentPower += totalPowerGeneratorAmount;
                
                powerGeneratorDelayedTime = 0;
            }
            if (healthGeneratorDelayedTime >= healthGeneratorDelay)
            {
                currentHealth += totalHealthGeneratorAmount;
                
                healthGeneratorDelayedTime = 0;
            }
        }
    }

}
