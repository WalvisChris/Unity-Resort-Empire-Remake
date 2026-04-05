using System.Collections.Generic;
using UnityEngine;

public class Structure : MonoBehaviour
{
    // --- SCOPE ---
    // stores building type values as placed prefabs/instances, so:
    // - footprint can be calculated and tiles can be freed on destroy
    // - price can be calculated and balance can be updated on destroy (TODO)

    public StructureType structureType;
    private Vector2Int currentTile;
    private List<Vector2Int> coveredTiles = new List<Vector2Int>();

    void Start()
    {
        if (structureType == null) return;
        currentTile = new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.z));
        for (int x = 0; x < structureType.size.x; x++)
        {
            for (int y = 0; y < structureType.size.y; y++)
            {
                Vector2Int tile = new Vector2Int(currentTile.x + x, currentTile.y + y);
                coveredTiles.Add(tile);
            }
        }
    }

    void OnDestroy()
    {
        if (structureType == null) return;
        
        // free claimed tiles
        foreach (Vector2Int tile in coveredTiles)
        {
            GridManager.Instance.freeTile(tile);
        }

        // give back half the money
        GameManager.instance.BalanceChange(structureType.price / 2);

        // update statistics if necessary
        switch (structureType.category)
        {
            case StructureType.BuildingCategory.Janitor:
                GameManager.instance.janitorCount--;
                break;
            case StructureType.BuildingCategory.Room:
                GameManager.instance.roomCount--;
                break;
            case StructureType.BuildingCategory.Plant:
                GameManager.instance.plantCount--;
                break;
            case StructureType.BuildingCategory.Facility:
                GameManager.instance.facilityCount--;
                break;
        }
        HUDManager.instance.UpdateStatistics();
    }
}
