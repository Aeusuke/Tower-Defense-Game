using UnityEngine;

// Represents a bullet shot out by a tower
public class Bullet : MonoBehaviour
{
    // References
    [SerializeField] ParticleSystem hitParticle;
    // Variables
    int damage;
    float range; // Distance the bullet travels before it destroys itself
    float speed;
    bool destroyOnCollision; // Whether the bullets destroys when it hit an enemy
    bool piercesDefense; // Whether the bullets ignores enemy defense stats
    Vector2 initialPosition;
    bool propertiesSet = false; // Whether the bullet's properties have been set

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {       
        if(propertiesSet) // Makes sure that the bullet only updates after its properties have been set by its corresponding tower
        {

            transform.Translate(speed * Time.deltaTime, 0, 0);
            float distanceTravelled = Vector2.Distance(initialPosition, transform.position);
            if (distanceTravelled > range)
            {
                Destroy(gameObject);
            }
        }        
    }

    // Initializes the fields according to the passed in parameters
    public void SetProperties(int damage, float range, float speed, bool destroyOnCollision, bool piercesDefense)
    {
        this.damage = damage;
        this.range = range;
        this.speed = speed;
        this.destroyOnCollision = destroyOnCollision;
        this.piercesDefense = piercesDefense;
        propertiesSet = true;

    }

    // Activates when the bullet collides with another object
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() && propertiesSet) // Only respond to collision when bullet collides with an enemy and its field are initialized
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            
            enemy.DamageEnemy(damage, piercesDefense);
            Instantiate(hitParticle.gameObject, transform.position, Quaternion.identity);
            if(destroyOnCollision)
            {
                Destroy(gameObject);
            }
        }
        
    }
}
