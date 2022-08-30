using System;
using UnityEngine;


namespace PaintingManual
{
   public class ResetTexture : MonoBehaviour
   {
      private LevelPainting _level;

      public Action ResetTextureAction;

      protected void Awake()
      {
         GetComponent<InitializationGame>().OnSelectedLevel+=UpdateLevel;
      }

      protected void OnEnable()
      {
         ResetTextureAction += ResetColoringTexture;

      }

      protected void OnDisable()
      {
         ResetTextureAction -= ResetColoringTexture;
      }

      private void UpdateLevel(LevelPainting level)
      {
         _level = level;
      }
      private void ResetColoringTexture()
      {
         var copyTexture = _level.CopyTexture.GetPixels32();
         _level.RendererComponent[0].sprite.texture.SetPixels32(copyTexture);
         _level.RendererComponent[0].sprite.texture.Apply();
         _level.Pixels = copyTexture;
      }
   }
}