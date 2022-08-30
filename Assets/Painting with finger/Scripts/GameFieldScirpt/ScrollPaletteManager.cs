using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
//using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;


namespace PaintingManual
{
   [RequireComponent(typeof(RectTransform),typeof(ScrollRect))]
   public class ScrollPaletteManager : MonoBehaviour, IScrollHandler,IDragHandler
   {
      [SerializeField] private Palette _palette;

      private RectTransform _content;
      private GameObject[] _pooledObjects;
      private ScrollRect _scrollRect;
      private int _elementsInScreen;
      private int _elemenentsHide = 0;
      private float _previousPosition=0;
      private bool odinraz = true;
      private Vector2 _previosVector;
      protected void Start()
      {
         _scrollRect = GetComponent<ScrollRect>();
         _content = _scrollRect.content;
         InitContent(_scrollRect.content);
         _scrollRect.onValueChanged.AddListener(ScrollBegin);
      }

      private void ScrollBegin(Vector2 arg0)
      {
         if(_scrollRect.velocity.y>1000)
            _scrollRect.velocity=Vector2.zero;
         if (_scrollRect.normalizedPosition.y < 1 ) //DOWN
         {
            if (Math.Round(_scrollRect.verticalNormalizedPosition,1)==0.1)
            {
               int count = (int) _content.anchoredPosition.y / 110;


               _scrollRect.DOVerticalNormalizedPos(1f, 1f);
               for (int i = 0; i < count; i++)
               {
                  _pooledObjects[0].transform.SetAsLastSibling();
                  ChangePooledMassive(1);
               }

            }
            else
            {
               Debug.Log("B");
            }
         }
         else
         {
            Debug.Log("C");
         }
         
      }
      
      

      private void ChangePooledMassive(int index)
      {
         var first = _pooledObjects.First();
         var tempArray = new GameObject[_pooledObjects.Length];
         for (int i = 0; i < _pooledObjects.Length;i++)
         {
            Array.Copy(_pooledObjects,index, tempArray, 0, _pooledObjects.Length - 1);
         }
         tempArray[tempArray.Length - 1] = first;
         _pooledObjects = tempArray;
      }
      private void InitContent(RectTransform content)
      {
         Color color;
         int count = _palette.ColorsInnerNames.Length;
         _pooledObjects = new GameObject[count];
         for (int i = 0; i < count; i++)
         {
            var prefab = Instantiate(_palette.ColorPrefab, _content);
            _pooledObjects[i] = prefab;
            prefab.name = i.ToString();
            var imageInner = prefab.GetComponent<Image>();
            ColorUtility.TryParseHtmlString(_palette.ColorsInnerNames[i], out color);
            imageInner.color = color;

            var outer = prefab.transform.GetChild(0);
            var imageOutter = outer.GetComponent<Image>();
            ColorUtility.TryParseHtmlString(_palette.ColorOutterNames[i], out color);
            imageOutter.color = color;
         }
      }

      public void OnScroll(PointerEventData eventData)
      {
         //Debug.Log("a");
      }

      public void OnDrag(PointerEventData eventData)
      {
         //Debug.Log(eventData.delta);
      }
   }
}