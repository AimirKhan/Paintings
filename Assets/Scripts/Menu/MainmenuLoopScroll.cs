using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mainmenu
{
    [RequireComponent(typeof(ScrollRect))]
    public class MainmenuLoopScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Canvas m_Canvas;
        [SerializeField] private int m_Spacing;
        [SerializeField] private bool m_IsVertical;
        [Header("Alignment")]
        [SerializeField] private float m_VelocityThreshold = 300;
        [SerializeField] private float m_AlignSpeed = 10f;
        [Header("Scaling games cells")]
        //[SerializeField] private float m_MinCellScale = .7f;
        //[SerializeField] private float m_MaxCellScale = 1f;
        private bool m_IsDragging = false;
        private bool m_IsPositiveDrag = true;
        private bool m_IsMagnited = true;

        private float m_PixelPerUnit = 100;
        // control loop scroll
        private float m_Velocity;
        private ScrollRect m_ScrollRect;
        private float m_LastPosX;
        private float m_LastPosY;

        private float m_NewLastPosX;
        private float m_NewLastPosY;
        private Transform centerItem;
        private Vector2 m_TopPosition;
        private Vector2 m_BottomPosition;
        private Vector2 m_LeftPosition;
        private Vector2 m_RightPosition;
        private Vector2 m_ScreenSize;

        // init content
        private float m_ChildHeight;
        private float m_ChildWidth;
        private Transform[] m_Childs;

        public ScrollRect ScrollRect => m_ScrollRect;
        public UnityAction<Transform> OnColorSelected;

        protected void Start()
        {
            Initilize();
        }

        private void Initilize()
        {
            m_PixelPerUnit = m_Canvas != null ? m_PixelPerUnit = m_Canvas.referencePixelsPerUnit : m_PixelPerUnit;
            m_ScrollRect = GetComponent<ScrollRect>();
            m_ScreenSize = m_Canvas.GetComponent<RectTransform>().sizeDelta;
            m_ScrollRect.movementType = ScrollRect.MovementType.Unrestricted;
            InitializeContent();
            InitPosThreshold();
            int childCount = m_ScrollRect.content.childCount;
            for (int i = 0; i < childCount; i++)
            {
                m_Childs[i] = m_ScrollRect.content.GetChild(i);
            }
        }

        protected void OnEnable()
        {
            OnColorSelected += MoveItem;
        }
        protected void OnDisable()
        {
            OnColorSelected -= MoveItem;
        }
        private void MoveItem(Transform transform)
        {
            if (m_IsVertical)
            {
                if (transform.localPosition.y > 0)
                {
                    m_IsPositiveDrag = false;
                    m_ScrollRect.velocity = new Vector2(0, -transform.position.y);
                }
                else
                {
                    m_IsPositiveDrag = true;
                    m_ScrollRect.velocity = new Vector2(0, -transform.position.y);
                }
            }
            else
            {
                if (transform.localPosition.x > 0)
                {
                    m_IsPositiveDrag = false;
                    m_ScrollRect.velocity = new Vector2(-transform.position.x, 0);
                }
                else
                {
                    m_IsPositiveDrag = true;
                    m_ScrollRect.velocity = new Vector2(-transform.position.y, 0);
                }
            }

        }
        /*
        private IEnumerator ChangeVelocity(Transform transform)
        {
            yield return new WaitForSeconds(0.5f);
            int sibling = transform.GetSiblingIndex();
            for (int i = 0; i < _scrollRect.content.childCount; i++)
            {
                var transformChild = _scrollRect.content.GetChild(i);
                transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            }

        }
        */
        public void OnBeginDrag(PointerEventData eventData)
        {
            m_IsDragging = true;
            m_IsMagnited = false;

            if (m_IsVertical)
            {
                m_LastPosY = eventData.position.y;
            }
            else
            {
                m_LastPosX = eventData.position.x;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Replace this to scrollrect tracking
            /*
            if (m_IsVertical)
            {
                if (eventData.position.y > m_LastPosY)
                    m_IsPositiveDrag = true;
                else
                    m_IsPositiveDrag = false;
                m_LastPosY = eventData.position.y;
            }
            else
            {
                if (eventData.position.x > m_LastPosX)
                    m_IsPositiveDrag = true;
                else
                    m_IsPositiveDrag = false;
                m_LastPosX = eventData.position.x;
            }
            */
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_IsDragging = false;
            AlignToCenter();
        }

        public void OnScrollViewChanged()
        {
            ScalingElements();
            int currItemIndex = m_IsPositiveDrag ? 0 : m_ScrollRect.content.childCount - 1; //first or last 
            var currItem = m_ScrollRect.content.GetChild(currItemIndex);

            if (!ReachedThreshold(currItem))
                return;
            int endItemIndex = m_IsPositiveDrag ? m_ScrollRect.content.childCount - 1 : 0;
            Transform endItem = m_ScrollRect.content.GetChild(endItemIndex);
            Vector2 newPos = endItem.localPosition;

            if (m_IsPositiveDrag)
            {
                if (m_IsVertical)
                {
                    newPos.y -= m_ChildHeight + m_Spacing;
                }
                else
                {
                    newPos.x -= m_ChildWidth + m_Spacing;
                }
            }
            else
            {
                if (m_IsVertical)
                {
                    newPos.y += m_ChildHeight + m_Spacing;
                }
                else
                {
                    newPos.x += m_ChildWidth + m_Spacing;
                }
            }

            currItem.localPosition = newPos;
            currItem.SetSiblingIndex(endItemIndex);
        }

        public void AlignToCenter()
        {
            //TODO Here Zoom In/Out relative of screen position

            m_Velocity = m_IsVertical ? Mathf.Abs(m_ScrollRect.velocity.y) :
                                        Mathf.Abs(m_ScrollRect.velocity.x);
            if (m_VelocityThreshold > m_Velocity && !m_IsDragging)
            {
                // Here need to exclude teleportation
                if (m_ScrollRect.content.childCount % 2 == 0 && !m_IsMagnited)
                {
                    int currNewItemIndex = m_IsPositiveDrag ? m_ScrollRect.content.childCount / 2 - 1
                                                            : m_ScrollRect.content.childCount / 2;
                    centerItem = m_ScrollRect.content.GetChild(currNewItemIndex);
                    m_IsMagnited = true;
                }
                else if (!m_IsMagnited)
                {
                    int currNewItemIndex = m_IsPositiveDrag ? m_ScrollRect.content.childCount / 2
                                                            : m_ScrollRect.content.childCount / 2;
                    centerItem = m_ScrollRect.content.GetChild(currNewItemIndex);
                    m_IsMagnited = true;
                }
                // Need to change to tweener
                Vector2 contentMove = new Vector2(Mathf.SmoothStep(ScrollRect.content.anchoredPosition.x,
                    -centerItem.localPosition.x, m_AlignSpeed * Time.fixedDeltaTime),
                    ScrollRect.content.anchoredPosition.y);
                ScrollRect.content.anchoredPosition = contentMove;
            }
        }
        public void IsPositiveDrag()
        {
            Vector2 currentPos = ScrollRect.content.anchoredPosition;
            if (m_IsVertical)
            {
                if (currentPos.y > m_NewLastPosY)
                    m_IsPositiveDrag = true;
                else
                    m_IsPositiveDrag = false;
                m_NewLastPosY = currentPos.y;
            }
            else
            {
                if (currentPos.x > m_NewLastPosX)
                    m_IsPositiveDrag = true;
                else
                    m_IsPositiveDrag = false;
                m_NewLastPosX = currentPos.x;
            }
        }

        private void ScalingElements()
        {
            //m_Childs
            //print("Pos " + m_Childs[0].localPosition);
            //print("Screen size " + m_ScreenSize);

            foreach(Transform child in m_Childs)
            {
                //child.localScale = 
            }
        }

        private bool ReachedThreshold(Transform item)
        {
            if (m_IsVertical)
            {
                return m_IsPositiveDrag ? item.position.y > m_TopPosition.y : item.position.y < m_BottomPosition.y;
            }
            else
            {
                return m_IsPositiveDrag ? item.position.x > m_RightPosition.x : item.position.x < m_LeftPosition.x;
            }
        }

        private void InitializeContent()
        {
            m_Childs = new Transform[m_ScrollRect.content.childCount];
            var startPos = (transform as RectTransform).rect.max;
            Debug.Log("Start pos: " + startPos);
            for (int i = 0; i < m_Childs.Length; i++)
            {
                var childTransform = (RectTransform)m_ScrollRect.content.transform.GetChild(i);
                m_Childs[i] = childTransform;
                childTransform.anchoredPosition = new Vector2(0.5f, 0.5f);
                m_ChildWidth = childTransform.rect.width;
                m_ChildHeight = childTransform.rect.height;
                if (m_IsVertical)
                {
                    childTransform.localPosition = new Vector2(0f, startPos.y - (m_ChildHeight + m_Spacing) * i);
                }
                else
                {
                    childTransform.localPosition = new Vector2(startPos.x - (m_ChildWidth + m_Spacing) * i, 0f);
                }
            }
        }

        private void InitPosThreshold()
        {
            var width = (transform as RectTransform).rect.width / m_PixelPerUnit / 2;
            var height = ((transform as RectTransform).rect.height / m_PixelPerUnit) / 2;
            if (m_IsVertical)
            {
                height = Mathf.FloorToInt(height);
                m_TopPosition = new Vector2(transform.position.x, transform.position.y + height + 0.5f);
                m_BottomPosition = new Vector2(transform.position.x, transform.position.y - height - 0.5f);
            }
            else
            {
                width = Mathf.FloorToInt(width);
                m_LeftPosition = new Vector2(transform.position.x - width - 0.5f, transform.position.y);
                m_RightPosition = new Vector2(transform.position.x + width + 0.5f, transform.position.y);
            }
        }
    }
}