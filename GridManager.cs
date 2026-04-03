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
        Initialise();
    }

    private void Initialise()
    {
        _grid = new Tile[ROWS_MAX, COLS_MAX];

        for (int x = 0; x < ROWS_MAX; x++)
        {
            for (int y = 0; y < COLS_MAX; y++)
            {
                _grid[x, y] = new Tile(false, false, false);
            }
        }
    }

    public bool isTileOccupied(Vector2Int position)
    {
        return _grid[position.x, position.y].isOccupied;
    }

    public void claimTile(Vector2Int position)
    {
        _grid[position.x, position.y].isOccupied = true;
    }

    public void freeTile(Vector2Int position)
    {
        _grid[position.x, position.y].isOccupied = false;
    }
}
