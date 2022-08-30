using System;
using CandyCoded.HapticFeedback;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PaintingManual
{
   public class ButtonsControl : MonoBehaviour
   {
      [SerializeField] private Button _resetButton;
      [SerializeField] private Button _saveExitButton;

      private bool _canReset=false;
      private bool _animationBegin;
      
      private LevelPainting _level;
      private InitializationGame _initializationGame;
      
      public Action OnButtonAnimation;
      
      protected void Awake()
      {
         _saveExitButton.onClick.AddListener(delegate { FindObjectOfType<CanvasController>().OnShowCanvas(CanvasType.Gallery); });
         _saveExitButton.onClick.AddListener(delegate { HapticFeedback.MediumFeedback(); });
         _saveExitButton.onClick.AddListener(()=>ExitFromLevel());
         
         _resetButton.onClick.AddListener(()=>CheckReset());
         _resetButton.onClick.AddListener(delegate { HapticFeedback.MediumFeedback(); });
         
      }

      protected void OnEnable()
      {
         _initializationGame = FindObjectOfType<InitializationGame>();
         _initializationGame.LevelSelected += LevelSelected;
         OnButtonAnimation += StartAnimationButton;
         
         _animationBegin = false;
      }

      
      protected void OnDisable()
      {
         OnButtonAnimation -= StartAnimationButton;
         _initializationGame.LevelSelected -= LevelSelected;
         
         _animationBegin = false;
      }

      private void StartAnimationButton()
      {
         if (!_animationBegin)
         {
            var animator = _saveExitButton.GetComponent<Animator>();
            animator.SetBool("EndGame",true);
            _animationBegin = true;
         }
      }
      private void LevelSelected(LevelPainting arg0)
      {
         if (arg0 != null)
         {
            _level = arg0;
         }
      }

      private void ExitFromLevel()
      {
         if (_level != null)
         {
            if (!_level.StickerReceived)
            {
               AnalyticsHelper.Instance.SendLevelAborted(1,_level.ID);
               Debug.Log($"{_level.name}: Aborted and Sticker dont received");
            }
         }
      }

      private void CheckReset()
      {
         if (_canReset)
         {
            GetComponent<ResetTexture>().ResetTextureAction?.Invoke();
            _canReset = false;
            _resetButton.GetComponent<Image>().DOColor(new Color(0.5f, 0.5f, 0.5f, 1f),0.3f);
         }
         else
         {
            _resetButton.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f),0.3f);
            _canReset = true;
         }
      }
      
      
   }
}