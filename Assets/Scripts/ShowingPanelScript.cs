using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShowingPanelScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform colliderTransform;
    public GameObject content;
    public ShowingEffect effect;
    public SlideEffectDirection SlideDirection;
    public bool show = false;
    public bool showedLock = false;
    [Range(1, 10)]
    public float speed;

    private bool gotCG;
    public CanvasGroup canvasGroup;
    private Vector3 insideTransform;
    private Vector3 outsideTransform;
    private float contentHeight;
    private float contentWidth;

    private void Start()
    {
        canvasGroup = content.GetComponent<CanvasGroup>();
        insideTransform = content.transform.localPosition;
        contentHeight = content.GetComponent<RectTransform>().rect.height;
        contentWidth = content.GetComponent<RectTransform>().rect.width;

        canvasGroup.alpha = 0;
    }

    private void Update()
    {          
        
        // fade effect
        if (effect == ShowingEffect.fade || effect == ShowingEffect.both)
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, (show || showedLock) ? 1 : 0, Time.deltaTime * speed);
            } else
            {
                print("Content must have a CanvasGroup element");
            }
        }

        // slider effect
        if (effect == ShowingEffect.slide || effect == ShowingEffect.both)
        {
            // Up Case
            if (SlideDirection == SlideEffectDirection.up)
            {
                outsideTransform = new Vector3(insideTransform.x, insideTransform.y - contentHeight, insideTransform.z);
                content.transform.localPosition = Vector3.Lerp(content.transform.localPosition, (show || showedLock) ? insideTransform : outsideTransform, Time.deltaTime * speed);
            }
            else if (SlideDirection == SlideEffectDirection.left)
            {
                outsideTransform = new Vector3(insideTransform.x+contentWidth, insideTransform.y, insideTransform.z);
                content.transform.localPosition = Vector3.Lerp(content.transform.localPosition, (show || showedLock) ? insideTransform : outsideTransform, Time.deltaTime * speed);
            }

        }

    }

    public void ChangeLockState()
    {
        showedLock = !showedLock;
    }

    public enum SlideEffectDirection
    {
        up,
        down,
        left,
        right,
    }
    public enum ShowingEffect
    {
        fade,
        slide,
        both,
    }

    public void OnPointerEnter(PointerEventData pointerEvent)
    {
        if (pointerEvent.pointerCurrentRaycast.gameObject == this.gameObject)
        {
            // Debug.Log("Mouse Over: " + pointerEvent.pointerCurrentRaycast.gameObject.name);
            show = true;
        }
    }

    public void OnPointerExit(PointerEventData pointerEvent)
    {
        show = false;
    }
}
