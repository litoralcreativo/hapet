using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemporalAlgorithmComplement : MonoBehaviour
{
    public AlgorithmScript algorithm;

    public Button CIPI;
    public Button DPPI;
    public Button CIPD;
    public Button DPPD;

    public void UpdateBtnsState()
    {
        CIPI.interactable = false;
        CIPD.interactable = false;
        DPPI.interactable = false;
        DPPD.interactable = false;

        if (algorithm.points.Count == 0)
        {
            CIPI.interactable = true;
            CIPD.interactable  = true;
        }
        else if (algorithm.points.Count == 1)
        {
            if (algorithm.points[0].foot == Foot.left)
            {
                CIPD.interactable = true;

            } else
            {
                CIPI.interactable = true;
            }   
        }
        else if (algorithm.points.Count >= 2)
        {
            if ((algorithm.points.Count) % 2 == 0)
            {
                // must be a takeoff
                DPPI.interactable  = true;
                DPPD.interactable  = true;
            }
            // even - par
            else
            {
                // must be a contact
                CIPI.interactable  = true;
                CIPD.interactable  = true;
            }

            if ((algorithm.points.Count - 2) % 4 == 0 || (algorithm.points.Count - 2) % 4 == 1)
            {
                // if first pair of pattern are different than first event foot, the pattern is wrong
                
                // FIRST PAIR
                if (algorithm.points[0].foot == Foot.left)
                {
                    CIPD.interactable  = false;
                    DPPD.interactable  = false;
                }
                else
                {
                    CIPI.interactable = false;
                    DPPI.interactable  = false;
                }
            }
            else
            {
                // if second pair of pattern are different than second event foot, the pattern is wrong

                // SECOND PAIR
                if (algorithm.points[0].foot == Foot.left)
                {
                    CIPI.interactable = false;
                    DPPI.interactable = false;
                }
                else
                {
                    CIPD.interactable = false;
                    DPPD.interactable = false;
                }
            }

        }
    }
}
