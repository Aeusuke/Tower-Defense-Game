using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Represents the enemy
public class Enemy : MonoBehaviour
{
    // Constants
    const int speedLevelLimit = 100; // After this level, the enemy speed will no longer increase. This prevent the enemy from moving too fast.
    // References
    PlayerResourceManager statManager;
    EnemySpawner spawner;
    List<MovementPoint> movementPoints; // List of points the enemy will move to
    [SerializeField] GameObject hpBar;
    // Variables
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int attackPower = 1; // The number of health points the enemy remove from the player when the enemy reaches the player's castle
    [SerializeField] int maxHealth = 3;
    [SerializeField] int coinReward = 2; // The number of coins the player earns when defeating this enemy
    [SerializeField] int powerReward = 2; // The number of power the player earns when defeating this enemy
    [SerializeField] int defense = 0;
    float fullBarLength; // The length of the enemy's HP bar in world coordinates when it is full
    [SerializeField] float moveSpeedGrowth = 0.1f; // The amount of movement Speed the enemy gain for every increase in its level
    [SerializeField] float attackPowerGrowth = 0.04f;
    [SerializeField] float maxHealthGrowth = 1f;
    [SerializeField] float coinRewardGrowth = 0.1f;
    [SerializeField] float powerRewardGrowth = 0.1f;
    [SerializeField] float defenseGrowth = 0.1f;
    [SerializeField] int currentHealth = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        statManager = FindObjectOfType<PlayerResourceManager>();
        spawner = FindObjectOfType<EnemySpawner>();
        fullBarLength = hpBar.transform.localScale.y;
        SetStats();
        SetUpMovementPointsList();   
        if(movementPoints.Count < 2) { return; } // At least two points are needed for the enemy to interpolate its location
        
        StartCoroutine(MoveEnemy());
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleHealth();
    }

    // Initialize the enemy stats according to the current wave number
    void SetStats()
    {
        if(spawner)
        {
            int difficultyValue = PlayerPrefs.GetInt("Difficulty"); // Enemy difficulty the player chose in the main menu
            int level = spawner.CurrentWave;
            if(level > speedLevelLimit)
            {
                moveSpeed += moveSpeedGrowth * speedLevelLimit;
            } else
            {
                moveSpeed += moveSpeedGrowth * level;
            }
            attackPower += (int)(attackPowerGrowth * level);
         
            maxHealth += (int)(maxHealthGrowth * Mathf.Pow(level, difficultyValue * 0.06f + 1.2f));
            
            coinReward += (int)(coinRewardGrowth * level);
            
            powerReward += (int)(powerRewardGrowth * level);
           
            defense += (int)(defenseGrowth * level);
            if (spawner && spawner.CurrentWave % 10 == 0 && spawner.CurrentWave > 0) // The enemy becomes a "boss" enemy every 10th wave
            {
                moveSpeed *= 0.75f;
                transform.localScale *= 2;
                attackPower *= 10;
                maxHealth *= 10;
                coinReward *= 15;
                powerReward *= 15;
            }
            currentHealth = maxHealth;
        }
        
    }

    // Damage the enemy and deciding whether to take account of its defense stat
    public void DamageEnemy(int amount, bool piercesDefense)
    {
        if(piercesDefense) // Ignores enemy defense if the bullet has the pierce defense property
        {
            currentHealth -= amount;
        } else
        {
            if(amount - defense >= 0)
            {
                currentHealth = currentHealth - amount + defense;
            }
            
        }
        
    }

    // Updating the HP bar according to its current health and destroys this enemy and reward the player if it runs out of health
    void HandleHealth()
    {
        float hpPercent = (float) currentHealth / maxHealth;
        hpBar.transform.localScale = new Vector2(hpBar.transform.localScale.x, hpPercent * fullBarLength);
        hpBar.transform.localPosition = new Vector2(hpBar.transform.localPosition.x, (1 - hpPercent) * fullBarLength / 2);
        if(currentHealth <= 0)
        {
            if(statManager)
            {
                statManager.AddCoins(coinReward);
                statManager.AddPower(powerReward);
            }
            if(spawner)
            {
                spawner.CountOneKill();
            }
            Destroy(gameObject);
        }
    }

    // Set up the enemy movement interpolation points
    void SetUpMovementPointsList()
    {
        
        movementPoints = new List<MovementPoint>();
        movementPoints = spawner.MovementPoints;
        
        movementPoints.Sort((a,b) => a.PointNumber.CompareTo(b.PointNumber)); // The enemy moves to the points in the list in a specified order
    }

    // Moves the enemy every frame until it reaches its destination
    IEnumerator MoveEnemy()
    {
        transform.position = movementPoints[0].transform.position;
        for (int i = 1; i < movementPoints.Count; i++)
        {
            Vector2 startPosition = movementPoints[i - 1].transform.position;
            Vector2 endPosition = movementPoints[i].transform.position;
            transform.right = endPosition - startPosition; // Makes sure the enemy always face toward the point it is moving towards
            float distanceBetweenPoints = Vector2.Distance(startPosition, endPosition);
            float interpolationPercent = 0;
            while(interpolationPercent < 1)
            {
                interpolationPercent += Time.deltaTime / distanceBetweenPoints * moveSpeed;
                transform.position = Vector2.Lerp(startPosition, endPosition, interpolationPercent);
                yield return new WaitForEndOfFrame(); // Don't continue the loop until a frame has passed
            }
        }
        if(statManager)
        {
            statManager.AddHealth(-attackPower); // Player loses some health after enemy reaches the player's castle
        }
        if (spawner)
        {
            spawner.CountOneKill();
        }
        Destroy(gameObject); // Destroy enemy when enemy reaches the castle/destination
    }


}
