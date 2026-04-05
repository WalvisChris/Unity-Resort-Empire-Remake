using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    // --- SCOPE ---
    // Camera movement
    // Cursor grid collision

    [Header("Movement")]
    public float movementSpeed = 10f;
    public float mouseRange = 100f;
    public LayerMask gridLayer;

    public static PlayerCamera Instance;

    void Awake()
    {
        if (Instance == null || Instance != this) Instance = this;
    }

    void Update()
    {
        MoveCamera();
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

    public Vector2Int CursorCollisionToTile(Vector2Int fallback)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, mouseRange, gridLayer))
        {
            Vector2Int tilePosition = new Vector2Int(Mathf.FloorToInt(hit.point.x), Mathf.FloorToInt(hit.point.z));
            return tilePosition;
        }
        return fallback; // TODO: get rid of fallback
    }

    public GameObject CursorCollisionToObjectWithLayer(LayerMask layer)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, mouseRange, layer))
        {
            return hit.collider.gameObject;
        }
        return null;
    }
}
