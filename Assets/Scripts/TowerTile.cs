using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Manages a specific green 1x1 tile on the map in which towers can be placed
public class TowerTile : MonoBehaviour
{
    // References
    ShopManager shopManager;
    [SerializeField] Color hoverColour = Color.yellow; // Tile colour when mouse hovers this tile and the tower is currently on cursor
    [SerializeField] Color towerSelectedColour = Color.blue; // Tile colour when this tile is selected
    [SerializeField] Color defaultColour = Color.white;
    [SerializeField] Transform towerStorage; // Location where the built towers are stored
    TileManager tileManager;
    PlayerResourceManager statManager;
    Tooltip tooltip;
    SpriteRenderer spriteRenderer;
    // Variables
    Tower placedTower = null; // The tower that is placed on this tile. Null means that this tile is vacant.
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeTowerTile();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessSell();
        ProcessUpgrade();
        if(placedTower && placedTower.ToDestroy)
        {
            DestroyTower();
        }
        
    }

    // Called on the first frame
    void InitializeTowerTile()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        statManager = FindObjectOfType<PlayerResourceManager>();
        shopManager = FindObjectOfType<ShopManager>();
        tooltip = FindObjectOfType<Tooltip>();
        tileManager = GetComponentInParent<TileManager>();
        towerStorage = GameObject.FindGameObjectWithTag("TowerStorage").transform;
    }

    // Handle selling this tower
    void ProcessSell()
    {
        if (statManager && placedTower && tileManager.IsSelected(this))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                statManager.AddCoins(placedTower.SellPrice);
                DestroyTower();
                if(tooltip)
                {
                    tooltip.UpdateTooltip("");
                }
            }
        }
    }

    // Destroy the tower on this tile and unselect it if a tower exists on this tile
    void DestroyTower()
    {
        if (!placedTower) return;
        Destroy(placedTower.gameObject);
        placedTower = null;

        tileManager.UnselectTower(this);
    }

    // Handle upgrading this tower 
    void ProcessUpgrade()
    {
        if (statManager && placedTower && tileManager.IsSelected(this))
        {
            if (Input.GetKeyDown(KeyCode.U) && statManager.CurrentCoins >= placedTower.CurrentUpgradeCost)
            {
                statManager.AddCoins(-placedTower.CurrentUpgradeCost);
                placedTower.LevelUp();
                if (tooltip)
                {
                    tooltip.UpdateTooltip(GetTooltipText());
                }

            }
        }
    }

    // Called whenever the mouse cursor is hovering over this tile. Highlights this tile if the currently selected tower from shop can be placed here.
    void OnMouseOver()
    {
        
        if (shopManager && !EventSystem.current.IsPointerOverGameObject())
        {
            Tower selectedTower = shopManager.SelectedTower;
            if(selectedTower && !placedTower)
            {
                spriteRenderer.material.color = hoverColour;
            } 
            
        }
    }

    // Called when the mouse cursor leaves this tile.
    // Set the tile to its default colour if mouse cursor is no longer hovering this tile and this tile is not currently selected.
    void OnMouseExit()
    {
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (true || mouseWorldPos.x < transform.position.x - 0.5f || mouseWorldPos.x > transform.position.x + 0.5f ||
           mouseWorldPos.y < transform.position.y - 0.5f || mouseWorldPos.y > transform.position.y + 0.5f)
        {
            if(!tileManager.IsSelected(this))
                SetDefaultColour();
        }

    }

    // Returns the text content of the tooltip
    string GetTooltipText()
    {
        Dictionary<string, string> towerStats = placedTower.GetTooltipStats(true);
        string tooltipText;
        if (towerStats["IsGen"] == "True")
        {
            tooltipText = "Selected Tower" + "\n" + "Power Cost: " + towerStats["PowerCost"] + "\n"
                           + "Upgrade Cost: " + towerStats["UpgradeCost"] + "\n" + "Sell Price: " + towerStats["SellPrice"] + "\n"
                           + "Coins Per Cycle: " + towerStats["CoinGen"] + "\n" + "Power Per Cycle: " + towerStats["PowerGen"] + "\n"
                           + "Health Per Cycle: " + towerStats["HealthGen"] + "\n";
        }
        else
        {
            string destroyText = towerStats["Destroys"] == "True" ? "Tower destroys itself after attacking\n" : "";
            string pierceDefenseText = towerStats["PiercesDefense"] == "True" ? "Bullet ignores enemy defense stat\n" : "";
            string pierceEnemyText = towerStats["PiercesEnemy"] == "True" ? "Bullet pierces through enemy\n" : "";
            tooltipText = "Selected Tower" + "\n" + "Power Cost: " + towerStats["PowerCost"] + "\n"
                           + "Upgrade Cost: " + towerStats["UpgradeCost"] + "\n" + "Sell Price: " + towerStats["SellPrice"] + "\n"
                           + "Attack: " + towerStats["AttackRange"] + "\n" + "Attack Per Second: " + towerStats["AttackPerSecond"] + "\n"
                           + "Range: " + towerStats["Range"] + "\n" + "Bullet Speed: " + towerStats["BulletSpeed"] + "\n"
                           + "Special Effects: " + "\n" + destroyText + pierceDefenseText + pierceEnemyText;
        }

        return tooltipText;
    }

    // Set the colour of this tile to the selected colour
    public void SetSelectedColour()
    {
        spriteRenderer.material.color = towerSelectedColour;
        
    }

    // Set the colour of this tile to its default colour
    public void SetDefaultColour()
    {
        spriteRenderer.material.color = defaultColour;
    }

    // Called when the user clicks on this tile
    void OnMouseDown()
    {
        if (towerStorage && shopManager && statManager && !EventSystem.current.IsPointerOverGameObject())
        {
            if (placedTower) // Select this tile if a tower exists on this tile
            {

                tileManager.SelectTower(this);
                if (tooltip)
                {
                    tooltip.UpdateTooltip(GetTooltipText());
                }

            }

            Tower tower = shopManager.SelectedTower;
            shopManager.SelectedTower = null;
            // Place the tower on tile when the tile is vacant, a tower is on the mouse cursor, and the user can afford the tower.
            if (tower && !placedTower && statManager.CurrentCoins >= tower.CoinCost) 
            {

                placedTower = Instantiate(tower, transform.position, Quaternion.identity, towerStorage);
                statManager.AddCoins(-tower.CoinCost);
                tileManager.SelectTower(this);
                if (tooltip)
                {
                    tooltip.UpdateTooltip(GetTooltipText());
                }
            }

        }
        
    }
}
