using UnityEngine;
using UnityEngine.UI;
using TMPro;

enum ResourceType
{
    Coin,
    Power,
    Health
}

// Represents the trading resource UI
public class TradePanel : MonoBehaviour
{
    // References
    PlayerResourceManager statManager;
    ShopManager shopManager;
    Tooltip tooltip;
    [SerializeField] ResourceType giveType; // Type of resource you need to give for trading
    [SerializeField] ResourceType getType; // Type of resource you get from trading
    [SerializeField] TextMeshProUGUI giveText; // Text which displays the amount of resource you need to give for the trade
    [SerializeField] TextMeshProUGUI getText;
    [SerializeField] int giveAmount; // The amount of resource you need to give for trading
    [SerializeField] int getAmount;
    [SerializeField] Image giveResourceImage; // Image of the resource you need to give for trading
    [SerializeField] Image getResourceImage;
    [SerializeField] Sprite coinImage;
    [SerializeField] Sprite powerImage;
    [SerializeField] Sprite healthImage;
    [SerializeField] Button tradeButton; // Button for trading resources
    [SerializeField] Button imageButton; // Button for showing the tooltips

    // Start is called before the first frame update
    void Start()
    {
        InitializeTradePanel();
    }

    // Update is called once per frame
    void Update()
    {
        SetButtonState();
    }

    // Update the tooltip if the user clicks on the get resource image
    void HandleTooltips()
    {
        if (tooltip) tooltip.UpdateTooltip(GetTooltipText()); 

    }

    // Returns the text content of the tooltip
    string GetTooltipText()
    {
        string tooltipText = "Give " + giveAmount + " " + giveType + " for " + getAmount + " " + getType + ".";

        return tooltipText;
    }

    // Initialize the trade panel references and text
    void InitializeTradePanel()
    {
        statManager = FindObjectOfType<PlayerResourceManager>();
        shopManager = FindObjectOfType<ShopManager>();
        tooltip = FindObjectOfType<Tooltip>();
        tradeButton.onClick.AddListener(ProcessTradeClick);
        imageButton.onClick.AddListener(HandleTooltips);
        tradeButton.interactable = true;
        
        if (giveType == ResourceType.Coin) giveResourceImage.sprite = coinImage;
        
        else if (giveType == ResourceType.Power) giveResourceImage.sprite = powerImage;
        
        else if (giveType == ResourceType.Health) giveResourceImage.sprite = healthImage;
        
        giveText.text = giveAmount.ToString();

        if (getType == ResourceType.Coin) getResourceImage.sprite = coinImage;
        
        else if (getType == ResourceType.Power) getResourceImage.sprite = powerImage;
        
        else if (getType == ResourceType.Health) getResourceImage.sprite = healthImage;
        
        getText.text = getAmount.ToString();
    }

    /* 
     * Make the buy button clickable or not depending on certain conditions. Button is clickable only when the player
     * can afford the trade and is not currently holding a tower to place.
    */
    void SetButtonState()
    {
        if(shopManager && shopManager.SelectedTower)
        {
            tradeButton.interactable = false;
            return;
        }
        if(giveType == ResourceType.Coin && statManager && statManager.CurrentCoins >= giveAmount)
        {
            tradeButton.interactable = true;
        } else if(giveType == ResourceType.Power && statManager && statManager.CurrentPower >= giveAmount)
        {
            tradeButton.interactable = true;
        } else if(giveType == ResourceType.Health && statManager && statManager.CurrentHealth >= giveAmount)
        {
            tradeButton.interactable = true;
        } else
        {
            tradeButton.interactable = false;
        }

    }

    // Runs when the trade button is pressed
    void ProcessTradeClick()
    {
        if(giveType == ResourceType.Coin && statManager) statManager.AddCoins(-giveAmount);
        else if(giveType == ResourceType.Power && statManager) statManager.AddPower(-giveAmount);
        else if(giveType != ResourceType.Health && statManager) statManager.AddHealth(-giveAmount);
        
        if(getType == ResourceType.Coin && statManager) statManager.AddCoins(getAmount);
        else if(getType == ResourceType.Power && statManager) statManager.AddPower(getAmount);
        else if(getType == ResourceType.Health && statManager) statManager.AddHealth(getAmount);
        
    }
}
