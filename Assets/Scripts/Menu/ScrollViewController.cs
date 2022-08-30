using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour//, IBeginDragHandler, IDragHandler
{
    [SerializeField] private GameObject[] getPrefabsButton;
    [SerializeField] private float m_VelocityToPass;
    private List<GameObject> m_InstanceButtons;
    //private Button[] m_ButtonsComponent;
    private List<Button> m_ButtonsComponent;
    private const int Step = 100;
    private RectTransform m_ContentRange;
    private ScrollRect m_ScrollRect;
    private Canvas m_Canvas;
    //private Vector2[] m_ButtonPosition;
    private List<Vector2> m_ButtonPosition;

    private float m_AddEdgeLimit;
    //private Vector2[] m_ButtonScale;
    private Vector2[] m_ButtonScale;
    private const int ButtonScaleOffset = 2;
    [SerializeField] private int m_CountButtons;
    [SerializeField] private float m_Padding;
    [SerializeField] private int m_LeftEdgeGame;
    [SerializeField] private int m_DefaultStartButton;
    [SerializeField] private int m_RightEdgeGame;
    //Event e = Event.current;

    private int m_SelectedButtonID = 5;
    private bool m_IsScrolling = false;

    private Vector2 _magnet;
    private const int MagnetSpeed = 10;
    Vector2 vect = default(Vector2);
    private Rect m_TmpRect;

    Vector2 m_StartButtonsScale;
    Vector2 m_CurrentButtonScale;
    Vector2 m_OtherButtonsScale;

    //[SerializeField] private GameObject m_ScroolView;
    private CheckDeltaScroll m_CheckDeltaScroll;
    int m_ChangeIdButton = 0;

    

    public void Scrolling(bool scroll)
    {
        m_IsScrolling = scroll;
        
        
    }
    private void Awake()
    {
        //m_CountButtons = getPrefabsButton.Length;

    }

    private void Start()
    {
        m_CheckDeltaScroll = GetComponentInParent<CheckDeltaScroll>();
        m_ScrollRect = GetComponentInParent<ScrollRect>();
        m_Canvas = GetComponentInParent<Canvas>();
        m_InstanceButtons = new List<GameObject>(m_CountButtons);
        m_ButtonsComponent = new List<Button>(m_CountButtons);
        m_ButtonPosition = new List<Vector2>(m_CountButtons);
        m_ButtonScale = new Vector2 [m_CountButtons];
        m_ContentRange = GetComponent<RectTransform>();
        m_AddEdgeLimit = -(getPrefabsButton[0].GetComponent<RectTransform>().sizeDelta.x / 2);
        Time.timeScale = 1f;

        for (int i = 0; i < m_CountButtons; i++)
        {
            /*if (i == 0)
            {
                m_InstanceButtons[i] = Instantiate(getPrefabsButton[i], transform, false);
            }*/
            m_InstanceButtons.Add(Instantiate(getPrefabsButton[i % 4], transform, false));
            //m_InstanceButtons[i] = Instantiate(getPrefabsButton[i], transform, false);
            
                m_InstanceButtons[i].transform.localPosition = new Vector2(i  *
                (getPrefabsButton[i % 4].GetComponent<RectTransform>().sizeDelta.x + m_Padding), 0);

            /*m_InstanceButtons[i].transform.localPosition = new Vector2(i * 
                (getPrefabsButton[i].GetComponent<RectTransform>().sizeDelta.x + 100), 0);*/
            /*if (i == 0)
            {
                m_InstanceButtons[i].transform.localPosition = new Vector2(-(((getPrefabsButton[i].GetComponent<RectTransform>().sizeDelta.x) / 2)
                    * (m_CountButtons - 1) + (Step * (m_CountButtons - 1) / 2)), 0);
            }
            else
            {
                m_InstanceButtons[i].transform.localPosition = new Vector2(m_InstanceButtons[i - 1].transform.localPosition.x +
                    getPrefabsButton[0].GetComponent<RectTransform>().sizeDelta.x + Step, 0);
            }*/
            m_ButtonPosition.Add(- m_InstanceButtons[i].transform.localPosition);
            m_ButtonScale[i] = m_InstanceButtons[i].transform.localScale;
            m_ButtonsComponent.Add(m_InstanceButtons[i].GetComponent<Button>());
        }

        m_StartButtonsScale = new Vector2(m_InstanceButtons[0].transform.localScale.x, m_InstanceButtons[0].transform.localScale.y);
        m_CurrentButtonScale = new Vector2(m_InstanceButtons[0].transform.localScale.x * 1.1f, m_InstanceButtons[0].transform.localScale.y * 1.1f);
        m_OtherButtonsScale = new Vector2(m_InstanceButtons[0].transform.localScale.x * 0.7f, m_InstanceButtons[0].transform.localScale.y * 0.7f);

        //UpdatePositionAndScale(m_ChangeIdButton);
        m_ContentRange.anchoredPosition = m_ButtonPosition[m_DefaultStartButton];
        
    }

    private void FixedUpdate()
    {
        //if (!_isScrolling) return;
        
        float nearestButton = float.MaxValue;
        /*if (!m_IsScrolling)
        {
            if (m_CheckDeltaScroll.DirState == CheckDeltaScroll.Directions.Right)
            {
                m_SelectedButtonID++;
                _magnet.x = Mathf.SmoothStep(m_ContentRange.anchoredPosition.x, m_ButtonPosition[m_SelectedButtonID].x,
        MagnetSpeed * Time.fixedDeltaTime);
                m_ContentRange.anchoredPosition = _magnet;
                m_CheckDeltaScroll.DirState = CheckDeltaScroll.Directions.None;
            }
            else if (m_CheckDeltaScroll.DirState == CheckDeltaScroll.Directions.Left)
            {
                m_SelectedButtonID--;
                _magnet.x = Mathf.SmoothStep(m_ContentRange.anchoredPosition.x, m_ButtonPosition[m_SelectedButtonID].x,
        MagnetSpeed * Time.fixedDeltaTime);
                m_ContentRange.anchoredPosition = _magnet;
                m_CheckDeltaScroll.DirState = CheckDeltaScroll.Directions.None;
            }
        }*/

        // Next scale up or scale down on scrolling
        for (int i = 0; i < m_CountButtons; i++)
        {
            float distance = Mathf.Abs(m_ContentRange.anchoredPosition.x - m_ButtonPosition[i].x);

            if (distance < nearestButton)
            {
                nearestButton = distance;
                m_SelectedButtonID = i;
            }
            float scaleMin = 0.7f;
            float scaleMax = 1.1f;
            //if (!m_IsScrolling & m_ButtonScale[i].x > scaleMin & m_ButtonScale[i].x <= scaleMax)
            if (!m_IsScrolling & Mathf.Abs(m_ScrollRect.velocity.x) <= m_VelocityToPass)
            {
                float scale = Mathf.Clamp(1 / (distance / Step) * ButtonScaleOffset, scaleMin, scaleMax);
                m_ButtonScale[i].x = Mathf.SmoothStep(m_InstanceButtons[i].transform.localScale.x, scale, 10 * Time.fixedDeltaTime);
                m_ButtonScale[i].y = Mathf.SmoothStep(m_InstanceButtons[i].transform.localScale.y, scale, 10 * Time.fixedDeltaTime);
                // m_ButtonScale[i].x = 1f;
                m_InstanceButtons[i].transform.localScale = m_ButtonScale[i];
            }
            else if (m_IsScrolling || !m_IsScrolling & Mathf.Abs(m_ScrollRect.velocity.x) != 0)
            {
                //float scale = Mathf.Clamp(1 / (distance / Step) * ButtonScaleOffset, scaleMin, scaleMax);
                m_ButtonScale[i].x = Mathf.SmoothStep(m_InstanceButtons[i].transform.localScale.x, scaleMin, 10 * Time.fixedDeltaTime);
                m_ButtonScale[i].y = Mathf.SmoothStep(m_InstanceButtons[i].transform.localScale.y, scaleMin, 10 * Time.fixedDeltaTime);
                // m_ButtonScale[i].x = 1f;
                m_InstanceButtons[i].transform.localScale = m_ButtonScale[i];
            }
        }
        SelectNewPosition();

        // Next magneting to selected game
        if (Mathf.Abs(m_ScrollRect.velocity.x) <= m_VelocityToPass)
        {
            if (m_IsScrolling) return;

            //Debug.Log("after return");
            _magnet.x = Mathf.SmoothStep(m_ContentRange.anchoredPosition.x, m_ButtonPosition[m_SelectedButtonID].x,
                MagnetSpeed * Time.fixedDeltaTime);
            m_ContentRange.anchoredPosition = _magnet;
        }
        //m_InstanceButtons[0].GetComponent<Button>().enabled = false;
        /*for (int i = 0; i < m_CountButtons; i++)
        {
            if (m_SelectedButtonID == i)
            {
                m_ButtonsComponent[i].enabled = true;
            }
            else
            {
                m_ButtonsComponent[i].enabled = false;
            }
        }*/
    }

    public void OnClickButtonScene(int buttonId) //unused for now 
    {
        Debug.Log((buttonId - 1) + "  " + (m_SelectedButtonID % 4));
        if (buttonId - 1 == m_SelectedButtonID % 4)
        {
            Debug.Log((buttonId - 1) + "  " + (m_SelectedButtonID %4));
            Debug.Log(buttonId);
            SceneManager.LoadScene(buttonId);
        }
        else
        {
            if (buttonId - 1 < m_SelectedButtonID % 4 && m_SelectedButtonID % 4 != 0)
            {
                if (buttonId - 1 == 0 && m_SelectedButtonID % 4 == 3)
                {
                    m_ChangeIdButton = 1;
                    //m_SelectedButtonID++;
                }
                else
                {
                    m_ChangeIdButton = -1;
                    Debug.Log((buttonId - 1) + "  " + (m_SelectedButtonID % 4));
                    //m_SelectedButtonID--;
                }
                
            }
            else if (buttonId - 1 == 3 && m_SelectedButtonID % 4 == 0)
            {
                Debug.Log((buttonId - 1) + "  " + (m_SelectedButtonID % 4));
                //m_SelectedButtonID--;
                m_ChangeIdButton = -1;
            }
            else if (buttonId - 1 == 0 && m_SelectedButtonID % 4 == 3)
            {
                Debug.Log((buttonId - 1) + "  " + (m_SelectedButtonID % 4));
                //m_SelectedButtonID++;
                m_ChangeIdButton = 1;
                Debug.Log((buttonId - 1) + "  " + (m_SelectedButtonID % 4));
            }
            else
            {
                Debug.Log((buttonId - 1) + "  " + (m_SelectedButtonID % 4));
                //m_SelectedButtonID++;
                m_ChangeIdButton = 1;
            }
            UpdatePositionAndScale(m_ChangeIdButton);
        }
    }

    /*public void OnBeginDrag(PointerEventData eventData)
    {
        
        if (eventData.delta.x < 0)
        {
            m_SelectedButtonID++;
            
            if (m_SelectedButtonID > m_CountButtons - 1)
            {
                m_SelectedButtonID = m_CountButtons - 1;
            }
        }
        else if (eventData.delta.x > 0)
        {
            m_SelectedButtonID--;
            
            if (m_SelectedButtonID < 0)
            {
                m_SelectedButtonID = 0;
            }
        }
        UpdatePositionAndScale();
        
    }*/



    /*private void CreateNewItem(int id)
    {
        Debug.Log(m_InstanceButtons[0].name + "  " + id);
        string name = m_InstanceButtons[0].name;
        m_InstanceButtons.RemoveAt(0);
        m_InstanceButtons.Add(Instantiate(getPrefabsButton[0], transform, false));
        m_InstanceButtons[m_InstanceButtons.Count - 1].transform.localPosition = new Vector2((m_InstanceButtons.Count - 1) *
            (getPrefabsButton[0].GetComponent<RectTransform>().sizeDelta.x + 100), 0);
        m_ButtonPosition.RemoveAt(0);
        m_ButtonPosition.Add(-m_InstanceButtons[m_InstanceButtons.Count - 1].transform.localPosition);
        m_ButtonsComponent.RemoveAt(0);
        m_ButtonsComponent.Add(m_InstanceButtons[m_InstanceButtons.Count - 1].GetComponent<Button>());
        
    }*/


    public void UpdatePositionAndScale(int diffChangeId)
    {
        m_SelectedButtonID += diffChangeId;
        Debug.Log(m_ContentRange.anchoredPosition.x);
        Debug.Log(m_ButtonPosition[m_SelectedButtonID].x);
        Sequence sequence = DOTween.Sequence();
        //Vector2 sc = new Vector2(button.transform.localScale.x.button.transform.localScale.y);

        //foreach (Button button in m_ButtonsComponent)
        for (int i = 0; i < m_CountButtons; i++)
        {

            /* sc = new Vector2(button.transform.localScale.x, button.transform.localScale.y);
            Vector2 sce = new Vector2(button.transform.localScale.x * 0.6f, button.transform.localScale.y * 0.6f);*/
            if (i == m_SelectedButtonID)
            {
                //m_ButtonsComponent[i].enabled = true;
                sequence.Join(m_InstanceButtons[i].GetComponent<Image>().transform.DOScale(m_CurrentButtonScale, 0.3f));
            }
            else
            {
                //m_ButtonsComponent[i].enabled = false;
                sequence.Join(m_InstanceButtons[i].GetComponent<Image>().transform.DOScale(m_OtherButtonsScale, 0.3f));
            }
            //sequence.Join(button.transform.DOScale(sce, 0.3f));
            /*float scale = Mathf.Clamp(1 / (distance / Step) * ButtonScaleOffset, 0.6f, 1f);
            m_ButtonScale[i].x = Mathf.SmoothStep(m_InstanceButtons[i].transform.localScale.x, scale, 10 * Time.fixedDeltaTime);
            m_ButtonScale[i].y = Mathf.SmoothStep(m_InstanceButtons[i].transform.localScale.y, scale, 10 * Time.fixedDeltaTime);
            m_InstanceButtons[i].transform.localScale = m_ButtonScale[i];*/

        }


        sequence.Join(m_ContentRange.DOAnchorPosX(m_ButtonPosition[m_SelectedButtonID].x, 0.3f).SetEase(Ease.OutCirc));
        sequence.Play().OnComplete(Changd);
        
        //if (m_SelectedButtonID == 2)
        //{
        //    for (int i = 0, j = 0; i < m_CountButtons; i++)
        //    {
        //        if (i < 8)
        //        {
        //            m_InstanceButtons[i].transform.localPosition = new Vector3(m_ButtonPosition[i + 4].x,
        //                m_InstanceButtons[i].transform.localPosition.y,
        //                m_InstanceButtons[i].transform.localPosition.z);
        //        }
        //        else
        //        {
        //            m_InstanceButtons[i].transform.localPosition = new Vector3(m_ButtonPosition[j].x,
        //                m_InstanceButtons[i].transform.localPosition.y,
        //                m_InstanceButtons[i].transform.localPosition.z);
        //            j++;
        //        }

        //    }
        //}
        

        /*if (m_SelectedButtonID == 2)
        {
            
            Debug.Log("Ok");
            m_ContentRange.anchoredPosition = new Vector2(m_ButtonPosition[5].x, m_ContentRange.anchoredPosition.y);
            m_SelectedButtonID = 6;
        }*/
        //DOTween.Play(m_ContentRange);
        //Debug.Log(m_ContentRange.anchoredPosition.x + "   " + m_ButtonPosition[m_SelectedButtonID].x);
    }
    public void Changd()
    {
        SelectNewPosition();
        m_InstanceButtons[m_SelectedButtonID].GetComponent<Image>().transform.DOScale(m_CurrentButtonScale, 0.3f);
    }
    Vector2 tmpPos;
    GameObject tmpObj;
    private void SelectNewPosition()
    {
        /*tmpObj = new GameObject();
        if (m_ContentRange.anchoredPosition.x == m_ButtonPosition[2].x)
        {
            tmpPos = m_InstanceButtons[m_CountButtons - 1].transform.localPosition;
            tmpPos.x = m_InstanceButtons[0].transform.localPosition.x - m_InstanceButtons[m_CountButtons - 1].GetComponent<RectTransform>().sizeDelta.x - 100;
            m_InstanceButtons[m_CountButtons - 1].transform.localPosition = tmpPos;
            
            for (int i = 0; i < m_CountButtons; i++)
            {
                if (i == 0)
                {
                    tmpObj = m_InstanceButtons[m_CountButtons - 1];
                    m_InstanceButtons[i] = tmpObj;
                }
                tmpObj
                m_InstanceButtons[0] = tmpObj;
                tmpObj = 
                //tpmObj = m_InstanceButtons[m_CountButtons - 1];

            }
        //}*/
        //if (m_SelectedButtonID == 2)
        //{
        //    m_ContentRange.anchoredPosition = new Vector2(m_ButtonPosition[6].x, m_ContentRange.anchoredPosition.y);
        //    m_SelectedButtonID = 6;
        //}
        //else if (m_SelectedButtonID == 9)
        //{
        //    //Debug.Log("Ok");
        //    m_ContentRange.anchoredPosition = new Vector2(m_ButtonPosition[5].x, m_ContentRange.anchoredPosition.y);
        //    m_SelectedButtonID = 5;
        //}

        if (m_ContentRange.anchoredPosition.x >= m_ButtonPosition[m_LeftEdgeGame].x - m_AddEdgeLimit)
        {
            Debug.Log("LeftEdge " + m_ButtonPosition[m_LeftEdgeGame].x + m_AddEdgeLimit);
            m_ContentRange.anchoredPosition = new Vector2(m_ButtonPosition[m_RightEdgeGame].x -
                m_AddEdgeLimit, m_ContentRange.anchoredPosition.y);
            //m_SelectedButtonID = 6;
        }
        else if (m_ContentRange.anchoredPosition.x <= m_ButtonPosition[m_RightEdgeGame].x + m_AddEdgeLimit)
        {
            Debug.Log("RightEdge " + m_ButtonPosition[m_RightEdgeGame].x + m_AddEdgeLimit);
            //Debug.Log("Ok");
            m_ContentRange.anchoredPosition = new Vector2(m_ButtonPosition[m_LeftEdgeGame].x +
                m_AddEdgeLimit, m_ContentRange.anchoredPosition.y);
            //m_SelectedButtonID = 5;
        }
    }

    
    /*public void OnDrag(PointerEventData eventData)
    {
        //vect.x += eventData.delta.x / m_Canvas.scaleFactor;
        //m_ContentRange.anchoredPosition = vect;
        //UpdatePositionAndScale();
    }*/

    /*public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.delta.x < 0)
        {
            Debug.Log("right");
        }
        else if (eventData.delta.x > 0)
        {
            Debug.Log("left");
        }

    }*/


}
