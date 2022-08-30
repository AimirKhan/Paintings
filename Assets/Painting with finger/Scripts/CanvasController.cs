using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PaintingManual
{
   public enum CanvasType
   {
      Gallery = 0,
      Game = 1,
      Premium = 2,
      Settings = 3,
      UI=4
   };

   public class CanvasController : MonoBehaviour
   {
      [SerializeField] [Header("Gallery")] private GameObject _galleryCanvas;
      [SerializeField] [Header("Gamefield")] private GameObject _gameCanvas;
      [SerializeField] [Header("Premium")] private GameObject _premiumCanvas;
      [SerializeField] [Header("Settings")] private GameObject _settingsCanvas;
      [SerializeField] [Header("UI")] private GameObject _UICanvas;
      
      private Dictionary<CanvasType, GameObject> _canvases;
      
      public UnityAction<CanvasType> OnShowCanvas;
      public Dictionary<CanvasType, GameObject> Canvases => _canvases;

      protected void Awake()
      {
         OnShowCanvas += SwitchCanvas;
         InitCanvases();
      }

      private void InitCanvases()
      {
         _canvases = new Dictionary<CanvasType, GameObject>();
         if (_galleryCanvas != null)
            _canvases.Add(CanvasType.Gallery,_galleryCanvas);
         if(_gameCanvas!=null)
            _canvases.Add(CanvasType.Game,_gameCanvas);
         if(_premiumCanvas!=null)
            _canvases.Add(CanvasType.Premium,_premiumCanvas);
         if(_settingsCanvas!=null)
            _canvases.Add(CanvasType.Settings,_settingsCanvas);
         if(_UICanvas!=null)
            _canvases.Add(CanvasType.UI,_UICanvas);
      }
      private void SwitchCanvas(CanvasType type)
      {
         switch (type)
         {
            case CanvasType.Gallery:
               _canvases[CanvasType.Gallery].SetActive(true);
               _canvases[CanvasType.Game].SetActive(false);
               _canvases[CanvasType.UI].SetActive(true);
               return;
            case CanvasType.Game:
               _canvases[CanvasType.Gallery].SetActive(false);
               _canvases[CanvasType.Game].SetActive(true);
               _canvases[CanvasType.UI].SetActive(false);
               return;
            case CanvasType.Premium:
               return;
            case CanvasType.Settings:
               return;
         }
      }
   }
}