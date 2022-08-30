using UnityEngine;

namespace PaintingManual
{
   [CreateAssetMenu(menuName = "Painting manual/Palette",fileName = "New Palette")]
   public class Palette : ScriptableObject
   {
      [SerializeField] private Sprite _spriteActivated;
      [SerializeField] private Sprite _outterSprite;
      [SerializeField] private Sprite _inactiveSprite;
      [SerializeField] private string[] _colorsInnerNames;
      [SerializeField] private string[] _colorsOutterName;
      [SerializeField] private GameObject _colorPrefab;

      public string[] ColorsInnerNames => _colorsInnerNames;
      public string[] ColorOutterNames => _colorsOutterName;
      public GameObject ColorPrefab => _colorPrefab;
      public Sprite SpriteActivated => _spriteActivated;
      public Sprite OutterSprite => _outterSprite;
      public Sprite InActiveSprite => _inactiveSprite;
   }
}

