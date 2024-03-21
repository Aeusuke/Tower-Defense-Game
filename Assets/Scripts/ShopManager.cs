using UnityEngine;

// Manages the shop UI
public class ShopManager : MonoBehaviour
{
    // References
    [SerializeField] Canvas frontCanvas; // The main part of the shop UI
    [SerializeField] Canvas backCanvas; // The background of the shop UI

    // Variables  
    Tower selectedTower = null; // Tower which is currently selected. Null means that no towers are currently selected.
    bool showUI = true; // Whether the shop UI is present on screen
    public Tower SelectedTower
    {
        get { return selectedTower; }
        set { selectedTower = value; }
    }

    // Update is called once per frame
    void Update()
    {
        // Show/hide shop UI whenever H is pressed
        if(Input.GetKeyDown(KeyCode.H))
        {
            if(showUI)
            {
                showUI = false;
                frontCanvas.enabled = false;
                backCanvas.enabled = false;
            } else
            {
                showUI = true;
                frontCanvas.enabled = true;
                backCanvas.enabled = true;
            }
        }
    }

   



}
