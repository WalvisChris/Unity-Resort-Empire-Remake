using UnityEngine;

public class Tile
{
    public bool isOccupied;
    public bool isWalkable;

    public Tile(bool isOccupied, bool isWalkable)
    {
        this.isOccupied = isOccupied;
        this.isWalkable = isWalkable;
    }
}
