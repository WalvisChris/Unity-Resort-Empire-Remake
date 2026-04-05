using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // --- SCOPE ---
    // game progression
    // - quests
    // - day/night cycle
    // - popularity
    // - balance
    // ghost spawning (remove from HUDManager)
    // AudioManager?

    public static GameManager instance;

    // statistics
    public int balance = 5000;
    public int janitorCount = 0;
    public int visitorCount = 0;
    public int roomCount = 0;
    public int plantCount = 0;
    public int facilityCount = 0;

    // ghost
    public Material ghostMaterial;
    private GameObject ghost;

    // cursor modes: idle - place - destroy
    private bool destroyMode = false;
    public LayerMask destroyableLayer;

    // debug
    public TextMeshProUGUI debugText;

    void Awake()
    {
        if (instance == null || instance != this) instance = this;
    }

    void Update()
    {
        if (destroyMode)
        {
            if (Input.GetMouseButtonDown(1)) destroyMode = false;
            if (Input.GetMouseButton(0))
            {
                GameObject obj = PlayerCamera.Instance.CursorCollisionToObjectWithLayer(destroyableLayer);

                if (obj != null) Destroy(obj);
            }
        }
        // debug
        debugText.text = destroyMode ? "Destroy Mode: ON" : "Destroy Mode: OFF";
    }

    public void CreateGhost(StructureType s)
    {
        if (ghost != null) return;

        // object
        ghost = Instantiate(s.prefab, Vector3.zero, Quaternion.identity);

        // ghost material
        Renderer[] renderers = ghost.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            Material[] mats = new Material[r.materials.Length];

            for (int i = 0; i < mats.Length; i++)
            {
                mats[i] = new Material(ghostMaterial);
            }

            r.materials = mats;
        }

        // script
        GhostIndicator ghostScript = ghost.AddComponent<GhostIndicator>();
        ghostScript.selectedStructureType= s;
    }

    public void ToggleDestroyMode() => destroyMode = !destroyMode;

    public void BalanceChange(int amount)
    {
        balance += amount;
        HUDManager.instance.balanceText.text = $"$  {balance}";
    }
}
