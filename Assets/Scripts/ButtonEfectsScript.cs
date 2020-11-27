using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonEfectsScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool over;
    public float scaleFactor = 1.5f;
    public bool show;

    public void SetScaleToZero()
    {
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {

        if (over)
        {
            //print(over);
            transform.localScale = Vector3.Lerp(transform.localScale, show? Vector3.one * scaleFactor:Vector3.one, Time.deltaTime * 10);
        } else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, show ? Vector3.one : Vector3.zero, Time.deltaTime * 10);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
    }

    public void Show(bool show)
    {
        if (show)
        {
            show = true;
        }
        else
        {
            show = false;
        }
    }
}
