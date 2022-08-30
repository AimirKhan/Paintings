using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Picture_manager : MonoBehaviour
{
    private Board_controller m_board;
    private LineRenderer m_line;
    private Star_controller m_starList;
    private ClickZone_controller m_clickZone;

    [SerializeField] private Animator m_starPrefab;

    private int printedLineIndex;
    private bool isPrintAnim;
    private Level_prefab m_level;
    private float lineScaler;
    private Grid_controller gridController;
    private Screen_manager screenManager;

    [Header("Star List")]
    [SerializeField] private List<int> starsIndex;

    [SerializeField] private Button m_BtPremium;


    void Awake()
    {
        if (m_board == null)
        {
            m_board = GameObject.FindGameObjectWithTag("board").GetComponent<Board_controller>();
        }

        m_line = m_board.GetComponentInChildren<LineRenderer>();
        if (m_line == null) Debug.LogError("Picture_manager: No Line Renderer");

        m_starList = m_board.GetComponentInChildren<Star_controller>();
        if (m_starList == null) Debug.LogError("Picture_manager: No star list");

        m_clickZone = FindObjectOfType<ClickZone_controller>();
        if (m_clickZone == null) Debug.LogError("Picture_manager: No click zone");

        gridController = FindObjectOfType<Grid_controller>();
        if (gridController == null) Debug.LogError("Picture_manager: No grid Controller");

        screenManager = GetComponent<Screen_manager>();
        if (screenManager == null) Debug.LogError("Picture_manager: No Screen_manager");
    }

    void Clear()
    {
        m_line.loop = false;

        if (m_level != null)
            m_level.Clear();

        m_line.positionCount = 0;
        lineScaler = 0;
        printedLineIndex = 0;
        m_board.Unlock_picture(false);
        m_clickZone.SetActiveZone();

        foreach (Transform child in m_starList.transform)
            Destroy(child.gameObject);
    }

    public void LoadLevel(Level_prefab newLevel)
    {
        Clear();

        m_level = newLevel;
        m_level.Load();

        calc_lineScaler();

        LoadStars();
        m_starList.StartGame();

        m_board.GetLocked().sprite = m_level.GetLocked();
        m_board.GetUnlocked().sprite = m_level.GetUnlocked();

        m_board.Back_open(m_level.GetBackground());

        m_board.Resize();

        gridController.ClearCounter();

        AnalyticsHelper.GetInstance().SendLevelStarted(2, m_level.GetLevelId());
    }

    private void calc_lineScaler()
    {
        Vector2 lineSize = m_level.GetJsonSize();
        float scaleW = m_board.getCameraGridSize() / lineSize.x;
        float scaleH = m_board.getCameraGridSize() / lineSize.y;

        if (scaleW <= scaleH) lineScaler = scaleW;
        else lineScaler = scaleH;

        if (lineSize.x > lineSize.y)
        {
            Vector2 mainCamera = m_board.getCameraResolution();

            if (lineSize.x / lineSize.y < mainCamera.x / mainCamera.y) lineScaler *= lineSize.x / lineSize.y;
            else lineScaler *= mainCamera.x / mainCamera.y;
        }

        lineScaler *= m_board.getGameScale();
    }

    public void LoadStars()
    {
        m_starList.Clear();
        starsIndex = m_level.GetStarList();

        for (int i = 0; i < starsIndex.Count; i++)
        {
            m_starList.Add(
                Instantiate(m_starPrefab, m_starList.transform),
                m_level.getLine(starsIndex[i], lineScaler),
                starsIndex[i]);
        }
    }

    public void PrintLine()
    {
        PrintLine(m_level.GetSize(), true);
    }

    public void PrintLine(int end, bool win = false)
    {
        if (isPrintAnim)
            return;

        StartCoroutine(PrintLineAnim(end, win));
    }

    IEnumerator PrintLineAnim(int end, bool win = false)
    {
        m_clickZone.SetActiveZone(false);
        isPrintAnim = true;
        // TODO отрисовка в процентах

        for (; printedLineIndex < end; printedLineIndex += 15)
        {
            m_line.positionCount++;
            m_line.SetPosition(m_line.positionCount - 1, m_level.getLine(printedLineIndex, lineScaler));
            yield return new WaitForSeconds(0.01f);
        }

        printedLineIndex = end;

        if (win)
        {
            m_line.loop = true;
            yield return new WaitForSeconds(1f);

            Win();
        }
        else
        {
            m_clickZone.SetActiveZone();
        }

        isPrintAnim = false;
    }

    private void Win()
    {
        m_board.Unlock_picture();
        screenManager.OpenEndScreen();
        gridController.SaveLCData(m_level.GetLevelId());
        gridController.CompleteLevel(m_level.GetLevelId());
    }

    public bool GetIsPrintAnim()
    {
        return isPrintAnim;
    }

    public void ReloadLevel()
    {
        LoadLevel(gridController.GetLevel(m_level.GetLevelIndex()));
    }

    public void LoadNext()
    {
        if (m_level.GetLevelIndex() + 1 < gridController.levels_size())
        {
            if (!Purchaser.isPremium())
            {
                if (gridController.GetLevel(m_level.GetLevelIndex() + 1).premium)
                {
                    OpenLockedLevel();
                    return;
                }
            }
            LoadLevel(gridController.GetLevel(m_level.GetLevelIndex() + 1));
        }
        else
        {
            LoadLevel(gridController.GetLevel(0));
        }
    }

    public int GetActiveLevelId()
    {
        if (m_level != null)
        {
            return m_level.GetLevelId();
        }

        return -1;
    }

    public void StressTest(int count)
    {
        var rand = new System.Random();

        for (int i = 0; i < count; i++)
        {
            screenManager.CloseGrid();
            LoadLevel(gridController.GetLevel(rand.Next(gridController.levels_size())));
            m_board.Back_close();
            screenManager.OpenGrid();
        }
    }

    public void DebugGetLevelJson()
    {
        m_level.DebugGetJson();
    }

    public void OpenLockedLevel()
    {
        screenManager.HomeBtClick();
        m_board.Back_close();
        m_BtPremium.onClick.Invoke();
    }
}


