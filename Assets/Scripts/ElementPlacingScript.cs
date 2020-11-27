using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementPlacingScript : MonoBehaviour
{
    public Transform insidePH;
    public Transform outsidePH;
    public Transform normalPH;
    public Transform container;
    [Range (1, 10)]
    public float effectSpeed = 1;

    public List<ElementPlacingScript> otherPanels;

    public bool enable = true;
    public bool active = false;

    private void Update()
    {
        if (active)
            container.localPosition = Vector3.Lerp(container.localPosition, insidePH.localPosition, effectSpeed * Time.deltaTime);
        else if (enable)
            container.localPosition = Vector3.Lerp(container.localPosition, normalPH.localPosition, effectSpeed * Time.deltaTime);
        else
            container.localPosition = Vector3.Lerp(container.localPosition, outsidePH.localPosition, effectSpeed * Time.deltaTime);
    }

    public void EnableMenu()
    {
        if (!enable)
        {
            enable = true;
        }
        else
        {
            DisableMenu();
        }
    }

    public void DisableMenu()
    {
        if (enable)
        {
            enable = false;
        }
    }

    public void OpenMenu()
    {
        if (!active)
        {
            active = true;
            if (otherPanels != null)
            {
                foreach (ElementPlacingScript eps in otherPanels)
                {
                    eps.DisableMenu();
                }
            }
        }
        else
        {
            active = false;
            if (otherPanels != null)
            {
                foreach (ElementPlacingScript eps in otherPanels)
                {
                    eps.EnableMenu();
                }
            }
        }

    }
}
