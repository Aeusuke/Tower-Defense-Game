using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Manages selection/deselection tower tiles
public class TileManager : MonoBehaviour
{
    // References
    [SerializeField] ShopManager shopManager;
    // Variables
    TowerTile selectedTowerTile; // Tower tile which is currently being selected. Null means that no tower is currently selected.

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject() && selectedTowerTile)
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 selectedTilePosition = selectedTowerTile.transform.position;
            if (mouseWorldPos.x < selectedTilePosition.x - 0.5f || mouseWorldPos.x > selectedTilePosition.x + 0.5f ||
                mouseWorldPos.y < selectedTilePosition.y - 0.5f || mouseWorldPos.y > selectedTilePosition.y + 0.5f)
            {
                UnselectTower(selectedTowerTile);
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            shopManager.SelectedTower = null;
            UnselectTower(selectedTowerTile);
        }
    }

    // Checks whether the passed in tower tile is currently selected
    public bool IsSelected(TowerTile towerTile)
    {
        return towerTile == selectedTowerTile;
    }

    // Select the passed in tower tile. Ensures that only one tile can be selected at a time.
    public void SelectTower(TowerTile towerTile)
    {
        if(selectedTowerTile)
        {
            selectedTowerTile.SetDefaultColour();
        }
        selectedTowerTile = towerTile;
        if(selectedTowerTile)
        {
            selectedTowerTile.SetSelectedColour();
        }
    }

    // Unselects the passed in tower tile
    public void UnselectTower(TowerTile towerTile)
    {
        if(selectedTowerTile && towerTile == selectedTowerTile)
        {
            selectedTowerTile.SetDefaultColour();
            selectedTowerTile = null;
            
        }
       

    }
}
