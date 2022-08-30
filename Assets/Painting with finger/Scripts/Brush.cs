using System;
using UnityEngine;

namespace PaintingManual
{
   [Serializable]
   public struct Brush
   {
      public int Size;
      public Color Color;
      public ToolType ToolType;
      public Sprite Stamp;
   }
   [Serializable]
   public enum ToolType
   {
      None,
      Crayon,
      Brush,
      Marker,
      Spray,
      Eraser,
      
   }
}