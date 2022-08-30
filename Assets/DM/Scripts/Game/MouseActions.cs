using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;
using Zenject;

public class MouseActions : MonoBehaviour
{
    [Inject] LevelController m_LevelController;
    [SerializeField] private string m_CorrectCellSound;
    [SerializeField] private string m_WrongCellSound;

    private Figures m_Figures;
    private int m_DestinationFigureId;
    public int FigureId => m_DestinationFigureId;
    private bool m_ActivatedCell;
    public bool ActivatedCell => m_ActivatedCell;
    private Animator m_CellAnimations;
    private SpriteRenderer m_Renderer;

    private void Awake()
    {
        //Initialize(0); //TODO replace to template Hierarchy
        Initialize();
    }

    public void Initialize()//No Invoke
    {
        m_Renderer = GetComponent<SpriteRenderer>();
        m_CellAnimations = GetComponent<Animator>();
        //this.cellId = cellId; //TODO cellId get {}; private set;
    }

    public void Init(LevelController levelController,
        Figures figureObj,
        int figureId,
        bool activatedCell)
    {
        m_LevelController = levelController;
        m_Figures = figureObj;
        m_DestinationFigureId = figureId;
        m_ActivatedCell = activatedCell;
    }

    private void OnMouseDown()
    {
        ReplaceFigure();
    }

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0))
        {
            ReplaceFigure();
        }
    }

    private void ReplaceFigure()
    {
        if (m_DestinationFigureId == m_Figures.SelectedID && m_Figures.SelectedID > 0)
        {
            if (m_ActivatedCell == false)
            {
                CorrectCellActions();
                for (int i = 0; i < m_Figures.FigureScore.Length; i++)
                {
                    if (m_Figures.FiguresId[i] == m_DestinationFigureId)
                    {
                        m_Figures.FigureScore[i] += 1;
                        if ( m_Figures.FigureGoalScore[i] == m_Figures.FigureScore[i])
                        {
                            // Actions for one figure over
                            m_Figures.DisableGoaledFigure(m_Figures.FiguresId[i]);
                        }
                    }
                }
                m_Figures.CommonScore += 1;
                if (m_Figures.CommonScore == m_Figures.CommonScoreGoal)
                {
                    // Complete Level
                    m_LevelController.LevelComplete();
                    //TODO Complete level sound
                }
                m_ActivatedCell = true;
            }
        }
        else if (m_Figures.SelectedID > 0 && m_DestinationFigureId > 0)
        {
            // Wrong cell actions
            WrongCellActions();
        }
    }

    private void CorrectCellActions()
    {
        m_Renderer.sprite = m_Figures.FiguresData[m_DestinationFigureId].ColoredFigureSprite;
        m_CellAnimations.Play("CorrectCell");
        HapticFeedback.LightFeedback();
        SoundPlayer.instance.Play(m_CorrectCellSound, 1);
    }

    private void WrongCellActions()
    {
        m_CellAnimations.Play("WrongCell");
        SoundPlayer.instance.Play(m_WrongCellSound, 1);
    }
}
