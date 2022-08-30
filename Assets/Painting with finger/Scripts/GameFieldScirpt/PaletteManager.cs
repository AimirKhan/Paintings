using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


namespace PaintingManual
{
   public class PaletteManager : MonoBehaviour
   {
      [SerializeField] private Palette _palette;
      
      private LoopScroll _scroll;
      private ToolManager _toolManager;

      public event Action<Color> OnColorChanged;

      protected void OnEnable()
      {
         _scroll = FindObjectOfType<LoopScroll>();
         _toolManager = FindObjectOfType<ToolManager>();
      }

      protected void Awake()
      {
         InitPalette();
      }
      
      private void InitPalette()
      {
         int countColors = _palette.ColorOutterNames.Length;
         Color color;
         for (int i = 0; i < countColors; i++)
         {
            var inner=Instantiate(_palette.ColorPrefab, transform);
            var imageInner = inner.GetComponent<Image>();
            inner.name = i.ToString();
            ColorUtility.TryParseHtmlString(_palette.ColorsInnerNames[i], out color);
            imageInner.color = color;
            AddFunctionToButton(inner,imageInner.color,_palette.ColorOutterNames[i]);
         }
      }
      

      private void AddFunctionToButton(GameObject inner,Color color,string colorOutters)
      {
         var buttonColor = inner.AddComponent<Button>();
         buttonColor.onClick.AddListener(()=>OnColorChanged?.Invoke(color));
         //buttonColor.onClick.AddListener(()=>_scroll.OnColorSelected?.Invoke(buttonColor.transform));
      }
      
      
   }
}

