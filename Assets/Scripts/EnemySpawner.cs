using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Handles spawning the enemies
public class EnemySpawner : MonoBehaviour
{
    // Constants
    const int finalWave = 100; // Final wave before the player wins the game
    // References
    [SerializeField] Enemy[] enemyPool; // The list of possible types of enemy it can spawn
    [SerializeField] Button nextWaveButton; // Button you press to advance to the next wave
    [SerializeField] TextMeshProUGUI waveText; // Text which represents the current wave number
    [SerializeField] List<MovementPoint> movementPoints;
    [SerializeField] VictoryCanvas victoryCanvas;
    [SerializeField] PlayerResourceManager playerResourceManager;
    public List<MovementPoint> MovementPoints
    {
        get { return movementPoints; }
    }

    // Constants
    const float spawnDelay = 1f; // The amount of time between each enemy spawn
    const int normalEnemiesPerWave = 30;
    const int bossEnemiesPerWave = 1;
    
    // Variables
    [SerializeField] int enemiesPerWave;
    [SerializeField] int enemiesDied = 0;
    [SerializeField] int currentWave = 0;
    [SerializeField] bool isStandby = true; // isStandby = true when there are no enemies currently on the map and false otherwise
    [SerializeField] bool spawnSequenceStarted = false; // Ensures that the full spawn sequence for the current wave only runs once

    public int CurrentWave { get { return currentWave; }}
    public bool IsStandby { get { return isStandby; }}

    // Start is called before the first frame update
    void Start()
    {
        enemiesPerWave = normalEnemiesPerWave;
        nextWaveButton.interactable = true; // Make the next wave button clickable at the start of the game
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStandby && !spawnSequenceStarted) // Starts the spawn sequence if the user clicks on the next wave button
        {
            nextWaveButton.interactable = false;
            spawnSequenceStarted = true;
            StartCoroutine(SpawnEnemies());
        }
        if(enemiesDied >= enemiesPerWave && spawnSequenceStarted) // Makes the next wave button clickable after all enemies of the current wave are defeated
        {
            if(currentWave >= finalWave)
            {
                // Trigger victory screen if the player still have some health left. 
                // The game over screen would be triggered instead if player ran out of health when all enemies are defeated.
                if (!victoryCanvas.IsVictory && playerResourceManager.CurrentHealth > 0)
                {
                    victoryCanvas.TriggerVictory();
                }
                
            } else
            {
                nextWaveButton.interactable = true;
                spawnSequenceStarted = false;
                enemiesDied = 0;
                isStandby = true;
                currentWave++;
            }
                
            
        }
        if (currentWave % 10 == 0 && currentWave > 0) // Spawns boss enemy every 10th wave
        {
            enemiesPerWave = bossEnemiesPerWave;
        }
        else
        {
            enemiesPerWave = normalEnemiesPerWave;
        }
        waveText.text = "Wave: " + currentWave;
    }

    public void CountOneKill()
    {
        enemiesDied++;
    }

    public void StartNextWave()
    {
        isStandby = false;
    }

    // Spawns a random enemy from the pool after every spawnDelay time has passed
    IEnumerator SpawnEnemies()
    {
        if(enemyPool.Length > 0)
        {
            for(int i = 0; i < enemiesPerWave; i++)
            {
                Enemy randomEnemy = enemyPool[Random.Range(0, enemyPool.Length)];
                Instantiate(randomEnemy, Vector2.right * 999999, Quaternion.identity, transform);
                yield return new WaitForSeconds(spawnDelay);
            }
            
        }
        
    }
}
