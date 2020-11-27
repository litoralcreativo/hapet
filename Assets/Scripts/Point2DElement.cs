using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;
using TMPro;


public class Point2DElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public Image[] arrows;
    public Point2D point2D;
    public VideoPlayer vplayer;
    public RectTransform bubble;
    public bool selected;
    public bool over;
    public RectTransform[] rectTransforms;
    public GameObject ToolsBtns;
    public TMP_Text tmpro;
    public Text timeText;
    [ShowOnly] public Vector2 screenPosition;

    [Header("Images")]
    public Image footEventTypeImg;
    public Image footImg;

    public List<Sprite> sprites;

    [Header("Colors")]
    public Color normalColor;
    public Color selectedColor;

    public Color leftColor;
    public Color rightColor;

    public double millis;
    public long frame;

    public GameObject panelOfText;

    private bool goToFrame = false;

    private Pointers2DScript parentOfParent;

    private void Start()
    {
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            transform.parent.GetChild(i).GetComponent<Point2DElement>().selected = false;
        }
        //selected = true;
        foreach (ButtonEfectsScript btns in ToolsBtns.GetComponentsInChildren<ButtonEfectsScript>())
        {
            btns.SetScaleToZero();
        }
        
        parentOfParent = transform.parent.transform.parent.GetComponent<Pointers2DScript>();
    }

    private void Update()
    {
        

        // when over
        if (over && !selected)
        {
            float rot = (float)(Time.deltaTime * 30);
            Vector3 rota = new Vector3(0, 0, rot);
            background.gameObject.SetActive(true);
            background.transform.Rotate(rota);

            bubble.localScale = Vector3.Lerp(bubble.localScale, Vector3.one * 4f, Time.deltaTime * 10);
            if (Input.GetMouseButtonDown(0))
            {
                for (int i = 0; i < transform.parent.childCount; i++)
                {
                    transform.parent.GetChild(i).GetComponent<Point2DElement>().selected = false;
                }
                transform.SetAsLastSibling();
                selected = !selected;
            }
            panelOfText.SetActive(true);
        }
        else if (selected)
        {
            background.gameObject.SetActive(false);
            bubble.localScale = Vector3.Lerp(bubble.localScale, Vector3.one * 3.5f, Time.deltaTime * 10);
            if (Input.GetMouseButtonDown(1))
            {
                selected = !selected;
            }
            panelOfText.SetActive(true);

        }
        else if (!selected && !over)
        {
            background.gameObject.SetActive(false);
            var rot = background.transform.rotation;
            rot.z = 0;
            background.transform.rotation = rot;    

            bubble.localScale = Vector3.Lerp(bubble.localScale, Vector3.one * 2f, Time.deltaTime * 10);
            panelOfText.SetActive(false);
        }

        // when selected
        if (selected)
        {
            if (goToFrame == true)
            {
                vplayer.frame = frame;
                goToFrame = false;
            }
            //background.color = Color.Lerp(background.color, selectedColor, Time.deltaTime * 8);
            foreach (ButtonEfectsScript btns in ToolsBtns.GetComponentsInChildren<ButtonEfectsScript>())
            {
                btns.show = true;
            }
        }
        else
        {
            goToFrame = true;
            //background.color = Color.Lerp(background.color, normalColor, Time.deltaTime * 8);
            foreach (ButtonEfectsScript btns in ToolsBtns.GetComponentsInChildren<ButtonEfectsScript>())
            {
                btns.show = false;
            }

        }
    }

    public void DeleteSelf()
    {
        int index = transform.GetSiblingIndex();
        transform.parent = null;
        parentOfParent.UpdatePoints();
        Destroy(gameObject);
    }

    public void CreateEvent(Foot f, SimpleEventType eT)
    {
        point2D = new Point2D(transform.position, eT, f);

        if (point2D.eventType == SimpleEventType.takeOff)
        {
            footEventTypeImg.sprite = sprites.Find(x => x.name == "Despegue");
            arrows[0].gameObject.SetActive(false);
            arrows[1].gameObject.SetActive(true);
        }
        else if (point2D.eventType == SimpleEventType.contact)
        {
            footEventTypeImg.sprite = sprites.Find(x => x.name == "ContactoInicial");
            arrows[0].gameObject.SetActive(true);
            arrows[1].gameObject.SetActive(false);
        }

        if (point2D.foot == Foot.left)
        {
            arrows[0].color = leftColor;
            arrows[1].color = leftColor;
        }
        else
        {
            arrows[0].color = rightColor;
            arrows[1].color = rightColor;
        }
    }

    public void ChangeFoot()
    {
        if (point2D.foot == Foot.left)
        {
            point2D.foot = Foot.right;
            footImg.sprite = sprites.Find(x => x.name == "FootR");
            arrows[0].color = rightColor;
            arrows[1].color = rightColor;

        }
        else if (point2D.foot == Foot.right)
        {
            point2D.foot = Foot.left;
            footImg.sprite = sprites.Find(x => x.name == "FootL");
            arrows[0].color = leftColor;
            arrows[1].color = leftColor;
        }
        print(transform.parent.transform.parent.GetComponent<Pointers2DScript>());
        parentOfParent = transform.parent.transform.parent.GetComponent<Pointers2DScript>();
        parentOfParent.UpdatePoints();
    }

    public void ChangeSelectState()
    {
        selected = !selected;
    }

    public void ChangeMilliseconds(double time, long fra = 0)
    {
        millis = time;
        frame = fra;
        point2D.millisSinceStart = time;
        //tmpro.text = time.ToString("#.###");
        timeText.text = time.ToString("#.###");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
    }

    public void ChangeFootEventType()
    {
        if (point2D.eventType == SimpleEventType.contact)
        {
            point2D.eventType = SimpleEventType.takeOff;
            footEventTypeImg.sprite = sprites.Find(x => x.name == "Despegue");
            arrows[0].gameObject.SetActive(false);
            arrows[1].gameObject.SetActive(true);
        }
        else if (point2D.eventType == SimpleEventType.takeOff)
        {
            point2D.eventType = SimpleEventType.contact;
            footEventTypeImg.sprite = sprites.Find(x => x.name == "ContactoInicial");
            arrows[0].gameObject.SetActive(true);
            arrows[1].gameObject.SetActive(false);
        }
        parentOfParent.UpdatePoints();
    }
}
