using UnityEngine;
using UnityEngine.UI;

namespace PaintingManual
{
   [CreateAssetMenu(fileName = "New tool", menuName = "Painting manual/Tool", order = 0)]
   public class Tool : ScriptableObject
   {
      [SerializeField] private ToolType _toolType;
      [SerializeField] private Sprite _colorPartSprite;
      [SerializeField] private Sprite _stampil;
      [SerializeField] private GameObject _prefab;

      
      public ToolType ToolToolType => _toolType;
      public Sprite ColorPartSpriteSprite => _colorPartSprite;
      public Sprite Stampil => _stampil;
      public GameObject Prefab => _prefab;
      public Color ColorReceived { get; set; } = Color.white;
      public float OldPosX { get; set; }
   }
}