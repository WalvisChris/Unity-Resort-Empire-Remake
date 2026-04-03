using UnityEngine;

public class Tile
{
    public bool isOccupied;
    public bool isWalkable;
    public bool isGrass;

    public Tile(bool isOccupied, bool isWalkable, bool isGrass)
    {
        this.isOccupied = isOccupied;
        this.isWalkable = isWalkable;
        this.isGrass = isGrass;
    }
}
