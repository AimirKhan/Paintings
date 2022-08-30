using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid_SizeFix))]
//[CanEditMultipleObjects]
public class Editor_Grid_SizeFix : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Grid_SizeFix grid_SizeFix = (Grid_SizeFix)target;

        if (GUILayout.Button("Set Zero"))
        {
            grid_SizeFix.SetZero();
        }

        if (GUILayout.Button("Update"))
        {
            grid_SizeFix.Resize();
        }
    }
}
