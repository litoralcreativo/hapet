using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfoPanelScript : MonoBehaviour
{
    private List<Point2D> panelList;
    private List<float> calculationPanelList;
    [ShowOnly] public List<string> panelListFloats = new List<string>();
    [ShowOnly] public List<string> calculationPanelListString = new List<string>();
    private float symmetryValue;
    public GameObject itemPanelPrefab;
    public AlgorithmScript algorithm;

    public ListType list;

    public void GetValues()
    {
        if (panelList == null)
        {
            panelList = new List<Point2D>();
        }

        if (calculationPanelList == null)
        {
            calculationPanelList = new List<float>();
        }

        panelListFloats.Clear();
        calculationPanelListString.Clear();

        symmetryValue = 0;

        //panelList.Clear();

        if (list == ListType.CIPI)
        {
            panelList = algorithm.CIpoints_L;
        } 
        else if (list == ListType.CIPD)
        {
            panelList = algorithm.CIpoints_R;
        }
        else if (list == ListType.DPPI)
        {
            panelList = algorithm.DPpoints_L;
        }
        else if (list == ListType.DPPD)
        {
            panelList = algorithm.DPpoints_R;
        }

        // calculations
        if (list == ListType.StepL)
        {
            calculationPanelList = algorithm.stepTime_L;
            calculationPanelList.Add(algorithm.stepTimeAvr_L);
        } 
        else if (list == ListType.StepR)
        {
            calculationPanelList = algorithm.stepTime_R;
            calculationPanelList.Add(algorithm.stepTimeAvr_R);
        }
        else if (list == ListType.FlyL)
        {
            calculationPanelList = algorithm.flyTime_L;
            calculationPanelList.Add(algorithm.flyTimeAvr_L);
        }
        else if (list == ListType.FlyR)
        {
            calculationPanelList = algorithm.flyTime_R;
            calculationPanelList.Add(algorithm.flyTimeAvr_R);
        }
        else if (list == ListType.PassingL)
        {
            calculationPanelList = algorithm.passingTime_L;
            calculationPanelList.Add(algorithm.passingTimeAvr_L);
        }
        else if (list == ListType.PassingR)
        {
            calculationPanelList = algorithm.passingTime_R;
            calculationPanelList.Add(algorithm.passingTimeAvr_R);
        }
        else if (list == ListType.CycleL)
        {
            calculationPanelList = algorithm.cycleTime_L;
            calculationPanelList.Add(algorithm.cycleTimeAvr_L);
        }
        else if (list == ListType.CycleR)
        {
            calculationPanelList = algorithm.cycleTime_R;
            calculationPanelList.Add(algorithm.cycleTimeAvr_R);
        }
        else if (list == ListType.SymmStep)
        {
            symmetryValue = algorithm.stepSymmetry;
        }
        else if (list == ListType.SymmFly)
        {
            symmetryValue = algorithm.flySymmetry;
        }
        else if (list == ListType.SymmPass)
        {
            symmetryValue = algorithm.passingSymmetry;
        }
        else if (list == ListType.SymmCycle)
        {
            symmetryValue = algorithm.cycleSymmetry;
        }


        SetValues();
    }

    public enum ListType
    {
        CIPI,
        CIPD,
        DPPI, 
        DPPD,
        StepL,
        StepR,
        FlyL,
        FlyR,
        PassingL,
        PassingR,
        CycleL,
        CycleR,
        SymmStep,
        SymmFly,
        SymmPass,
        SymmCycle,
    }

    public void SetValues()
    {
        foreach (Transform child in transform)
        {
            if (child.GetComponent<GridInfoItemScript>() != null)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (Point2D pt in panelList)
        {
            GameObject newRecord = Instantiate(itemPanelPrefab, transform);
            newRecord.GetComponent<GridInfoItemScript>().value = (float)pt.millisSinceStart;
            newRecord.GetComponent<GridInfoItemScript>().SetText();
            panelListFloats.Add(newRecord.GetComponent<GridInfoItemScript>()._text.text);
        }

        for (int i = 0; i < calculationPanelList.Count; i++)
        {
            if (i != calculationPanelList.Count - 1)
            {
                GameObject newRecord = Instantiate(itemPanelPrefab, transform);
                newRecord.GetComponent<GridInfoItemScript>().value = calculationPanelList[i];
                newRecord.GetComponent<GridInfoItemScript>().SetText();
                calculationPanelListString.Add(newRecord.GetComponent<GridInfoItemScript>()._text.text);
            }
            else
            {
                GameObject newRecord = Instantiate(itemPanelPrefab, transform);
                newRecord.GetComponent<GridInfoItemScript>().value = calculationPanelList[i];
                newRecord.GetComponent<GridInfoItemScript>().SetText(true);
                calculationPanelListString.Add(newRecord.GetComponent<GridInfoItemScript>()._text.text);
            }
        }

        if (symmetryValue != 0)
        {
            GameObject newRecord = Instantiate(itemPanelPrefab, transform);
            newRecord.GetComponent<GridInfoItemScript>().value = symmetryValue;
            newRecord.GetComponent<GridInfoItemScript>().SetSymmetryText();
            calculationPanelListString.Add(newRecord.GetComponent<GridInfoItemScript>()._text.text);
        }
    }
}
