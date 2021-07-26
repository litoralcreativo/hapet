using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System;

public class Pointers2DScript : MonoBehaviour
{
    [Header("Points")]
    public List<GameObject> points;
    public PointerScript pointer;
    public GameObject prefabPointer2DElement;
    public bool videoOK;
    public VideoPlayer vPlayer;
    public GameObject containerOfPoints;
    public bool pointsShown;

    public bool temporalStudy;
    private TemporalAlgorithmComplement complementTemp;

    private void Update()
    {

        foreach (GameObject child in points)
        {
            if (child.activeSelf != pointsShown)
            {
                child.SetActive(pointsShown);
            }
        }        

        if (vPlayer.frame > 0)
        {
            videoOK = true;
            //pointer.ableToPoint = true;
        }
        else
        {
            //pointer.ableToPoint = false;
            videoOK = false;
        }

        if (!temporalStudy)
        {
            if (pointer.pointing)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Point2DElement holderp2de = new Point2DElement();
                    bool create = true;
                    foreach (GameObject go in points)
                    {
                        if (go.GetComponent<Point2DElement>().point2D.millisSinceStart == vPlayer.time)
                        {
                            holderp2de = go.GetComponent<Point2DElement>();
                            create = false;
                        }
                    }


                    if (!create)
                        holderp2de.DeleteSelf();


                    Vector2 pos = pointer.transform.position;

                    //                Vector2 pos = Input.mousePosition;

                    GameObject newPointer = Instantiate(prefabPointer2DElement, containerOfPoints.transform);
                    newPointer.GetComponent<RectTransform>().position = pos;
                    if (pointer.pointingCI)
                    {
                        newPointer.GetComponent<Point2DElement>().CreateEvent(Foot.left, SimpleEventType.contact);
                    }
                    else if (pointer.pointingDP)
                    {
                        newPointer.GetComponent<Point2DElement>().CreateEvent(Foot.left, SimpleEventType.takeOff);
                    }

                    newPointer.GetComponent<Point2DElement>().vplayer = vPlayer;
                    newPointer.GetComponent<Point2DElement>().ChangeMilliseconds(vPlayer.time);
                    newPointer.GetComponent<Point2DElement>().screenPosition = pos;
                    UpdatePoints();

                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Point2DElement holderp2de = new Point2DElement();
                    bool create = true;
                    foreach (GameObject go in points)
                    {
                        if (go.GetComponent<Point2DElement>().point2D.millisSinceStart == vPlayer.time)
                        {
                            holderp2de = go.GetComponent<Point2DElement>();
                            create = false;
                        }
                    }

                    if (!create)
                        holderp2de.DeleteSelf();

                    Vector2 pos = pointer.transform.position;

                    //Vector2 pos = Input.mousePosition;
                    GameObject newPointer = Instantiate(prefabPointer2DElement, containerOfPoints.transform);
                    newPointer.GetComponent<RectTransform>().position = pos;
                    if (pointer.pointingCI)
                    {
                        newPointer.GetComponent<Point2DElement>().CreateEvent(Foot.left, SimpleEventType.contact);
                    }
                    else if (pointer.pointingDP)
                    {
                        newPointer.GetComponent<Point2DElement>().CreateEvent(Foot.left, SimpleEventType.takeOff);
                    }
                    newPointer.GetComponent<Point2DElement>().vplayer = vPlayer;
                    newPointer.GetComponent<Point2DElement>().ChangeFoot();
                    newPointer.GetComponent<Point2DElement>().ChangeMilliseconds(vPlayer.time);
                    UpdatePoints();
                }
            }
        }
        

        if (vPlayer.isPlaying)
        {
            foreach (GameObject go in points)
            {
                go.GetComponent<Point2DElement>().selected = false;
            }
        } 
    }

    public void ChangeVisibleState()
    {
        pointsShown = !pointsShown;
    }

    public void DeleteAllPoints()
    {
        List <GameObject> objs = new List<GameObject>();
        foreach (Transform tr in containerOfPoints.transform)
        {
            Debug.Log(tr);
            Point2DElement p2de = tr.GetComponent<Point2DElement>();
            if (p2de != null)
            {
                objs.Add(tr.gameObject);
                
            }
        }
        foreach (GameObject go in objs)
        {
            go.transform.parent = null;
            Destroy(go);
        }

        UpdatePoints();
    }

    public void UpdatePoints()
    {
        points.Clear();
        foreach (Transform tr in containerOfPoints.transform)
        {
            if (tr.GetComponent<Point2DElement>() != null)
            {
                points.Add(tr.gameObject);
            }
        }

        GetComponent<AlgorithmScript>().UpdatePoints();

        if (temporalStudy)
        {
            if (complementTemp == null)
                complementTemp = GetComponent<TemporalAlgorithmComplement>();

            GetComponent<TemporalAlgorithmComplement>().UpdateBtnsState();
        }
    }

    public void AddCIPI()
    {
        bool create = true;
        Point2DElement holderp2de = new Point2DElement();

        foreach (GameObject go in points)
        {
            if (go.GetComponent<Point2DElement>().point2D.millisSinceStart == vPlayer.time)
            {
                holderp2de = go.GetComponent<Point2DElement>();
                create = false;
            }
        }

        if (!create)
            holderp2de.DeleteSelf();

        GameObject newPointer = Instantiate(prefabPointer2DElement, containerOfPoints.transform);

        float posX = Screen.width * (float)vPlayer.frame / (float)vPlayer.frameCount - (Screen.width / 2);

        newPointer.GetComponent<RectTransform>().localPosition = new Vector2(posX, 0);
        newPointer.GetComponent<Point2DElement>().CreateEvent(Foot.left, SimpleEventType.contact);
        newPointer.GetComponent<Point2DElement>().vplayer = vPlayer;
        newPointer.GetComponent<Point2DElement>().ChangeMilliseconds(vPlayer.time, vPlayer.frame);
        UpdatePoints();
    }

    public void AddCIPD()
    {
        bool create = true;
        Point2DElement holderp2de = new Point2DElement();

        foreach (GameObject go in points)
        {
            if (go.GetComponent<Point2DElement>().point2D.millisSinceStart == vPlayer.time)
            {
                holderp2de = go.GetComponent<Point2DElement>();
                create = false;
            }
        }

        if (!create)
            holderp2de.DeleteSelf();

        GameObject newPointer = Instantiate(prefabPointer2DElement, containerOfPoints.transform);

        float posX = Screen.width * (float)vPlayer.frame / (float)vPlayer.frameCount - (Screen.width / 2);
        newPointer.GetComponent<RectTransform>().localPosition = new Vector2(posX, 0);
        newPointer.GetComponent<Point2DElement>().CreateEvent(Foot.right, SimpleEventType.contact);
        newPointer.GetComponent<Point2DElement>().vplayer = vPlayer;
        newPointer.GetComponent<Point2DElement>().ChangeMilliseconds(vPlayer.time, vPlayer.frame);
        UpdatePoints();
    }

    public void AddDPPI()
    {

        bool create = true;
        Point2DElement holderp2de = new Point2DElement();

        foreach (GameObject go in points)
        {
            if (go.GetComponent<Point2DElement>().point2D.millisSinceStart == vPlayer.time)
            {
                holderp2de = go.GetComponent<Point2DElement>();
                create = false;
            }
        }

        if (!create)
            holderp2de.DeleteSelf();

        GameObject newPointer = Instantiate(prefabPointer2DElement, containerOfPoints.transform);

        float posX = Screen.width * (float)vPlayer.frame / (float)vPlayer.frameCount - (Screen.width/2);

        newPointer.GetComponent<RectTransform>().localPosition = new Vector2(posX, 0);
        newPointer.GetComponent<Point2DElement>().CreateEvent(Foot.left, SimpleEventType.takeOff);
        newPointer.GetComponent<Point2DElement>().vplayer = vPlayer;
        newPointer.GetComponent<Point2DElement>().ChangeMilliseconds(vPlayer.time, vPlayer.frame);
        UpdatePoints();
    }

    public void AddDPPD()
    {
        bool create = true;
        Point2DElement holderp2de = new Point2DElement();

        foreach (GameObject go in points)
        {
            if (go.GetComponent<Point2DElement>().point2D.millisSinceStart == vPlayer.time)
            {
                holderp2de = go.GetComponent<Point2DElement>();
                create = false;
            }
        }

        if (!create)
            holderp2de.DeleteSelf();

        GameObject newPointer = Instantiate(prefabPointer2DElement, containerOfPoints.transform);

        float posX = Screen.width * (float)vPlayer.frame / (float)vPlayer.frameCount - (Screen.width / 2);

        newPointer.GetComponent<RectTransform>().localPosition = new Vector2(posX, 0);
        newPointer.GetComponent<Point2DElement>().CreateEvent(Foot.right, SimpleEventType.takeOff);
        newPointer.GetComponent<Point2DElement>().vplayer = vPlayer;
        newPointer.GetComponent<Point2DElement>().ChangeMilliseconds(vPlayer.time, vPlayer.frame);
        UpdatePoints();
    }
}
