using UnityEngine;
using UnityEditor;

namespace PaintingManual
{
    [CustomEditor(typeof(Painting))]
    public class PaintingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Painting painting = (Painting)target;

            if (GUILayout.Button("Finish level"))
            {
                painting.DebugComplete();
            }
        }
    }

}
