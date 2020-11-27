using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ToolTip toolTip;
    public string string1;
    public string string2;

    public bool justString1;


    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (justString1)
            toolTip.ShowToolTip(string1);
        else
            toolTip.ShowToolTip(string2);
    }


    public void OnPointerExit(PointerEventData pointerEventData)
    {
        toolTip.HideToolTip();
    }


}
