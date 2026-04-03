using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    public static int ROWS_MAX = 15;
    public static int COLS_MAX = 15;

    private Tile[,] _grid;

    private void Awake()
    {
        if (Instance == null || Instance != this) Instance = this;
        InitialiseGrid();
    }

    private void InitialiseGrid()
    {
        // harcoded lobby tiles
        Vector2Int[] lobbyTiles = new Vector2Int[]
        {
            new Vector2Int(10, 0),
            new Vector2Int(11, 0),
            new Vector2Int(10, 1),
            new Vector2Int(11, 1),
            new Vector2Int(10, 2),
            new Vector2Int(11, 2),
            new Vector2Int(9, 3),
            new Vector2Int(10, 3),
            new Vector2Int(11, 3)
        };

        // create grid
        _grid = new Tile[ROWS_MAX, COLS_MAX];

        for (int x = 0; x < ROWS_MAX; x++)
        {
            for (int y = 0; y < COLS_MAX; y++)
            {
                bool occupied = lobbyTiles.Any(pos => pos.x == x && pos.y == y);
                _grid[x, y] = new Tile(occupied, false, false);
            }
        }
    }

    public bool isTileOccupied(Vector2Int position, bool excludeGrass = false)
    {
        return excludeGrass ? _grid[position.x, position.y].isOccupied && !_grid[position.x, position.y].isGrass : _grid[position.x, position.y].isOccupied;
    }

    public bool isTileOutsideGrid(Vector2Int position)
    {
        return position.x < 0 || position.x >= ROWS_MAX || position.y < 0 || position.y >= COLS_MAX;
    }

    public void claimTile(Vector2Int position, BuildingType b)
    {
        if (_grid[position.x, position.y].isOccupied) Debug.LogWarning($"Tile at {position} is already occupied!");
        _grid[position.x, position.y].isOccupied = true;
        _grid[position.x, position.y].isWalkable = b.isWalkable;
        _grid[position.x, position.y].isGrass = b.isGrass;
    }

    public void freeTile(Vector2Int position)
    {
        _grid[position.x, position.y].isOccupied = false;
    }
}
