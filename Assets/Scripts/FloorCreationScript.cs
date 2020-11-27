using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FloorCreationScript : MonoBehaviour
{
    public int numOfPoints = 0;
    public GameObject pointerPrefab;
    public bool adding;
    public string addingName = "x";
    public List<GameObject> listOfReferencePoints;
    public screenManager clickeableArea;

    public Button floorCalculatorBtn;
    public GameObject referenceContainer;
    public GameObject reffMappedPoint;

    bool over = false;
    public Material mat;

    [Header("Length")]
    public float ab;
    public float bc;
    public float cd;
    public float da;
    public float ac;

    [Header("Points")]
    public Vector2 pointA;
    public Vector2 pointB;
    public Vector2 pointC;
    public Vector2 pointD;

    public void SetAB(string st)
    {
        ab = int.Parse(st);
    }

    public void SetBC(string st)
    {
        bc = int.Parse(st);
    }

    public void SetCD(string st)
    {
        cd = int.Parse(st);
    }

    public void SetDA(string st)
    {
        da = int.Parse(st);
    }

    public void SetAC(string st)
    {
        ac = int.Parse(st);
    }


    void Update()
    {
        if (adding)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                AddPoint(Input.mousePosition, addingName);
            }

        }

    }

    public void UpdateDistances()
    {
        
        if (ab > 0 && bc > 0 && cd > 0 && da > 0 && ac > 0)
        {
            floorCalculatorBtn.interactable = true;
        }
        else
        {
            floorCalculatorBtn.interactable = false;
        }
    }

    public void AddPoint(string input)
    {
        adding = true;
        addingName = input;
    }

    public void AddPoint(Vector3 positionOfMouse, string name)
    {
        if (clickeableArea.over)
        {
            GameObject newReferencePoint = Instantiate(pointerPrefab, this.transform);
            Vector2 pos = new Vector2(positionOfMouse.x - Screen.width / 2, positionOfMouse.y - Screen.height / 2);
            newReferencePoint.GetComponent<RectTransform>().localPosition = pos;
            newReferencePoint.GetComponent<ReferencePointScript>().thisName = name;
            newReferencePoint.GetComponent<ReferencePointScript>().textName.text = name;
            listOfReferencePoints.Add(newReferencePoint);
            adding = false;
        } else
        {
            print("outside area");
        }

        if (listOfReferencePoints.Count == 4)
        {
            floorCalculatorBtn.interactable = true;
        } else
        {
            floorCalculatorBtn.interactable = false;
        }

    }



    public void ReferenceTrigonometryCalculation()
    {
        pointA = new Vector2(0, 0);
        pointB = new Vector2(ab, 0);
        print("A (x,y) = " + pointA.x + " - " + pointA.y);
        print("B (x,y) = " + pointB.x + " - " + pointB.y);

        // pointC

        //angle in rad of A
        float angbAc = Mathf.Acos((Mathf.Pow(ab, 2) + Mathf.Pow(ac, 2) - Mathf.Pow(bc, 2)) / (2 * ac * ab));
        float cX = ac * (Mathf.Sin((Mathf.Deg2Rad * 90) - angbAc));
        float cY = -1 * ac * (Mathf.Cos((Mathf.Deg2Rad * 90) - angbAc));
        print("C (x,y) = " + cX + " - " + cY);
        pointC = new Vector2(cX, cY);


        // pointD

        //angle in rad of C
        float angaDc = AngleOfThreeSegments(cd, ac, da);
        float dY = -1 * da * (Mathf.Sin((Mathf.Deg2Rad * 180) - (angaDc + angbAc)));
        float dX = -1 * da * (Mathf.Cos((Mathf.Deg2Rad * 180) - (angaDc + angbAc)));
        print("D (x,y) = " + dX + " - " + dY);
        pointD = new Vector2(dX, dY);

        ResetPrimitivesReferences();
        
    }

    public float AngleOfThreeSegments(float oposite, float left, float right)
    {
        float result = Mathf.Acos((Mathf.Pow(left, 2) + Mathf.Pow(right, 2) - Mathf.Pow(oposite, 2)) / (2 * left * right));
        return result;
    }

    public void ResetPrimitivesReferences()
    {
        for (int i = 0; i < referenceContainer.transform.childCount; i++)
        {
            Destroy(referenceContainer.transform.GetChild(i).gameObject);
        }

        float Offset = ac / 2;

        GameObject pointAmapped = Instantiate(reffMappedPoint, referenceContainer.transform);
        pointAmapped.GetComponent<RectTransform>().localPosition = pointA;

        GameObject pointBmapped = Instantiate(reffMappedPoint, referenceContainer.transform);
        pointBmapped.GetComponent<RectTransform>().localPosition = pointB;

        GameObject pointCmapped = Instantiate(reffMappedPoint, referenceContainer.transform);
        pointCmapped.GetComponent<RectTransform>().localPosition = pointC;

        GameObject pointDmapped = Instantiate(reffMappedPoint, referenceContainer.transform);
        pointDmapped.GetComponent<RectTransform>().localPosition = pointD;
    }
}
