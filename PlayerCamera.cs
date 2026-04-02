using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCamera : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 10f;
    public float mouseRange = 100f;
    public LayerMask floorLayer;


    [Header("BuildingTypes")]
    public BuildingType tileBuilding;
    public BuildingType houseBuilding;
    public BuildingType icecreamBuilding;

    [Header("Ghost")]
    public Material ghostMaterial;
    private BuildingType selectedBuilding;
    private GameObject ghostInstance;
    private GhostIndicator ghostScript;

    void Update()
    { 
        if (Input.GetMouseButtonDown(1)) {
            if (ghostInstance == null) {
                CreateGhost();
            } else
            {
                DestroyGhost();
            }
        }

        if (Input.GetMouseButtonDown(0) && ghostInstance != null)
        {
            if (ghostScript != null)
            {
                Vector2Int currentTile = ghostScript.currentTile;

                if (!ghostScript.isTileOccupied(ghostScript.currentTile))
                {
                    ghostScript.PlacePrefab(selectedBuilding, currentTile);
                }
            }
        }

        if (ghostInstance != null) MoveGhost();

        MoveCamera();
    }

    private void CreateGhost()
    {
        selectedBuilding = houseBuilding; // TODO: hardcoded for now
        ghostInstance = Instantiate(selectedBuilding.prefab);

        Renderer[] renderers = ghostInstance.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            Material[] mats = new Material[r.materials.Length];
            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = ghostMaterial;
            }
            r.materials = mats;
        }

        ghostScript = ghostInstance.AddComponent<GhostIndicator>();
    }

    private void DestroyGhost()
    {
        Destroy(ghostInstance);
        ghostInstance = null;
        selectedBuilding = null;
        ghostScript = null;
    }

    private void MoveGhost()
    {
        if (ghostScript == null) return;
        Vector2Int fallback = ghostScript.currentTile;
        ghostScript.MoveToTile(CursorCollisionToTile(fallback));
    }

    private Vector2Int CursorCollisionToTile(Vector2Int fallback)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, mouseRange, floorLayer)) {
            Vector2Int tilePosition = new Vector2Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
            return tilePosition;
        }
        return fallback;
    }

    private void MoveCamera()
    {
        // New Input System
        Vector3 move = Vector3.zero;
        if (Keyboard.current.wKey.isPressed) move += new Vector3(1f, 0f, 1f);
        if (Keyboard.current.sKey.isPressed) move += new Vector3(-1f, 0f, -1f);
        if (Keyboard.current.aKey.isPressed) move += new Vector3(-1f, 0f, 1f);
        if (Keyboard.current.dKey.isPressed) move += new Vector3(1f, 0f, -1f);
        transform.position += move.normalized * movementSpeed * Time.deltaTime;
    }
}
