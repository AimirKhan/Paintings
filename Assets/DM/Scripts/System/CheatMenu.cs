using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Zenject;

public class CheatMenu : MonoBehaviour
{
    [Inject] SaveSerial m_SaveSerial;
    [SerializeField] private LevelSelectorController m_LevelSelectorController;
    [SerializeField] private RectTransform m_GalleryParent;
    [SerializeField] private float m_CheatMenuShowSpeed = 1f;
    public RectTransform m_StartPos;
    public RectTransform m_EndPos;
    private bool m_CheatMenuEnabled = false;

    public void ClearSaveLevels()
    {
        // Set current level to first
        m_SaveSerial.currentLevelToSave = 0;
        // Set level status resolved to unresolved
        for (int i = 0; i < m_SaveSerial.passedLevelsToSave.Length; i++)
        {
            if (m_SaveSerial.passedLevelsToSave[i] == 1)
            {
                m_SaveSerial.passedLevelsToSave[i] = 0;
            }
        }
        m_SaveSerial.SaveGame(); // Save changes
        foreach(Transform childObject in m_LevelSelectorController.LevelsContainer.transform)
        {
            Destroy(childObject.gameObject);
        }
        m_LevelSelectorController.Initilize();
    }

    public void ShowHideCheatMenu()
    {
        if (m_CheatMenuEnabled)
        {
            m_CheatMenuEnabled = false;
            m_GalleryParent.DOAnchorPos(m_StartPos.localPosition, m_CheatMenuShowSpeed);
        }
        else
        {
            m_CheatMenuEnabled = true;
            m_GalleryParent.DOAnchorPos(m_EndPos.localPosition, m_CheatMenuShowSpeed);
        }
    }
}
