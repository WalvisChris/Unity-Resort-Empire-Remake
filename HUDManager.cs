using Unity.VisualScripting;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public void OnClick(BuildingType b)
    {
        PlayerCamera.Instance.CreateGhost(b);
    }
}
