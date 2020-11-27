using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LedScript : MonoBehaviour
{
    public Color disableColor;
    public Color enableColor;
    public bool enable;

    public Image img;

    public string enableString;
    public string disableString;

    private SimpleTooltip simpleTooltip;

    private void Start()
    {
        if (GetComponent<SimpleTooltip>() != null)
        {
            simpleTooltip = GetComponent<SimpleTooltip>();
        }

        if (simpleTooltip != null)
        {
            if (enable)
                simpleTooltip.infoLeft = enableString;
            else
                simpleTooltip.infoLeft = disableString;
        }
    }

    public void EnableLed()
    {
        if (!enable)
        {
            enable = true;
            img.color = enableColor;
        }
        if (simpleTooltip != null)
        {
            simpleTooltip.infoLeft = enableString;
        }
    }

    public void DisableLed()
    {
        if (enable)
        {
            enable = false;
            img.color = disableColor;
        }
        if (simpleTooltip != null)
        {
            simpleTooltip.infoLeft = disableString;
        }
    }
}
