using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class StickersCounter : MonoBehaviour
{
    [Inject] private SaveLevelsComplete m_LevelsComplete;

    [SerializeField] private CompleteStickers m_PanelStickers;

    [SerializeField] private int m_StickersCount = 4;

    private int m_CountLevels;

    private void Awake()
    {
        m_LevelsComplete.LoadCountLevels();
        m_CountLevels = m_LevelsComplete.m_LevelsComplete;
    }

    /// <summary>
    /// Gkjhsdfljkgb
    /// </summary>
    /// <param name="modeID">Game mode ID (ProgressFC = 0, ProgressFP = 1, ProgressLC = 2, ProgressDM = 3)</param>
    /// <param name="levelID">Level ID</param>
    /// <param name="countOfHelper">How many times the helper was activated?</param>
    /// <param name="time">How many times has the player already run this level? In seconds/</param>
    public void UpdateLevelsStickers(int modeID, int levelID, int countOfHelper, int time)
    {
        m_LevelsComplete.m_LevelsComplete++;
        m_LevelsComplete.SaveCountLevels();
        if (m_LevelsComplete.m_LevelsComplete % m_StickersCount == 0)
        {
            Debug.Log(m_PanelStickers == null);
            m_PanelStickers.SetCurrentSticker(m_LevelsComplete.m_LevelsComplete / m_StickersCount);
            AnalyticsHelper.Instance.SendLevelFinished(modeID, levelID, true, countOfHelper, time);
        }
        else
        {
            AnalyticsHelper.Instance.SendLevelFinished(modeID, levelID, false, countOfHelper, time);
        }
    }
}
