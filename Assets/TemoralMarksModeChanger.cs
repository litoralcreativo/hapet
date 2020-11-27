using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemoralMarksModeChanger : MonoBehaviour
{
    public ElementPlacingScript elementPlacing;
    public ShowingPanelScript leftPanel;
    public ShowingPanelScript rightPanel;


    public void ChangeState()
    {
        if (elementPlacing.active)
        {
            leftPanel.showedLock = true;
            rightPanel.showedLock = true;
        }

        if (!elementPlacing.active)
        {
            leftPanel.showedLock = false;
            rightPanel.showedLock = false;
        }
    }
}
