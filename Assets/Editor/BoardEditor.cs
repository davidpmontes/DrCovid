using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Board))]
public class BoardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Board myScript = (Board)target;

        if (GUILayout.Button("Print board"))
        {
            myScript.PrintBoard();
        }

        if (GUILayout.Button("red"))
        {
            myScript.GeneratePill("red");
        }

        if (GUILayout.Button("blue"))
        {
            myScript.GeneratePill("blue");
        }

        if (GUILayout.Button("yellow"))
        {
            myScript.GeneratePill("yellow");
        }

        if (GUILayout.Button("blueYellow"))
        {
            myScript.GeneratePill("blueYellow");
        }

        if (GUILayout.Button("redBlue"))
        {
            myScript.GeneratePill("redBlue");
        }

        if (GUILayout.Button("redYellow"))
        {
            myScript.GeneratePill("redYellow");
        }
    }
}
