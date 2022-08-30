using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grid_controller : MonoBehaviour
{
    [SerializeField] private Sprite m_premium;
    [SerializeField] private Sprite m_complete;
    [SerializeField] private StickersCounter m_stickersCounter;
    [SerializeField] private HelperCounter m_helperCounter;

    private Picture_manager m_picture_Manager;

    private Level_prefab[] m_levels;
    private GridLayoutGroup m_gridLayoutGroup;
    private RectTransform m_rectTransform;
    private DateTime m_StartTime;

    private List<int> m_save_data;

    Vector3 cellSize;

    void Awake()
    {
        if (m_picture_Manager == null)
        {
            m_picture_Manager = FindObjectOfType<Picture_manager>();
            if (m_picture_Manager == null) Debug.LogError("Level prefab: picture_Manager == null");
        }

        m_rectTransform = GetComponent<RectTransform>();

        m_levels = GetComponentsInChildren<Level_prefab>();

        m_gridLayoutGroup = GetComponent<GridLayoutGroup>();

        cellSize = m_gridLayoutGroup.cellSize;

        LoadLCdata();

        var set = new HashSet<int>();
        foreach (var item in m_levels)
            if (!set.Add(item.GetLevelId()))
                Debug.LogError("Level repeat detected! Level id: " + item.GetLevelId());
    }

    void Start()
    {
        SetSizeGrid();

        for (int i = 0; i < m_levels.Length; i++)
            UpdateFrame(i);
    }

    private void SetSizeGrid()
    {
        float colums = (m_levels.Length + m_levels.Length % m_gridLayoutGroup.constraintCount) / m_gridLayoutGroup.constraintCount;

        float h = colums * m_gridLayoutGroup.cellSize.x + (colums - 1) * m_gridLayoutGroup.spacing.x + // grid zone
            m_gridLayoutGroup.cellSize.x; // more space

        float w = m_gridLayoutGroup.cellSize.y * m_gridLayoutGroup.constraintCount + m_gridLayoutGroup.spacing.y * (m_gridLayoutGroup.constraintCount - 1) +
            m_gridLayoutGroup.cellSize.y; // more space

        m_rectTransform.sizeDelta = new Vector2(h, w);
        m_rectTransform.anchoredPosition = new Vector2(h / 2, 0);

    }

    public Level_prefab GetLevel(int id)
    {
        return m_levels[id]; 
    }

    public int levels_size()
    {
        return m_levels.Length;
    }

    private void LoadLCdata()
    {
        m_save_data = LC_Saver.LoadLC();

        if (m_save_data == null)
        {
            m_save_data = new List<int>();
            return;
        }


        List<Level_prefab> tmp = new List<Level_prefab>(m_levels);
        for (int i = 0; i < m_save_data.Count; i++)
        {
            bool isFind = false;

            for (int j = 0; j < tmp.Count; j++)
            {
                if (tmp[j].GetLevelId() == m_save_data[i])
                {
                    tmp[j].complete = true;
                    tmp.RemoveAt(j);
                    isFind = true;
                    break;
                }
            }

            if (!isFind)
                Debug.LogError("[LoadData] Lost level: " + m_save_data[i]);
        }
    }

    public void SaveLCData(int level_index)
    {
        for (int i = 0; i < m_save_data.Count; i++)
        {
            if (m_save_data[i] == level_index)
                return;
        }


        m_save_data.Add(level_index);
        SaveLCData();
    }

    public void SaveLCData()
    {
        LC_Saver.SaveLC(m_save_data);
    }

    public void CompleteLevel(int index)
    {
        if (m_levels[index].complete)
        {
            return;
        }

        // TODO: Helper counter
        TimeSpan timer = DateTime.Now.Subtract(m_StartTime);
        int time = (timer.Hours * 60 * 60) + (timer.Minutes * 60) + timer.Seconds;

        m_stickersCounter.UpdateLevelsStickers(2, index, m_helperCounter.NumberOfCalls, time);
        m_levels[index].complete = true;
        UpdateFrame(index);
    }

    private void UpdateFrame(int index)
    {
        Grid_SizeFix grid_SizeFix = m_levels[index].GetComponent<Grid_SizeFix>();

        if (!Purchaser.isPremium())
        {
            if (m_levels[index].premium)
            {
                grid_SizeFix.SetFrame(m_premium);
            }
            else
            {
                if (m_levels[index].complete)
                {
                    grid_SizeFix.SetFrame(m_complete);
                    m_levels[index].UnlockGrid();
                }
            }
        }
        else
        {
            if (m_levels[index].complete)
            {
                grid_SizeFix.SetFrame(m_complete);
                m_levels[index].UnlockGrid();
            }
        }

        grid_SizeFix.Resize(cellSize.x, cellSize.y);
    }

    public void ClearCounter()
    {
        m_StartTime = DateTime.Now;
        m_helperCounter.SetZeroCount();
    }

    public void LevelAborted()
    {
        AnalyticsHelper.GetInstance().SendLevelAborted(2, m_picture_Manager.GetActiveLevelId());
    }
}
