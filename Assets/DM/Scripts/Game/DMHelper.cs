using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Zenject;

public class DMHelper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Inject] LevelController m_LevelController;
    [SerializeField] private GameFieldCreation m_GameFieldCreation;
    [SerializeField] private Figures m_Figures;
    [SerializeField] private Transform m_Finger;
    [SerializeField] private Vector2 m_FingerOffset;
    [SerializeField] private Animator m_HelperAnimator;
    [SerializeField] private float m_TimeForShowHelper = 5;

    public static Action onFieldPressedAction;

    private bool m_HelperCanceled = false;
    private int m_SelectedFigure;
    private List<MouseActions> m_MouseActions = new List<MouseActions>();

    void Awake()
    {
        m_Figures = m_GameFieldCreation.Figures;
        m_Finger.gameObject.SetActive(true);
    }

    void Start()
    {
        m_MouseActions = m_GameFieldCreation.CellsPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //OnClickHelper();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //StartCoroutine(StartHelperTimer());
    }

    private void OnEnable()
    {
        m_Figures.onFigurePress += RecalculateHelper;
        StartCoroutine(StartHelperTimer());
    }
    private void OnDisable()
    {
        OnClickHelper();
        m_Figures.onFigurePress -= RecalculateHelper;
    }

    public void StartHelperWithTimer()
    {
        Debug.Log("Start coroutine");
        m_HelperCanceled = false;
        StartCoroutine(StartHelperTimer());
    }

    IEnumerator StartHelperTimer()
    {
        yield return new WaitForSeconds(m_TimeForShowHelper);
        StartHelper();
    }

    void StartHelper()
    {
        m_SelectedFigure = m_Figures.SelectedID;
        foreach (MouseActions cell in m_GameFieldCreation.CellsPos)
        {
            if (!cell.ActivatedCell & m_SelectedFigure == cell.FigureId)
            {
                m_Finger.transform.position = cell.transform.position + (Vector3)m_FingerOffset;
                m_HelperAnimator.Play("HelperMainAnim");
                m_LevelController.HelperCallCount++;
                break;
            }
        }
    }

    public void OnPanelDrag()
    {
        if (!m_HelperCanceled)
        {
            OnClickHelper();
            m_HelperCanceled = true;
        }
    }

    public void OnClickHelper()
    {
        //m_HelperAnimator.Play("HelperOutAnim");
        Debug.Log("Stop coroutine");
        StopAllCoroutines();
        m_HelperAnimator.Play("HelperEmptyState");
    }

    void RecalculateHelper()
    {
        OnClickHelper();
        StartCoroutine(StartHelperTimer());
    }
}
