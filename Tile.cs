using UnityEngine;

public class Tile
{
    // behaviour
    public bool isOccupied;
    public bool isWalkable;

    // types
    public bool isGrass;
    public bool isWater;
    public bool isInteractable;

    public Tile(bool isOccupied, bool isWalkable, bool isGrass, bool isWater, bool isInteractable)
    {
        this.isOccupied = isOccupied;
        this.isWalkable = isWalkable;
        this.isGrass = isGrass;
        this.isWater = isWater;
        this.isInteractable = isInteractable;
    }
}
