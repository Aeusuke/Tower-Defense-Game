using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Represents the UI for buying the corresponding tower
public class BuyTowerPanel : MonoBehaviour
{
    // References
    [SerializeField] Image towerImage; // UI image of the tower
    [SerializeField] Tower realTower; // The tower associated with this panel
    [SerializeField] TextMeshProUGUI costText; // The text which represents of the cost of this tower
    ShopManager shopManager;
    PlayerResourceManager statManager;
    [SerializeField] Button buyButton; // Buy Button
    [SerializeField] Button imageButton; // Button for showing the tooltips
    Tooltip tooltip;
    public Tower RealTower { get { return realTower; }}


    // Start is called before the first frame update
    void Start()
    {
        InitializeBuyPanel();
    }


    // Update is called once per frame
    void Update()
    {
        if(statManager)
        {
            if(!shopManager.SelectedTower) // Show the tower image when the player is not currently holding a tower to place
            {
                towerImage.gameObject.SetActive(true);
            }
            SetButtonState();
        }
        
            
       
    }

    // Updates the tooltip if the player clicks on this tower image UI
    void HandleTooltips()
    {
        if(tooltip) { tooltip.UpdateTooltip(GetTooltipText()); }
        
    }

    // Returns the text content of the tooltip
    string GetTooltipText()
    {
        Dictionary<string, string> towerStats = realTower.GetTooltipStats(false);
        string tooltipText;
        if(towerStats["IsGen"] == "True") // Checks if the tower associated with this panel is a resource generator
        {
            tooltipText = towerStats["Name"] + "\n" + "Power Cost: " + towerStats["PowerCost"] + "\n"
                           + "Coins Per Cycle: " + towerStats["CoinGen"] + "\n" + "Power Per Cycle: " + towerStats["PowerGen"] + "\n" 
                           + "Health Per Cycle: " + towerStats["HealthGen"] + "\n";
        } else
        {
            string destroyText = towerStats["Destroys"] == "True" ? "Tower destroys itself after attacking\n" : "";
            string pierceDefenseText = towerStats["PiercesDefense"] == "True" ? "Bullet ignores enemy defense stat\n" : "";
            string pierceEnemyText = towerStats["PiercesEnemy"] == "True" ? "Bullet pierces through enemy\n" : "";
            tooltipText = towerStats["Name"] + "\n" + "Power Cost: " + towerStats["PowerCost"] + "\n"
                           + "Attack: " + towerStats["AttackRange"] + "\n" + "Attack Per Second: " + towerStats["AttackPerSecond"] + "\n" 
                           + "Range: " + towerStats["Range"] + "\n" + "Bullet Speed: " + towerStats["BulletSpeed"] + "\n"
                           + "Special Effects: " + "\n" + destroyText + pierceDefenseText + pierceEnemyText;
        }
        
        return tooltipText;
    }

    // Runs on the first frame update
    void InitializeBuyPanel()
    {
        shopManager = FindObjectOfType<ShopManager>();
        statManager = FindObjectOfType<PlayerResourceManager>();
        tooltip = FindObjectOfType<Tooltip>();
        buyButton.onClick.AddListener(ProcessBuyClick);
        imageButton.onClick.AddListener(HandleTooltips); 
        towerImage.gameObject.SetActive(true);
        costText.text = realTower.CoinCost.ToString();
    }

    /* 
     * Make the buy button clickable or not depending on certain conditions. Button is clickable only when the player
     * can afford the tower and is not currently holding a tower to place.
    */
    void SetButtonState()
    {
        if(statManager.CurrentCoins >= realTower.CoinCost && !shopManager.SelectedTower)
        {
            buyButton.interactable = true;
        } else
        {
            buyButton.interactable = false;
        }
    }

    // Runs when the buy button is pressed
    void ProcessBuyClick()
    {
        shopManager.SelectedTower = realTower;
        towerImage.gameObject.SetActive(false);
        
    }

}
