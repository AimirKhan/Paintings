using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StickersController))]
public class EditorStickers : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        StickersController stickersController = (StickersController)target;

        GUILayout.Label("m_CountLevels = " + stickersController.getCountLevels());
        GUILayout.Label("m_CountStickers = " + stickersController.getCountStickers());
        GUILayout.Label("m_CountNewStickers = " + stickersController.getCountNewStickers());

        if (GUILayout.Button("Add Stickers"))
        {
            stickersController.DebugAddlevel();
        }
    }
}
