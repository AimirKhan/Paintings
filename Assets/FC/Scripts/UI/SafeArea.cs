using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private Rect m_SafeArea;
    [SerializeField] private RectTransform m_RectTransform;

    private Vector2 anchorMin;
    private Vector2 anchorMax;

    private bool isLeft;

    private void Awake()
    {
        m_SafeArea = Screen.safeArea;
        //m_RectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Настраиваем ориентацию телефона.
    /// </summary>
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.orientation = ScreenOrientation.AutoRotation;

        if (Screen.orientation == ScreenOrientation.LandscapeLeft)
            isLeft = true;
        else isLeft = false;

        Screen.autorotateToLandscapeLeft = Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = Screen.autorotateToPortraitUpsideDown = false;

        //UpdateSafeArea();
    }

    /// <summary>
    /// Обрабатываем переворот телефона
    /// </summary>
    private void Update()
    {
        if (Screen.orientation == ScreenOrientation.LandscapeLeft && isLeft)
        {
            isLeft = !isLeft;
            UpdateSafeArea();
        }
        else if(Screen.orientation == ScreenOrientation.LandscapeRight && !isLeft)
        {
            isLeft = !isLeft;
            UpdateSafeArea();
        }
    }

    void UpdateSafeArea()
    {
        anchorMin = m_SafeArea.position;
        anchorMax = m_SafeArea.position + m_SafeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height; 

        m_RectTransform.anchorMin = anchorMin;
        m_RectTransform.anchorMax = anchorMax;
    }
}
