using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ReferencePointScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject detector;
    public  TextMeshProUGUI textName;
    public Color defaultColor;
    public Color selectedColor;

    public bool over;
    public bool selected = false;
    public bool canSelect = true;

    public string thisName;

    public ReferencePoint refPoint;

    private void Start()
    {
        refPoint = new ReferencePoint(thisName, transform.position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        detector.SetActive(true);
        over = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        detector.SetActive(false);
        over = false;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click");
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Middle)
        {
            Debug.Log("Middle click");
        }
        else if (pointerEventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right click");
            Select();
        }
    }

    public void Select()
    {
        if (canSelect)
        {
            selected = true;
            GetComponent<TextMeshProUGUI>().color = selectedColor;
            detector.GetComponentInChildren<TextMeshProUGUI>().color = selectedColor;
        }
    }

    public void Disselect()
    {
        selected = false;
        detector.GetComponentInChildren<TextMeshProUGUI>().color = defaultColor;
    }


}
