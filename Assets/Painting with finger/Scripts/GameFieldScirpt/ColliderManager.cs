using UnityEngine;

namespace PaintingManual
{
   public class ColliderManager : MonoBehaviour
   {
      private LevelPainting _level;
      private Painting _painting;
      private InitializationGame _initializationGame;
      
      protected void OnEnable()
      {
         _painting = GetComponent<Painting>();
         _initializationGame = GetComponent<InitializationGame>();
         
         _painting.TurnedOff += TurnOffColliders;
         _painting.TurnedOn += TurnOnColliders;
         _initializationGame.OnSelectedLevel += UpdateLevel;;
      }

      protected void OnDisable()
      {
         _painting.TurnedOff -= TurnOffColliders;
         _painting.TurnedOn -= TurnOnColliders;
         _initializationGame.OnSelectedLevel -= UpdateLevel;;
      }

      private void UpdateLevel(LevelPainting level)
      {
         _level = level;
      }

      private void TurnOffColliders(string layer)
      {
         for (int i = 0; i < _level.ColliderComponent.Length; i++)
         {
            if (_level.ColliderComponent[i].name != layer)
            {
               _level.ColliderComponent[i].enabled = false;
            }
         }
      }

      private void TurnOnColliders()
      {
         for (int i = 0; i < _level.ColliderComponent.Length; i++)
         {
            _level.ColliderComponent[i].enabled = true;
         }
      }
      
   }
}