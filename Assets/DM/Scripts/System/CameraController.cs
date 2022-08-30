using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;
    [SerializeField] private float m_Paddings = 0;
    [SerializeField] private LevelController m_LevelControlller;

    public void ZoomingCameraToGrid()
    {
        LevelData _levelData = m_LevelControlller.Levels;
        m_Paddings = (1 + (m_Paddings / 100)); 

        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = _levelData.FieldWidthCount / _levelData.FieldHeightCount;

        if (screenRatio >= targetRatio)
        {
            m_Camera.orthographicSize = (_levelData.FieldHeightCount / 2f) * m_Paddings;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            m_Camera.orthographicSize = (_levelData.FieldHeightCount / 2f * differenceInSize) * m_Paddings;
        }
    }

    public float Paddings
    {
        get
        {
            return m_Paddings;
        }
        set
        {
            m_Paddings = value;
        }
    }
}
