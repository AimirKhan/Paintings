using System;
using System.Collections;
using System.Collections.Generic;
using CandyCoded.HapticFeedback;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace PaintingManual
{
   public class InitializationGallery : MonoBehaviour
   {
      [SerializeField] private LevelPainting[] _levels;
      [SerializeField] private Transform _parentTransform;
      [SerializeField] private GameObject _tilePrefab;
      [SerializeField] private GameObject _tileClosedPrefab;
      [SerializeField] private GameObject _tileColoredPrefab;
      [SerializeField] private GameObject _premium;


      private HashSet<Tuple<LevelPainting,GameObject>> _hashSet;
      private CheatsMenu _cheatsMenu;
      
      protected void Awake()
      {
         _hashSet = new HashSet<Tuple<LevelPainting, GameObject>>();
         var canvasController = FindObjectOfType<CanvasController>();
         var script = canvasController.Canvases[CanvasType.Game].GetComponent<InitializationGame>();
         for (int i=0; i < _levels.Length; i++)
         {
            GameObject tile;

            if (!_levels[i].Free && !Purchaser.isPremium())
               tile = Instantiate(_tileClosedPrefab, _parentTransform);
            else
            {
               if (!_levels[i].Colored || _levels[i].StickerReceived)
                  tile = Instantiate(_tileColoredPrefab, _parentTransform);
               else
                  tile = Instantiate(_tilePrefab, _parentTransform);
            }

            var coloring = Instantiate(_levels[i].ColoringPrefab, tile.transform);
            coloring.transform.localScale = new Vector2(29f, 29f);
            coloring.transform.localPosition = new Vector2(-0.5f, 0f);
            var level = _levels[i];
            if (level.Free || Purchaser.isPremium())
            {
               tile.GetComponent<Button>().onClick.AddListener(delegate { canvasController.OnShowCanvas(CanvasType.Game); });
               tile.GetComponent<Button>().onClick.AddListener(delegate { script.LevelSelected(level); });
            }
            else 
            {
               tile.GetComponent<Button>().onClick.AddListener(()=>_premium.GetComponent<Button>().onClick?.Invoke());
            }
            tile.GetComponent<Button>().onClick.AddListener(delegate { HapticFeedback.LightFeedback(); });
            _hashSet.Add(new Tuple<LevelPainting, GameObject>(level,tile));
         }
      }

      protected void OnEnable()
      {
         _cheatsMenu = FindObjectOfType<CheatsMenu>();
         if(_cheatsMenu!=null)
            _cheatsMenu.OnPremiumClick+=CheatsMenuOnOnPremiumClick;
         ChangeTile();
         
      }

      protected void ChangeTile()
      {
         foreach (var item in _hashSet)
         {
            if (item.Item1.StickerReceived || item.Item1.Colored)
            {
               item.Item2.GetComponent<Image>().sprite = _tilePrefab.GetComponent<Image>().sprite;
               var coloring = item.Item2.transform.GetChild(0);
               coloring.transform.localScale = new Vector2(29f, 29f);
               coloring.transform.localPosition = new Vector2(-0.5f, 0f);
            }
            
         }
      }
      protected void OnDisable()
      {
         if(_cheatsMenu!=null)
            _cheatsMenu.OnPremiumClick-=CheatsMenuOnOnPremiumClick;
      }

      private void CheatsMenuOnOnPremiumClick()
      {
         var intScene = SceneManager.GetActiveScene().buildIndex;
         SceneManager.LoadScene(intScene);
      }
      
      private IEnumerator StartLoadLevel(CanvasController canvasController,InitializationGame initializationGame,LevelPainting level)
      {
         yield return new WaitForSeconds(1f);
         canvasController.OnShowCanvas?.Invoke(CanvasType.Game);
         initializationGame.LevelSelected?.Invoke(level);
      }

      private void Test()
      {
         
      }
   }
}