using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class LoopScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   [SerializeField] private Camera _camera;

   private float _pixelPerUnit=100;
   // control loop scroll
   private ScrollRect _scrollRect;
   private float _lastPosY;
   private bool _itsPositiveDrag=true;
   private Vector2 _topPosition;
   private Vector2 _bottomPosition;
   
   // init content
   private float _childHeight;
   private float _childWidth;
   private Transform[] _childs;

   public ScrollRect ScrollRect => _scrollRect;
   public UnityAction<Transform> OnColorSelected;
   protected void Start()
   {
      _scrollRect = GetComponent<ScrollRect>();
      _scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
      InitializeContentVertical();
      InitPosThreshold();
      //OnColorSelected+=MoveItem;
      
      //_upPosition = transform.position.y+_camera.orthographicSize+0.5f;
      //_downPosition = transform.position.y - _camera.orthographicSize - 0.5f;
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
      if (transform.localPosition.y > 0)
      {
         _itsPositiveDrag = false;
         _scrollRect.velocity= new Vector2(0,-transform.position.y);
      }
      else
      {
         _itsPositiveDrag = true;
         _scrollRect.velocity= new Vector2(0,-transform.position.y);
      }

   }

   private IEnumerator ChangeVelocity(Transform transform)
   {
      yield return new WaitForSeconds(0.5f);
      int sibling = transform.GetSiblingIndex();
      for (int i = 0; i < _scrollRect.content.childCount; i ++ )
      {
         var transformChild = _scrollRect.content.GetChild(i);
         transform.SetSiblingIndex(transform.GetSiblingIndex()+1);
      }

   }
   public void OnBeginDrag(PointerEventData eventData)
   {
      _lastPosY = eventData.position.y;
   }

   public void OnDrag(PointerEventData eventData)
   {
      if (eventData.position.y > _lastPosY)
         _itsPositiveDrag = true;
      else
         _itsPositiveDrag = false;
      _lastPosY = eventData.position.y;
   }

   public void OnEndDrag(PointerEventData eventData)
   {
      
   }

   public void OnScrollViewChanged()
   {
      int currItemIndex = _itsPositiveDrag ?  0:_scrollRect.content.childCount - 1; //first or last 
      var currItem = _scrollRect.content.GetChild(currItemIndex);
      
      if (!ReachedThreshold(currItem))
         return;
      int endItemIndex = _itsPositiveDrag ? _scrollRect.content.childCount - 1 : 0; 
      Transform endItem = _scrollRect.content.GetChild(endItemIndex);
      Vector2 newPos = endItem.localPosition;
      if (_itsPositiveDrag)
      {
         newPos.y -= _childHeight+20;
      }
      else
      {
         newPos.y += _childHeight+20;
      }
      
      currItem.localPosition = newPos;
      currItem.SetSiblingIndex(endItemIndex);

   }
   
   private bool ReachedThreshold(Transform item)
   {
      //return _positiveDrag ? item.position.y  > _upPosition : item.position.y  < _downPosition;
      return _itsPositiveDrag ? item.position.y  > _topPosition.y : item.position.y  < _bottomPosition.y;
   }
   
      
   private void InitializeContentVertical()
   {
      _childs = new Transform[_scrollRect.content.childCount];
      var startPos = (transform as RectTransform).rect.max;
      for (int i = 0; i < _childs.Length; i++)
      {
         var childTransform = (RectTransform) _scrollRect.content.transform.GetChild(i);
         _childs[i] = childTransform;
         childTransform.anchoredPosition = new Vector2(0.5f, 0.5f);
         _childWidth = childTransform.rect.width;
         _childHeight = childTransform.rect.height;
         childTransform.localPosition = new Vector2(0f, startPos.y-(_childHeight+20) * i);
      }
   }

   private void InitPosThreshold()
   {
      var width =  (transform as RectTransform).rect.width/_pixelPerUnit;
      var height =  ( (transform as RectTransform).rect.height/_pixelPerUnit ) /2;
      height = Mathf.FloorToInt(height);
      _topPosition = new Vector2(transform.position.x, transform.position.y + height+0.5f);
      _bottomPosition = new Vector2(transform.position.x, transform.position.y -height-0.5f);
      Debug.DrawLine(Vector3.zero, _topPosition,Color.red,100f);
      Debug.DrawLine(Vector3.zero, _bottomPosition,Color.red,100f);
   }
}
