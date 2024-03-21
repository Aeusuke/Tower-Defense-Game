using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Represents a tower
public class Tower : MonoBehaviour
{
    // Constants
    const int powerChargePeriod = 5; // The number of attacks a particular tower makes before power is consumed 

    // After this level, the tower will no longer gain stats in range, bullet speed, or attacks per second.
    // Only bullet damage will be increased after the level limit when leveling up.
    // This prevents the bullet from travelling too quickly that the collision is undetectable and attacking too quickly
    // will overclock the computer. Having too long range is not particularly beneficial.
    const int speedRangeLevelLimit = 100; 

    // References
    [SerializeField] Bullet bullet;
    [SerializeField] TextMeshPro levelText; // Text for the current level of the tower
    [SerializeField] GameObject towerBody; // The sprite object of the tower
    PlayerResourceManager statManager;
    EnemySpawner enemySpawner;

    // Variables
    [SerializeField] bool isGenerator = false; // Whether the tower is a resource generator
    [SerializeField] int powerCost = 1; // The amount of power that is consumed every power consumption cycle
    [SerializeField] bool bulletDestroyOnCollision = true; // Whether the bullet gets destroyed when colliding with an enemy
    [SerializeField] bool piercesDefense = false; // Whether the bullet ignores the enemy defense stats
    [SerializeField] bool selfDestructsAfterAttack = false; // Whether the tower automatically destroys after making ab attack
    [SerializeField] float initialBulletSpeed = 5f; 
    [SerializeField] float initialBulletRange = 3f; // The distance the bullet will travel before it automatically gets destroyed and the enemy detection range at level 0
    [SerializeField] int initialMaxDamage = 5; // The maximum damage of the bullet ignoring enemy defense when this tower is at level 0
    [SerializeField] int initialMinDamage = 2; // The minimum damage of the bullet ignoring enemy defense when this tower is at level 0
    [SerializeField] int initialCoinGeneratedPerCycle = 0; // The number of coins that is generated every cycle when this tower is at level 0
    [SerializeField] int initialPowerGeneratredPerCycle = 0;
    [SerializeField] int initialHealthGeneratedPerCycle = 0;
    [SerializeField] float initialFirePerSecond = 1f; // The number of attacks this tower makes per second at level 0
    [SerializeField] int coinCost = 20; // Amount of coins required to buy this tower
    [SerializeField] int initialUpgradeCost = 10; // Amount of coins required to upgrade this tower at level 0
    [SerializeField] float bulletSpeedGrowth = 0.1f; // The amount at which the bullet speed of this tower increases for every increase in level
    [SerializeField] float bulletRangeGrowth = 0.1f;
    [SerializeField] float maxDamageGrowth = 0.5f;
    [SerializeField] float minDamageGrowth = 0.2f;
    [SerializeField] float firePerSecondGrowth = 0.01f;
    [SerializeField] float coinGeneratedPerCycleGrowth = 0;
    [SerializeField] float powerGeneratedPerCycleGrowth = 0;
    [SerializeField] float healthGeneratedPerCycleGrowth = 0;
    [SerializeField] int level = 0;
    bool isReloading = false; // Whether the tower is reloading its bullet
    bool enemyInRange = false; // Whether an enemy is within its bullet range
    bool toDestroy = false; // If true, the tower is going to destroy itself immediately
    float currentBulletRange; // The bullet range of the tower after taking account of its current level
    float currentBulletSpeed;
    int currentMaxDamage;
    int currentMinDamage;
    float currentFirePerSecond;
    int currentCoinGeneratedPerCycle;
    int currentPowerGeneratredPerCycle;
    int currentHealthGeneratedPerCycle;
    int currentUpgradeCost;
    int numAttacks = 0; // The number of attacks this tower have made since last power consumption cycle
    public int CurrentCoinGeneratedPerCycle { get { return currentCoinGeneratedPerCycle; }}
    
    public int CurrentPowerGeneratredPerCycle { get { return currentPowerGeneratredPerCycle; }}

    public int CurrentHealthGeneratedPerCycle { get { return currentHealthGeneratedPerCycle; }}

    public int CoinCost { get { return coinCost; }}

    public int SellPrice { get { return Mathf.FloorToInt(((2 * initialUpgradeCost + (initialUpgradeCost * 0.1f) * (level - 1)) * level / 2f + coinCost) / 2f); }}

    public int CurrentUpgradeCost { get { return currentUpgradeCost; }}

    public bool ToDestroy { get { return toDestroy; }}

    // Awake is called when this script object is initialized
    void Awake()
    {
        HandleLevelStats();
    }

    // Start is called before the first frame update
    void Start()
    {
        statManager = FindObjectOfType<PlayerResourceManager>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if(toDestroy) { return; }
        
        if(isGenerator) { return; } // Cannot attack if the tower is a resource generator
        AimAtEnemy();
        if(!isReloading && enemyInRange && statManager && statManager.CurrentPower >= powerCost)
        {
            isReloading = true; // Makes sure that the tower cannot make another attack until reloading is done
            numAttacks++;
            if(numAttacks >= powerChargePeriod) // Consumes power after certain number of attacks
            {
                numAttacks = 0;
                statManager.AddPower(-powerCost);
            }
            StartCoroutine(ShootAtEnemy());
        }
    }

    /* 
     * Get dictionary of tower stats for the tooltip purposes
     * isLeveled = true means that the stats take account of the current tower level
     * isLeveled = false means that the stats are base stats
    */
    public Dictionary<string, string> GetTooltipStats(bool isLeveled)
    {
        Dictionary<string, string> stats = new Dictionary<string,string>();
        float attackPerSecond, range, bulletSpeed;
        int minAttack, maxAttack, coinGen, powerGen, healthGen, upgradeCost;
        if(isLeveled)
        {
            attackPerSecond = currentFirePerSecond;
            range = currentBulletRange;
            bulletSpeed = currentBulletSpeed;
            minAttack = currentMinDamage;
            maxAttack = currentMaxDamage;
            coinGen = currentCoinGeneratedPerCycle;
            powerGen = currentPowerGeneratredPerCycle;
            healthGen = currentHealthGeneratedPerCycle;
            upgradeCost = currentUpgradeCost;
            
        } else
        {
            attackPerSecond = initialFirePerSecond;
            range = initialBulletRange;
            bulletSpeed = initialBulletSpeed;
            minAttack = initialMinDamage;
            maxAttack = initialMaxDamage;
            coinGen = initialCoinGeneratedPerCycle;
            powerGen = initialPowerGeneratredPerCycle;
            healthGen = initialHealthGeneratedPerCycle;
            upgradeCost = initialUpgradeCost;
        }

        stats.Add("Name", gameObject.name);
        stats.Add("IsGen", isGenerator.ToString());
        stats.Add("PowerCost", powerCost.ToString());
        stats.Add("UpgradeCost", upgradeCost.ToString());
        stats.Add("SellPrice", SellPrice.ToString());
        stats.Add("Destroys", selfDestructsAfterAttack.ToString());
        stats.Add("PiercesDefense", piercesDefense.ToString());
        stats.Add("PiercesEnemy", (!bulletDestroyOnCollision).ToString()); 
        stats.Add("AttackRange", minAttack + "-" + maxAttack);
        stats.Add("AttackPerSecond", attackPerSecond.ToString());
        stats.Add("Range", range.ToString());
        stats.Add("BulletSpeed", bulletSpeed.ToString());
        stats.Add("CoinGen", coinGen.ToString());
        stats.Add("PowerGen", powerGen.ToString());
        stats.Add("HealthGen", healthGen.ToString());

        return stats;
    }

    // Level up the tower and improve the stats of the tower
    public void LevelUp()
    {
        level++;
        HandleLevelStats();
    }

    // Updates the level text and update the stats of the tower according to its current level
    void HandleLevelStats()
    {
        levelText.text = "Lv. " + level;
        int adjustedLevel = level;
        if(level > speedRangeLevelLimit)
        {
            adjustedLevel = speedRangeLevelLimit;
        }
        currentBulletRange = initialBulletRange + adjustedLevel * bulletRangeGrowth;
        currentBulletSpeed = initialBulletSpeed + adjustedLevel * bulletSpeedGrowth;
        currentFirePerSecond = initialFirePerSecond + adjustedLevel * firePerSecondGrowth;
        currentMinDamage = initialMinDamage + (int)(level * minDamageGrowth);
        currentMaxDamage = initialMaxDamage + (int)(level * maxDamageGrowth);
        currentCoinGeneratedPerCycle = initialCoinGeneratedPerCycle + (int)(level * coinGeneratedPerCycleGrowth);
        currentPowerGeneratredPerCycle = initialPowerGeneratredPerCycle + (int)(level * powerGeneratedPerCycleGrowth);
        currentHealthGeneratedPerCycle = initialHealthGeneratedPerCycle + (int)(level * healthGeneratedPerCycleGrowth);
        currentUpgradeCost = Mathf.CeilToInt(initialUpgradeCost * (0.1f * level + 1));
    }

    // Creates a bullet for shooting at enemy
    IEnumerator ShootAtEnemy()
    {
        int damage = Random.Range(currentMinDamage, currentMaxDamage + 1);
        GameObject newBullet = Instantiate(bullet.gameObject, transform.position, towerBody.transform.rotation);
        newBullet.SetActive(true);
        newBullet.GetComponent<Bullet>().SetProperties(damage, currentBulletRange, currentBulletSpeed, bulletDestroyOnCollision, piercesDefense);
        if(selfDestructsAfterAttack)
        {
            toDestroy = true;
            
        }
        yield return new WaitForSeconds(1 / currentFirePerSecond); // Reloads bullet after certain amount of time
        isReloading = false; 
    }
    
    // Tower rotates toward the closing enemy if an enemy is within the tower's bullet range
    void AimAtEnemy()
    {
        Enemy[] enemiesOnScreen = enemySpawner.GetComponentsInChildren<Enemy>();
        float closestDistance = Mathf.Infinity;
        Enemy closestEnemy = null;
        foreach (Enemy enemy in enemiesOnScreen)
        {
            float distance = Vector2.Distance(enemy.transform.position, transform.position);
            if(distance < currentBulletRange && distance < closestDistance)
            {
                
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }
        if (closestEnemy)
        {
            towerBody.transform.right = closestEnemy.transform.position - transform.position;
            enemyInRange = true;
        } else
        {
            enemyInRange = false;
        }
       
    }

}
