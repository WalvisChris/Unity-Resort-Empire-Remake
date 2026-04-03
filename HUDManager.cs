using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public Material ghostMaterial;
    private GameObject ghostObj;
    public void OnClick(BuildingType b)
    {
        if (ghostObj != null) return;

        // object
        ghostObj = Instantiate(b.prefab, Vector3.zero, Quaternion.identity);

        // ghost material
        Renderer[] renderers = ghostObj.GetComponentsInChildren<Renderer>();
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
        GhostIndicator ghostScript = ghostObj.AddComponent<GhostIndicator>();
        ghostScript.selectedBuildingType = b;
    }
}
