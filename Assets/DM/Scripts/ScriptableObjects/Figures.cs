using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CandyCoded.HapticFeedback;
using Zenject;

public class Figures : MonoBehaviour
{
    [Inject] LevelController m_LevelController;
    [SerializeField] private FiguresData[] m_FiguresData;
    [SerializeField] private GameObject[] m_FiguresButton;

    [Header("Mascot")]
    [SerializeField] private SpriteRenderer m_MascotIndicatorSprite;
    [SerializeField] private Animator m_MascotAnimator;

    [Header("Other")]
    [SerializeField] private string m_SoundName;
    [SerializeField] private int m_CommonScore;
    [SerializeField] private int m_CommonScoreGoal;
    [SerializeField] private int[] m_FigureScore;
    [SerializeField] private int[] m_FiguresId;
    [SerializeField] private int[] m_FigureGoalScore;

    private DoTween m_DoTween;
    private int m_SelectedId;
    private bool[] m_IsActiveFigure;
    private bool m_IsFirstStart = true;

    public Action onFigurePress;

    private void Awake()
    {
        m_DoTween = GetComponent<DoTween>();
        //FigureDataInit();
    }

    private void OnEnable()
    {
        onFigurePress += SetActiveFigure;
    }

    private void OnDisable()
    {
        onFigurePress -= SetActiveFigure;
    }

    public void EnableUsedFigures(bool newGame)
    {
        if (newGame)
        {
            // Play Button
            ResetScores(newGame);
            if (m_IsFirstStart)
            {
                for (int i = 0; i < m_LevelController.Levels.FiguresGoalCount.Length; i++)
                {
                    m_CommonScoreGoal += m_LevelController.Levels.FiguresGoalCount[i];
                    m_FigureScore = new int[m_LevelController.Levels.UsedFigures.Length];
                }
                m_IsFirstStart = false;
            }
        }
        else
        {
            // Next Level Button
            ResetScores(newGame);
            for (int i = 0; i < m_LevelController.Levels.FiguresGoalCount.Length; i++)
            {
                m_CommonScoreGoal += m_LevelController.Levels.FiguresGoalCount[i];
                m_FigureScore = new int[m_LevelController.Levels.UsedFigures.Length];
            }
        }

        for (int i = 0; i < m_FiguresButton.Length; i++)
        {
            // Reset used figures to default values
            m_FiguresButton[i].transform.localScale = Vector2.one;
            m_FiguresButton[i].GetComponent<Image>().color = Color.white;
            m_FiguresButton[i].SetActive(false);
        }

        for (int i = 0; i < m_LevelController.Levels.UsedFigures.Length; i++)
        {
            for (int j = 0; j < m_FiguresButton.Length; j++)
            {
                if (m_LevelController.Levels.UsedFigures[i] == j)
                {
                    // Enable figures
                    if (m_FigureScore[i] != m_FigureGoalScore[i])
                    {
                        int tempFigure = m_LevelController.Levels.UsedFigures[i];
                        m_FiguresButton[tempFigure].SetActive(true);
                    }
                }
            }
        }
        // Activate first active figure on start
        InitilizeFiguresBools();
        SelectActiveFigure();
    }

    private void ResetScores(bool newGame)
    {
        //figureScore = new int[levelController.Levels.UsedFigures.Length];
        m_FiguresId = new int[m_LevelController.Levels.UsedFigures.Length];
        m_FiguresId = m_LevelController.Levels.UsedFigures;
        m_FigureGoalScore = new int[m_LevelController.Levels.FiguresGoalCount.Length];
        m_FigureGoalScore = m_LevelController.Levels.FiguresGoalCount;
        if (!newGame)
        {
            m_CommonScore = 0;
            m_CommonScoreGoal = 0;
        }
    }

    public void OnFigureClick(FiguresData value)
    {
        if (m_SelectedId != value.FigureId)
        {
            m_SelectedId = value.FigureId;
            onFigurePress?.Invoke();
        }
        // m_MascotIndicatorSprite.sprite = value.MascotSprite;
    }

    private void SetActiveFigure()
    {
        for (int i = 0; i < m_FiguresButton.Length; i++)
        {
            if (m_FiguresButton[i].activeSelf == true && i == m_SelectedId)
            {
                // Selected Figure
                m_DoTween.SelectedFigure(m_FiguresButton[i].transform);
                SoundPlayer.instance.Play(m_SoundName, 1);
                HapticFeedback.LightFeedback();
                m_MascotAnimator.Play("colorchange.dm_mascot_colorchange");
                m_MascotIndicatorSprite.sprite = m_FiguresData[i].MascotSprite;
            }
            else if (m_FiguresButton[i].activeSelf == true && i != m_SelectedId)
            {
                // Deselect Figure
                m_DoTween.DeselectedFigure(m_FiguresButton[i].transform);
            }
        }
    }

    public void DisableGoaledFigure(int figureId)
    {
        StartCoroutine(DisableFigureCourutine(figureId));
        m_IsActiveFigure[figureId] = false;
        SelectActiveFigure();
    }

    public IEnumerator DisableFigureCourutine(int courutineFigureId)
    {
        m_DoTween.FadeOutAnimation(m_FiguresButton[courutineFigureId].transform,
            m_FiguresButton[courutineFigureId].GetComponent<Image>());
        yield return new WaitForSeconds(0.5f); // Value hardcoded, need to change
        m_FiguresButton[courutineFigureId].SetActive(false);
    }

    private void SelectActiveFigure()
    {
        for (int i = 0; i < m_FiguresButton.Length; i++)
        {
            //if (m_FiguresButton[i].activeSelf)
            if (m_IsActiveFigure[i])
            {
                m_SelectedId = i;
                i = m_FiguresButton.Length;
            }
        }
        SetActiveFigure();
    }

    private void InitilizeFiguresBools()
    {
        m_IsActiveFigure = new bool[m_FiguresButton.Length];
        for (int i = 0; i < m_FiguresButton.Length; i++)
        {
            if (m_FiguresButton[i].activeSelf)
            {
                m_IsActiveFigure[i] = true;
            }
            else
            {
                m_IsActiveFigure[i] = false;
            }
        }
    }

    public int SelectedID
    {
        get => m_SelectedId;
        set => m_SelectedId = value;
    }

    public int CommonScore
    {
        get => m_CommonScore;
        set => m_CommonScore = value;
    }

    public int CommonScoreGoal
    {
        get => m_CommonScoreGoal;
        set => CommonScoreGoal = value;
    }

    public int[] FigureScore
    {
        get => m_FigureScore;
        set => m_FigureScore = value;
    }

    public int[] FiguresId
    {
        get => m_FiguresId;
        set => m_FiguresId = value;
    }

    public int[] FigureGoalScore
    {
        get => m_FigureGoalScore;
        set => m_FigureGoalScore = value;
    }

    public FiguresData[] FiguresData
    {
        get => m_FiguresData;
        set => m_FiguresData = value;
    }
}
