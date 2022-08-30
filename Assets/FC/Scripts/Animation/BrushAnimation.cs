using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushAnimation : MonoBehaviour
{
    [SerializeField] private GameManager m_GameManager;
    [SerializeField] private GameObject m_Brush;

    private bool isPlayed;

    void Start()
    {
        isPlayed = false;
        m_Brush.SetActive(isPlayed);
        m_GameManager.UI.m_EventSelectLevel.AddListener(ChangeStatusBrushAnimation);
    }

    private void ChangeStatusBrushAnimation()
    {
        isPlayed = !isPlayed;

        if (isPlayed)
        {
            m_Brush.SetActive(isPlayed);
        }
        else
        {
            m_Brush.SetActive(isPlayed);
        }
    }

}
