using UnityEngine;

[CreateAssetMenu(fileName = "BuildingType", menuName = "Scriptable Objects/BuildingType")]
public class BuildingType : ScriptableObject
{
    public Vector2Int size;
    public string buildingName;
    public GameObject prefab;
    public bool canPlaceOnGrass;
    public bool isGrass;
    public bool isWalkable;
}
