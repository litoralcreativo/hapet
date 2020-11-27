using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Pointer3DScript : MonoBehaviour
{
    public List<Vector2> points2D = new List<Vector2>();
    public List<Point2D> pointsElements = new List<Point2D>();
    public AlgorithmScript algorithm;
    public Pointers2DScript pointers2DScript;
    public GameObject prefab3dElement;
    public Transform parentContainer3d;
    public bool mouseDebug;
    public bool creating3DObjects;
    public bool drawLine;
    public Camera camera;
    public LineRenderer line;

    public TextMeshProUGUI text;

    private Element3D firstPoint;
    private Element3D secondPoint;

    public Texture2D tex;

    [ShowOnly] public Vector3 fp;
    [ShowOnly] public Vector3 sp;

    private void Update()
    {
        points2D.Clear();
        foreach (Point2D pt in algorithm.points)
        {
            points2D.Add(pt.screenPoint);
        }

        if (drawLine)
        {
            if (Input.GetKey(KeyCode.A))
            {
                DrawLine();
            }

            if (firstPoint != null && secondPoint != null)
            {
                if (!text.gameObject.activeSelf)
                {
                    text.gameObject.SetActive(true);

                    float dist = Vector3.Distance(firstPoint.transform.position, secondPoint.transform.position);
                    float xDist = firstPoint.transform.position.x - secondPoint.transform.position.x;

                    text.text = dist + " cm | x: " + xDist + " cm";
                }
            } 
            else
            {
                text.gameObject.SetActive(false);
            }
        }

        if (firstPoint != null)
            fp = firstPoint.transform.position;

        if (secondPoint != null)
            sp = secondPoint.transform.position;

        /*
        if (Input.GetKey(KeyCode.C))
            creating3DObjects = true;
        else
            creating3DObjects = false;

        if (Input.GetKey(KeyCode.D))
            mouseDebug = true;
        else
            mouseDebug = false;


        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < points2D.Count; i++)
            {
                if (mouseDebug)
                    DrawRay(points2D[i]);

                if (creating3DObjects)
                    Create3DMark(points2D[i], algorithm.points[i]);
            }
            creating3DObjects = false;
        }
        */
    }

    public void DrawRay(Vector2 point)
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(point);
        //print(point);

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            print(objectHit.name + " ( " + hit.point.x + " | " + hit.point.z + ")");
        }
        Debug.DrawRay(ray.origin, ray.direction * 5000, Color.green, 5f);
    }

    public void Create3DMarksWithButton()
    {
        foreach (Transform tr in parentContainer3d)
        {
            if (tr.GetComponent<Element3D>())
            {
                Destroy(tr.gameObject);
            }
        }

        for (int i = 0; i < points2D.Count; i++)
            {
                /*if (mouseDebug)
                    DrawRay(points2D[i]);

                if (creating3DObjects)*/
                    Create3DMark(points2D[i], algorithm.points[i]);
            }
    }

    public void Create3DMark(Vector2 point, Point2D pt2d)
    {
        

        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(point);
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
            GameObject newElement = Instantiate(prefab3dElement, parentContainer3d);
            newElement.GetComponent<Element3D>().transform.position = hit.point;
            newElement.GetComponent<Element3D>().worldPosition = new Vector2(hit.point.x, hit.point.z);
            newElement.GetComponent<Element3D>().foot = pt2d.foot;
            newElement.GetComponent<Element3D>().type = pt2d.eventType;

            newElement.GetComponent<Element3D>().SetValues();
        }

        pointers2DScript.pointsShown = false;
    }

    public void DrawLine()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;
                if (objectHit.GetComponent<Element3D>())
                {
                    print(objectHit.name + " ( " + hit.point.x + " | " + hit.point.z + ")");
                    if (firstPoint == null && secondPoint == null)
                    {
                        print("1");
                        firstPoint = objectHit.GetComponent<Element3D>();
                        firstPoint.Select();
                        //line.SetPosition(0, firstPoint.transform.position);
                        //line.SetPosition(1, firstPoint.transform.position);
                    }
                    else if (firstPoint != null && secondPoint == null)
                    {
                        print("2");

                        if (objectHit == firstPoint.transform)
                        {
                            firstPoint.Disselect();
                            firstPoint = null;
                        }
                        else
                        {
                            secondPoint = objectHit.GetComponent<Element3D>();
                            secondPoint.Select();
                        }
                    } 
                    else if (firstPoint == null && secondPoint != null)
                    {
                        print("3");

                        if (objectHit == secondPoint.transform)
                        {
                            secondPoint.Disselect();
                            secondPoint = null;
                        }
                        else
                        {
                            firstPoint = objectHit.GetComponent<Element3D>();
                            firstPoint.Select();
                        }
                    }
                    else if (firstPoint != null && secondPoint != null)
                    {
                        print("4");

                        if (objectHit == secondPoint.transform)
                        {
                            secondPoint.Disselect();
                            secondPoint = null;
                        }
                        else if (objectHit == firstPoint.transform)
                        {
                            firstPoint.Disselect();
                            firstPoint = null;
                        }
                    }
                    else
                    {
                        print("5");

                        if (firstPoint != null)
                        {
                            firstPoint.Disselect();
                            firstPoint = null;
                        }
                        if (secondPoint != null)
                        {
                            secondPoint.Disselect();
                            secondPoint = null;
                        }
                        firstPoint = objectHit.GetComponent<Element3D>();
                    }
                    
                }
            }

            if (firstPoint != null && secondPoint != null)
            {
                line.SetPosition(0, firstPoint.transform.position);
                line.SetPosition(1, secondPoint.transform.position);
            } else
            {
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, Vector3.zero);
            }
                

        }

        if (Input.GetMouseButtonDown(1))
        {
            if (firstPoint != null)
            {
                firstPoint.Disselect();
                firstPoint = null;
            }
            if (secondPoint != null)
            {
                secondPoint.Disselect();
                secondPoint = null;
            }


            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
        }
    }
}
