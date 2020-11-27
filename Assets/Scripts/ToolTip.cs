using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{

    [SerializeField]
    private Camera uiCam;


    private Text toolTipText;
    private RectTransform backgroundRectTransform;

    private void Awake()
    {
        backgroundRectTransform = transform.Find("backgroundRectTransform").GetComponent<RectTransform>();
        toolTipText = transform.Find("toolTipText").GetComponent<Text>();
        ShowToolTip("random text for testing");
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCam, out localPoint);
        transform.localPosition = localPoint;

    }

    public void ShowToolTip(string toolTipString)
    {
        gameObject.SetActive(true);
        print("something");
        toolTipText.text = toolTipString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(toolTipText.preferredWidth + textPaddingSize * 2f, toolTipText.preferredHeight + textPaddingSize * 2f);
        backgroundRectTransform.sizeDelta = backgroundSize;
    }

    public void HideToolTip()
    {
        gameObject.SetActive(false);
    }
}
