using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPanelScript : MonoBehaviour
{
    public GameObject grid;

    public void SetAllValues()
    {
        
        foreach (GridInfoPanelScript panel in transform.GetComponentsInChildren<GridInfoPanelScript>())
        {
            panel.GetValues();
        }
    }
}
