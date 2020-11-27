using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AlgorithmScript : MonoBehaviour
{
    private Pointers2DScript pointer;
    public List<Point2D> points = new List<Point2D>();

    public List<Point2D> DPpoints_L;
    public List<Point2D> DPpoints_R;
    public List<Point2D> CIpoints_L;
    public List<Point2D> CIpoints_R;
    public List<LedScript> monitorLeds;

    [ShowOnly] public int pointsCount;

    [Header("Type of Contact")]
    [ShowOnly] public int contactPointsCount;
    [ShowOnly] public int takeOffPointsCount;

    [Header("Foot")]
    [ShowOnly] public int leftPointCount;
    [ShowOnly] public int rightPointCount;

    [Header("Booleans for calculations")]
    [ShowOnly] public bool startingWithTwo_CI;
    [ShowOnly] public bool pattern;

    [ShowOnly] public bool canDetectCycle_L;
    [ShowOnly] public bool canDetectCycle_R;

    [ShowOnly] public bool canDetectStep_L;
    [ShowOnly] public bool canDetectStep_R;
    
    [ShowOnly] public bool canDetectFly_L;
    [ShowOnly] public bool canDetectFly_R;

    [ShowOnly] public bool canDetectPassing_LR;
    [ShowOnly] public bool canDetectPassing_RL;

    [ShowOnly] public bool startWith_L;
    [ShowOnly] public bool startWith_R;


    [Header("Cycle Time")]
    [ShowOnly] public List<float> cycleTime_L;
    [ShowOnly] public float cycleTimeAvr_L;

    [ShowOnly] public List<float> cycleTime_R;
    [ShowOnly] public float cycleTimeAvr_R;

    [Header("Step Time")]
    [ShowOnly] public List<float> stepTime_L;
    [ShowOnly] public float stepTimeAvr_L;

    [ShowOnly] public List<float> stepTime_R;
    [ShowOnly] public float stepTimeAvr_R;

    [Header("Fly Time")]
    [ShowOnly] public List<float> flyTime_L;
    [ShowOnly] public float flyTimeAvr_L;

    [ShowOnly] public List<float> flyTime_R;
    [ShowOnly] public float flyTimeAvr_R;

    [Header("Passing Time")]
    [ShowOnly] public List<float> passingTime_L;
    [ShowOnly] public float passingTimeAvr_L;

    [ShowOnly] public List<float> passingTime_R;
    [ShowOnly] public float passingTimeAvr_R;

    [Header("Symmetry")]
    [ShowOnly] public float stepSymmetry;
    [ShowOnly] public float flySymmetry;
    [ShowOnly] public float passingSymmetry;
    [ShowOnly] public float cycleSymmetry;

    private TemporalAlgorithmComplement complement;




    /* Info
     * 
     *  SIMETRÍA = 100 - ((100 * Mathf.Abs(Pr - Pl)) / (0.5 * Mathf.Abs(Pr + Pl)))
     * 
     *  Pr y Pl son los parametros temporales para cada miembro
     *  
     *  Velocidad: distancia recorrida en la caminata en una unidad de tiempo y en la dirección de la marcha.
     *  Cadencia: número de pasos realizados en un minuto.
     *  
     *  Tiempo de ciclo: tiempo que transcurre entre dos eventos iguales del ciclo. Mismo Evento y Mismo Pie.
     *  Tiempo de apoyo: tiempo que transcurre entre el contacto y el despegue
     *  Tiempo de vuelo: tiempo que transcurre entre el despegue y el contacto
     *  Tiempo de paso: tiempo que transcurre entre apoyo de un pie y apoyo del pie contrario.
     * 
     */


    private void Awake()
    {
        pointer = GetComponent<Pointers2DScript>();
        points = new List<Point2D>();

        CIpoints_L = new List<Point2D>();
        CIpoints_R = new List<Point2D>();

        DPpoints_L = new List<Point2D>();
        DPpoints_R = new List<Point2D>();

        // calculations lists
        cycleTime_L = new List<float>();
        cycleTime_R = new List<float>();

        stepTime_L = new List<float>();
        stepTime_R = new List<float>();

        flyTime_L = new List<float>();
        flyTime_R = new List<float>();

        passingTime_L = new List<float>();
        passingTime_R = new List<float>();
        
        pointer.UpdatePoints();
        UpdatePoints();
    }

    public void UpdatePoints()
    {
        points.Clear();

        CIpoints_L.Clear();
        CIpoints_R.Clear();
        DPpoints_L.Clear();
        DPpoints_R.Clear();


        foreach (GameObject pt in pointer.points)
        {
            Point2D pt2d = pt.GetComponent<Point2DElement>().point2D;
            points.Add(pt2d);
        }
        pointsCount = points.Count;

        int CI = 0;
        int DP = 0;
        int LF = 0;
        int RF = 0;

        foreach (Point2D pt in points)
        {  
            // Get points by type
            if (pt.foot == Foot.left)
            {
                LF++;
                // Get points by foot
                if (pt.eventType == SimpleEventType.contact)
                {
                    CI++;
                    CIpoints_L.Add(pt);
                }
                else
                {
                    DP++;
                    DPpoints_L.Add(pt);
                }
            } 
            else
            {
                RF++;
                // Get points by foot
                if (pt.eventType == SimpleEventType.contact)
                {
                    CI++;
                    CIpoints_R.Add(pt);
                }
                else
                {
                    DP++;
                    DPpoints_R.Add(pt);
                }
            }
        }

        contactPointsCount = CI;
        takeOffPointsCount = DP;
        leftPointCount = LF;
        rightPointCount = RF;

        List<Point2D> _CIpoints_L = CIpoints_L.OrderBy(o => o.millisSinceStart).ToList();
        List<Point2D> _CIpoints_R = CIpoints_R.OrderBy(o => o.millisSinceStart).ToList();
        List<Point2D> _DPpoints_L = DPpoints_L.OrderBy(o => o.millisSinceStart).ToList();
        List<Point2D> _DPpoints_R = DPpoints_R.OrderBy(o => o.millisSinceStart).ToList();
        List<Point2D> _points = points.OrderBy(o => o.millisSinceStart).ToList();

        points.Clear();
        points = _points;

        CIpoints_L.Clear();
        CIpoints_L = _CIpoints_L;

        CIpoints_R.Clear();
        CIpoints_R = _CIpoints_R;

        DPpoints_L.Clear();
        DPpoints_L = _DPpoints_L;

        DPpoints_R.Clear();
        DPpoints_R = _DPpoints_R;


        UpdateBooleans();
        CalculateParameters();
    }

    public void UpdateBooleans()
    {
        // set all to false
        canDetectCycle_L = false;
        canDetectCycle_R = false;
        canDetectStep_L = false;
        canDetectStep_R = false;
        canDetectFly_L = false;
        canDetectFly_R = false;
        canDetectPassing_LR = false;
        canDetectPassing_RL = false;
        startingWithTwo_CI = false;
        pattern = false;

        // Cycle L
        if (CIpoints_L.Count >= 2)
        {
            canDetectCycle_L = true;
        }

        // Cycle R
        if (CIpoints_R.Count >= 2)
        {
            canDetectCycle_R = true;
        }

        // Step L
        if (CIpoints_L.Count >= 1 && DPpoints_L.Count >= 1)
        {
            if (CIpoints_L[0].millisSinceStart < DPpoints_L[0].millisSinceStart)
            {
                canDetectStep_L = true;
            }
        }

        // Step R
        if (CIpoints_R.Count >= 1 && DPpoints_R.Count >= 1)
        {
            if (CIpoints_R[0].millisSinceStart < DPpoints_R[0].millisSinceStart)
            {
                canDetectStep_R = true;
            }
        }

        // Fly L
        if (CIpoints_L.Count >= 2 && DPpoints_L.Count >= 1)
        {
            if (CIpoints_L[1].millisSinceStart > DPpoints_L[0].millisSinceStart)
            {
                canDetectFly_L = true;
            }
        }

        // Fly R
        if (CIpoints_R.Count >= 2 && DPpoints_R.Count >= 1)
        {
            if (CIpoints_R[1].millisSinceStart > DPpoints_R[0].millisSinceStart)
            {
                canDetectFly_R = true;
            }
        }

        // Pasing L <-> R
        if (CIpoints_L.Count >= 1 && CIpoints_R.Count >= 1)
        {
            if (CIpoints_L[0].millisSinceStart < CIpoints_R[0].millisSinceStart)
            {
                startWith_R = false;
                startWith_L = true;
            }
            else
            {
                startWith_L = false;
                startWith_R = true;
            }

            if (startWith_L)
            {
                canDetectPassing_LR = true;
                if (CIpoints_L.Count >= 2)
                {
                    canDetectPassing_RL = true;
                }
            } else if (startWith_R)
            {
                canDetectPassing_RL = true;
                if (CIpoints_R.Count >= 2)
                {
                    canDetectPassing_LR = true;
                }
            }

        }

        // Two CI verification
        if (points.Count >= 2)
        {
            if (points[0].eventType == SimpleEventType.contact && points[0].foot == Foot.left)
            {
                if (points[1].eventType == SimpleEventType.contact && points[1].foot == Foot.right)
                {
                    startingWithTwo_CI = true;
                }
            }
            else if (points[0].eventType == SimpleEventType.contact && points[0].foot == Foot.right)
            {
                if (points[1].eventType == SimpleEventType.contact && points[1].foot == Foot.left)
                {
                    startingWithTwo_CI = true;
                }
            }
            else
            {
                startingWithTwo_CI = false;
            }
        }

        pattern = PaternVerification();
        UpdateLeds();
    }

    public void UpdateLeds()
    {

        // Two CI
        if (startingWithTwo_CI)
            monitorLeds[0].EnableLed();
        else 
            monitorLeds[0].DisableLed();

        // StepL
        if (canDetectStep_L)
            monitorLeds[1].EnableLed();
        else
            monitorLeds[1].DisableLed();

        // StepR
        if (canDetectStep_R)
            monitorLeds[2].EnableLed();
        else
            monitorLeds[2].DisableLed();

        // FlyL
        if (canDetectFly_L)
            monitorLeds[3].EnableLed();
        else
            monitorLeds[3].DisableLed();

        // FlyR
        if (canDetectFly_R)
            monitorLeds[4].EnableLed();
        else
            monitorLeds[4].DisableLed();

        // PassLR
        if (canDetectPassing_LR)
            monitorLeds[5].EnableLed();
        else
            monitorLeds[5].DisableLed();

        // PassRL
        if (canDetectPassing_RL)
            monitorLeds[6].EnableLed();
        else
            monitorLeds[6].DisableLed();

        // CycleL
        if (canDetectCycle_L)
            monitorLeds[7].EnableLed();
        else
            monitorLeds[7].DisableLed();

        // CycleR
        if (canDetectCycle_R)
            monitorLeds[8].EnableLed();
        else
            monitorLeds[8].DisableLed();

        // pattern (the most important)
        if (pattern)
            monitorLeds[9].EnableLed();
        else
            monitorLeds[9].DisableLed();

    }

    public bool PaternVerification()
    {
        bool result = false;

        // case 0
        if (points.Count == 0)
        {
            result = true;
        }

        // case 1
        if (points.Count == 1 && points[0].eventType == SimpleEventType.contact)
        {
            result = true;
        }
        // case 2
        else if (points.Count == 2 && points[0].eventType == SimpleEventType.contact && points[1].eventType == SimpleEventType.contact)
        {
            if (points[0].foot != points[1].foot)
                result = true;
        }
        //case more than 2
        else if (points.Count > 2)
        {
            int count = 0;
            for (int i = 2; i < points.Count; i++)
            {
                // Event Type verification

                // odd - impar
                if ((i+1)%2 != 0)
                {
                    // if odd its not takeOff, the pattern is wrong
                    if (points[i].eventType != SimpleEventType.takeOff)
                    {
                        count++;
                    }
                }
                // even - par
                else
                {
                    // if even its not contact, the pattern is wrong
                    if (points[i].eventType != SimpleEventType.contact)
                    {
                        count++;
                    }
                }

                // Foot Verification
                if ((i-2) % 4 == 0 || (i - 2) % 4 == 1)
                {
                    // if first pair of pattern are different than first event foot, the pattern is wrong
                    if (points[i].foot != points[0].foot)
                    {
                        count++;
                    }
                }
                else
                {
                    // if second pair of pattern are different than second event foot, the pattern is wrong
                    if (points[i].foot != points[1].foot)
                    {
                        count++;
                    }
                }
            }
            result = (count == 0);
        }


        return result;
    }

    public void CalculateParameters()
    {
        // reset every value
        cycleTime_L.Clear();
        cycleTimeAvr_L = 0;
        cycleTime_R.Clear();
        cycleTimeAvr_R = 0;

        stepTime_L.Clear();
        stepTimeAvr_L = 0;
        stepTime_R.Clear();
        stepTimeAvr_R = 0;

        flyTime_L.Clear();
        flyTimeAvr_L = 0;
        flyTime_R.Clear();
        flyTimeAvr_R = 0;

        passingTime_L.Clear();
        passingTimeAvr_L = 0;
        passingTime_R.Clear();
        passingTimeAvr_R = 0;

        stepSymmetry = 0;
        flySymmetry = 0;
        passingSymmetry = 0;
        cycleSymmetry = 0;

        if (pattern)
        {

            // Cycle L
            if (canDetectCycle_L)
            {
                for (int i = 1; i < CIpoints_L.Count; i++)
                {
                    float dif = (float)(CIpoints_L[i].millisSinceStart - CIpoints_L[i - 1].millisSinceStart);
                    cycleTime_L.Add(dif);
                }

                cycleTimeAvr_L = Avarage(cycleTime_L);
            }

            // Cycle R
            if (canDetectCycle_R)
            {
                for (int i = 1; i < CIpoints_R.Count; i++)
                {
                    float dif = (float)(CIpoints_R[i].millisSinceStart - CIpoints_R[i - 1].millisSinceStart);
                    cycleTime_R.Add(dif);
                }

                cycleTimeAvr_R = Avarage(cycleTime_R);
            }

            // Step L
            if (canDetectStep_L)
            {
                for (int i = 0; i < DPpoints_L.Count; i++)
                {
                    float dif = (float)(DPpoints_L[i].millisSinceStart - CIpoints_L[i].millisSinceStart);
                    stepTime_L.Add(dif);
                }
                stepTimeAvr_L = Avarage(stepTime_L);
            }

            // Step R
            if (canDetectStep_R)
            {
                for (int i = 0; i < DPpoints_R.Count; i++)
                {
                    float dif = (float)(DPpoints_R[i].millisSinceStart - CIpoints_R[i].millisSinceStart);
                    stepTime_R.Add(dif);
                }
                stepTimeAvr_R = Avarage(stepTime_R);
            }

            // Fly L
            if (canDetectFly_L)
            {
                if (Mathf.Abs(CIpoints_L.Count - DPpoints_L.Count) <= 1)
                {
                    for (int i = 1; i < CIpoints_L.Count; i++)
                    {
                        float dif = (float)(CIpoints_L[i].millisSinceStart - DPpoints_L[i - 1].millisSinceStart);
                        flyTime_L.Add(dif);
                    }
                    flyTimeAvr_L = Avarage(flyTime_L);
                } 
                else
                {
                    for (int i = 1; i < CIpoints_L.Count-1; i++)
                    {
                        float dif = (float)(CIpoints_L[i].millisSinceStart - DPpoints_L[i - 1].millisSinceStart);
                        flyTime_L.Add(dif);
                    }
                    flyTimeAvr_L = Avarage(flyTime_L);
                }
            }

            // Fly R
            if (canDetectFly_R)
            {
                if (Mathf.Abs(CIpoints_R.Count - DPpoints_R.Count) <= 1)
                {
                    for (int i = 1; i < CIpoints_R.Count; i++)
                    {
                        float dif = (float)(CIpoints_R[i].millisSinceStart - DPpoints_R[i - 1].millisSinceStart);
                        flyTime_R.Add(dif);
                    }
                    flyTimeAvr_R = Avarage(flyTime_R);
                } 
                else
                {
                    for (int i = 1; i < CIpoints_R.Count-1; i++)
                    {
                        float dif = (float)(CIpoints_R[i].millisSinceStart - DPpoints_R[i - 1].millisSinceStart);
                        flyTime_R.Add(dif);
                    }
                    flyTimeAvr_R = Avarage(flyTime_R);
                }
            }

            // Passing L -> R
            if (canDetectPassing_LR)
            {
                if (startWith_L)
                {
                    if (CIpoints_R.Count <= CIpoints_L.Count)
                    {
                        for (int i = 0; i < CIpoints_R.Count; i++)
                        {
                            float dif = (float)(CIpoints_R[i].millisSinceStart - CIpoints_L[i].millisSinceStart);
                            passingTime_L.Add(dif);
                        }
                        passingTimeAvr_L = Avarage(passingTime_L);
                    }
                    else
                    {
                        Debug.Log("Error when passing LR starting with L");
                    }
                } 
                else if (startWith_R)
                {
                    if (CIpoints_R.Count >= CIpoints_L.Count)
                    {
                        for (int i = 1; i < CIpoints_R.Count; i++)
                        {
                            float dif = (float)(CIpoints_R[i].millisSinceStart - CIpoints_L[i-1].millisSinceStart);
                            passingTime_L.Add(dif);
                        }
                        passingTimeAvr_L = Avarage(passingTime_L);
                    }
                    else
                    {
                        Debug.Log("Error when passing LR starting with R");
                    }
                }         
            }

            // Passing R -> L
            if (canDetectPassing_RL)
            {
                if (startWith_R)
                {
                    if (CIpoints_L.Count <= CIpoints_R.Count && Mathf.Abs(CIpoints_L.Count - CIpoints_R.Count) <= 1)
                    {
                        for (int i = 0; i < CIpoints_L.Count; i++)
                        {
                            float dif = (float)(CIpoints_L[i].millisSinceStart - CIpoints_R[i].millisSinceStart);
                            passingTime_R.Add(dif);
                        }
                        passingTimeAvr_R = Avarage(passingTime_R);
                    }
                    else
                    {
                        Debug.Log("Error when passing RL starting with R");
                    }
                }
                else if (startWith_L)
                {
                    if (CIpoints_L.Count >= CIpoints_R.Count && Mathf.Abs(CIpoints_L.Count-CIpoints_R.Count)<=1)
                    {
                        for (int i = 1; i < CIpoints_L.Count; i++)
                        {
                            float dif = (float)(CIpoints_L[i].millisSinceStart - CIpoints_R[i - 1].millisSinceStart);
                            passingTime_R.Add(dif);
                        }
                        passingTimeAvr_R = Avarage(passingTime_R);
                    }
                    else
                    {
                        Debug.Log("Error when passing RL starting with L");
                    }
                }
            }

            //Symmetry
            CalculateSymmetry();
        }
        else
        {
            Debug.Log("Pattern is Wrong, can't calculate anything");
        }
    }

    public void CalculateSymmetry()
    {
        stepSymmetry = Symmetry(stepTimeAvr_R, stepTimeAvr_L);
        flySymmetry = Symmetry(flyTimeAvr_R, flyTimeAvr_L);
        passingSymmetry = Symmetry(passingTimeAvr_R, passingTimeAvr_L);
        cycleSymmetry = Symmetry(cycleTimeAvr_R, cycleTimeAvr_L);
    }

    public float Avarage(List<float> values)
    {
        float result = 0;
        foreach (float val in values)
        {
            result += val;
        }

        result = result / values.Count;
        return result;
    }

    public float Symmetry(float pr, float pl)
    {
        // SIMETRÍA = 100 - ((100 * Mathf.Abs(Pr - Pl)) / (0.5 * Mathf.Abs(Pr + Pl)))
        float result = 100 - ((100 * Mathf.Abs(pr - pl)) / (0.5f * Mathf.Abs(pr + pl)));
        return result;
    }
}
