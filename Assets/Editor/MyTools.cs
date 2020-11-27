using UnityEngine;
using UnityEditor;

public static class MyTools
{
    [MenuItem("My Tools/Add To Report %F1")]
    static void DEV_AppendToReport()
    {
        Debug.Log("I press the button");
    }
}
