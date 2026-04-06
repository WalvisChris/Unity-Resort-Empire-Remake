using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    // TODO:
    // - side pannel for building types
    // - UI pop ups for buttons

    public static HUDManager instance;

    [Header("UI Top")]
    public TextMeshProUGUI janitorText;
    public TextMeshProUGUI visitorText;
    public TextMeshProUGUI roomText;
    public TextMeshProUGUI plantText;
    public TextMeshProUGUI facilityText;

    [Header("UI Bottom")]
    public Image clockImage;
    public Image minutePointer;
    public Image hourPointer;

    public Sprite clock_day;
    public Sprite clock_night;

    public Image timeSpeedOne;
    public Image timeSpeedTwo;
    public Image timeSpeedThree;

    public TextMeshProUGUI balanceText;

    // values
    public float timeElapsed = 0f;
    public float timeMultiplier = 0.1f;

    // cursor support
    [Header("Cursor Indicators")]
    public GameObject destroyModePrefab;
    private GameObject destroyModeIndicator = null;

    void Awake()
    {
        if (instance == null || instance != this) instance = this;
    }

    void Update()
    {
        // daytime cycle
        timeElapsed += Time.deltaTime * timeMultiplier;

        float minuteAngle = timeElapsed * 12f % 360f;
        minutePointer.transform.rotation = Quaternion.Euler(0f, 0f, -minuteAngle);

        float hourAngle = 90f - (timeElapsed % 360f);
        hourPointer.transform.rotation = Quaternion.Euler(0f, 0f, hourAngle);

        float hour = timeElapsed / 30f;
        if (hour > 6f && hour < 18f)
        {
            clockImage.sprite = clock_day;
        }
        else
        {
            clockImage.sprite = clock_night;
        }

        // move cursor indicator with mouse
        if (destroyModeIndicator != null) destroyModeIndicator.transform.position = Input.mousePosition + new Vector3(40f, 0f, 40f);
    }

    public void Btn_TimeSpeed(float multiplier)
    {
        timeMultiplier = multiplier;
        timeSpeedOne.color = multiplier == 1f ? Color.gray : Color.white;
        timeSpeedTwo.color = multiplier == 2f ? Color.gray : Color.white;
        timeSpeedThree.color = multiplier == 3f ? Color.gray : Color.white;
    }

    public void Btn_BuildStructure(StructureType s)
    {
        GameManager.instance.CreateGhost(s);
    }

    public void Btn_Destroy()
    {
        Debug.Log("void Btn_Destroy() called");
        if (GameManager.instance.ToggleDestroyMode() && destroyModeIndicator == null)
        {
            Debug.Log("Destroy mode enabled, creating indicator");
            destroyModeIndicator = Instantiate(destroyModePrefab, Vector3.zero, Quaternion.identity, transform);
        } else if (destroyModeIndicator != null)
        {
            Debug.Log("Destroy mode disabled, destroying indicator");
            Destroy(destroyModeIndicator);
        }
    }

    public void UpdateStatistics()
    {
        janitorText.text = "JANITOR(S)\n" + GameManager.instance.janitorCount;
        visitorText.text = "VISITOR(S)\n" + GameManager.instance.visitorCount;
        roomText.text = "ROOM(S)\n" + GameManager.instance.roomCount;
        plantText.text = "PLANT(S)\n" + GameManager.instance.plantCount;
        facilityText.text = "FACILITY(S)\n" + GameManager.instance.facilityCount;
    }
}
