using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CheckDeltaScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private ScrollViewController m_scrollViewController;

    //private ScrollRect m_ScrollRect;
    private float m_StartPos;
    private float m_EndPos;
    private float m_Diff;

    private void Start()
    {
        //m_ScrollRect = GetComponent<ScrollRect>();
        m_scrollViewController = GetComponentInChildren<ScrollViewController>();
    }

    /*public enum Directions
    {
        Right,
        Left,
        None,
        Stand
    }

    public Directions DirState { get; set; }*/

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_StartPos = eventData.position.x;
        //Debug.Log(m_StartPos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
        /*if (eventData.delta.x < 0)
        {
            DirState = Directions.Right;
            //Debug.Log("right");
        }
        else if (eventData.delta.x > 0)
        {
            DirState = Directions.Left;
            //Debug.Log("left");
        }
        else
        {
            DirState = Directions.None;
            //Debug.Log("stop");
        }*/
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_EndPos = eventData.position.x;
        //Debug.Log(m_EndPos);
        m_Diff = Mathf.Abs(m_StartPos) - Mathf.Abs(m_EndPos);
        Debug.Log(m_Diff + "Diff");
        if (m_Diff > -280 && m_Diff < 0)
        {
            m_scrollViewController.UpdatePositionAndScale(-1);
        }
        else if (m_Diff < 250 && m_Diff > 0)
        {
            m_scrollViewController.UpdatePositionAndScale(1);
        }
    }
}
