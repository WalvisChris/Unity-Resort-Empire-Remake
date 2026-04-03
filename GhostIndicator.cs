using JetBrains.Annotations;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GhostIndicator : MonoBehaviour
{
    private List<Material> _materials = new List<Material>();

    public Vector2Int currentTile;

    public BuildingType selectedBuilding;

    void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                _materials.Add(m);
            }
        }
    }

    public bool isTileOccupied(Vector2Int position)
    {
        return GridManager.Instance.isTileOccupied(position);
    }

    public void MoveToTile(Vector2Int position)
    {
        // color
        foreach (Material m in _materials)
        {
            Color c = (isTileOccupied(position)) ? Color.red : Color.green;
            c.a = 0.7f;
            m.color = c;
        }

        // position
        transform.position = TileToVector3(position);
        currentTile = position;
    }

    public void PlacePrefab(BuildingType building, Vector2Int tile)
    {
        Vector3 position = TileToVector3(tile);
        Instantiate(building.prefab, position, Quaternion.identity);
        GridManager.Instance.claimTile(currentTile); // TODO: footprint
    }

    private Vector3 TileToVector3(Vector2Int tile)
    {
        return new Vector3(tile.x, 0f, tile.y);
    }
}
