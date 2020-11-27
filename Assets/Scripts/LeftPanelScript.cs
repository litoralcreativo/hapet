using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftPanelScript : MonoBehaviour
{
    
    public bool inside;
    [ShowOnly] private float speed = 8;
    public RectTransform content;
    public RectTransform insideRT;
    public RectTransform outsideRT;

    private void Start()
    {
        content.position = outsideRT.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (inside)
        {
            content.localPosition = Vector3.Lerp(content.localPosition, insideRT.localPosition, Time.deltaTime * speed);
        }
        else
        {
            content.localPosition = Vector3.Lerp(content.localPosition, outsideRT.localPosition, Time.deltaTime * speed);
        }
    }
}
