using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Board_controller))]
public class Editor_Board : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Board_controller board = (Board_controller)target;

        if (GUILayout.Button("Set Zero"))
        {
            board.SetZero();
        }

        if (GUILayout.Button("Update"))
        {
            board.Resize();
        }
    }
}