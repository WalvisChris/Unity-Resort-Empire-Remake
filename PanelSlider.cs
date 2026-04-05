using System.Collections.Generic;
using UnityEngine;

public class PanelSlider : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 opened;
    private Vector2 closed;
    private bool isOpen = false;

    private static List<PanelSlider> allPanels = new List<PanelSlider>();

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        opened = new Vector2(140f, rectTransform.anchoredPosition.y);
        closed = new Vector2(-140f, rectTransform.anchoredPosition.y);
        allPanels.Add(this);
    }

    void Update()
    {
        Vector2 target = isOpen ? opened : closed;
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, target, Time.deltaTime * 10f);
    }

    public void Toggle()
    {
        if (isOpen)
        {
            Close();
        } else
        {
            foreach (var p in allPanels)
            {
                if (p != this)
                    p.Close();
            }
            isOpen = true;
        }
    }

    public void Close() => isOpen = false;
}
