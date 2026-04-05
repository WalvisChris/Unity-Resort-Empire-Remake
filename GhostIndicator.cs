using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GhostIndicator : MonoBehaviour
{
    // --- SCOPE ---
    // Ghost creating
    // Ghost tile validation
    // Ghost movement
    // Prefab placing
    // Ghost deletion

    private List<Material> materials = new List<Material>();

    private Vector2Int currentTile;
    private List<Vector2Int> coveredTiles;

    public StructureType selectedStructureType;

    // --- BASIC ---

    void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                materials.Add(m);
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !isAreaOccupied() && !EventSystem.current.IsPointerOverGameObject()) PlacePrefab();
        if (Input.GetMouseButtonDown(1)) DestroyGhost();
        MoveGhost();
    }

    // --- GHOST LOGIC ---

    private void MoveGhost()
    {
        // caculate tile
        Vector2Int tilePositionV2 = PlayerCamera.Instance.CursorCollisionToTile(currentTile);
        Vector3 tilePositionV3 = TileToVector3(tilePositionV2);

        // set tile values
        currentTile = tilePositionV2;
        coveredTiles = new List<Vector2Int>();
        for (int x = 0; x < selectedStructureType.size.x; x++)
        {
            for (int y = 0; y < selectedStructureType.size.y; y++)
            {
                Vector2Int tile = new Vector2Int(currentTile.x + x, currentTile.y + y);
                coveredTiles.Add(tile);
            }
        }

        // out of bounds check
        foreach (Vector2Int tile in coveredTiles)
        {
            if (GridManager.Instance.isTileOutsideGrid(tile))
            {
                return;
            }
        }

        // color
        foreach (Material m in materials)
        {
            UnityEngine.Color c = isAreaOccupied() ? UnityEngine.Color.red : UnityEngine.Color.green;
            c.a = 0.7f;
            m.color = c;
        }

        // move ghost
        transform.position = tilePositionV3;
    }

    private void DestroyGhost()
    {
        Destroy(gameObject);
    }

    private void PlacePrefab()
    {
        if (GameManager.instance.balance < selectedStructureType.price) return;

        // instantiate prefab
        Vector3 tilePositionV3 = TileToVector3(currentTile);
        GameObject obj = Instantiate(selectedStructureType.prefab, tilePositionV3, Quaternion.identity);
        obj.GetComponent<Structure>().structureType = selectedStructureType;
        GameManager.instance.BalanceChange(-selectedStructureType.price);

        // claim all necessary tiles
        foreach (Vector2Int tile in coveredTiles)
        {
            GridManager.Instance.claimTile(tile, selectedStructureType);
        }

        // update statistics
        switch (selectedStructureType.category)
        {
            case StructureType.BuildingCategory.Janitor:
                GameManager.instance.janitorCount++;
                break;
            case StructureType.BuildingCategory.Room:
                GameManager.instance.roomCount++;
                break;
            case StructureType.BuildingCategory.Plant:
                GameManager.instance.plantCount++;
                break;
            case StructureType.BuildingCategory.Facility:
                GameManager.instance.facilityCount++;
                break;
        }
        HUDManager.instance.UpdateStatistics();
    }

    // --- UTILITIES ---

    public bool isTileOccupied(Vector2Int position)
    {
        return GridManager.Instance.isTileOccupied(position, selectedStructureType.canPlaceOnGrass);
    }

    private bool isAreaOccupied()
    {
        bool flag = false;
        foreach (Vector2Int tile in coveredTiles)
        {
            if (isTileOccupied(tile))
            {
                flag = true;
                break;
            }
        }
        return flag;
    }

    private Vector3 TileToVector3(Vector2Int tile)
    {
        return new Vector3(tile.x, 0f, tile.y);
    }
}
