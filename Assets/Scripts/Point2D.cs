using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point2D
{
    public Vector2 screenPoint;
    public Foot foot;
    public SimpleEventType eventType;
    public double millisSinceStart;

    public Point2D(Vector2 screenP, SimpleEventType eType, Foot f)
    {
        screenPoint = screenP;
        eventType = eType;
        foot = f;
    }
}

public enum Foot
{
    left,
    right
}

public enum SimpleEventType
{
    contact,
    takeOff,
}

public enum ComplexEventType
{
    tipContact,
    tipTakeOff,
    backContact,
    backTakeOff,
}
