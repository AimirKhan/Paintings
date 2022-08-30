using System;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace PaintingManual
{
   public class ToolManager : MonoBehaviour
   {
      [SerializeField] private Tool[] _tools;

      private GameObject _currentTool;
      private ToolType _currentToolType;
      private Tool _selectedTool;
      private PaletteManager _paletteManager;
      private GameObject[] _toolsGameObject;

      public event Action<Tool> OnToolChanged;
      public ToolType CurrentToolType => _currentToolType;

      private void Awake()
      {
         SpawnTools();
         AddFuncToTool();
      }
      
      protected void OnEnable()
      {
         _paletteManager = FindObjectOfType<PaletteManager>();
         if (_paletteManager == null)
            Debug.LogError("PaletteManager script doesn`t find!");
         else
            _paletteManager.OnColorChanged += ColorChanged;
      }

      private void SpawnTools()
      {
         for (int i = 0; i < _tools.Length; i++)
         {
            Instantiate(_tools[i].Prefab,this.transform);
         }
      }

      protected void OnDisable()
      {
         _paletteManager.OnColorChanged -= ColorChanged;
      }

      private void ColorChanged(Color newColor)
      {
         if (_selectedTool!=null)
         {
            var image=GetColorPart(); 
            if (image != null )
            {
               Color color = new Color(newColor.r, newColor.g, newColor.b, 1f);
               image.DOColor(color, 0.2f);
               _selectedTool.ColorReceived = newColor;
            }
         }
      }

      private void AddFuncToTool()
      {
         var childCount = gameObject.transform.childCount;
         _toolsGameObject = new GameObject[childCount];
         for (int i = 0; i < childCount; i++)
         {
            var prefabTool = gameObject.transform.GetChild(i).gameObject;
            _toolsGameObject[i] = prefabTool;
            var tool = _tools[i];
            tool.ColorReceived=prefabTool.GetComponent<Image>().color;
            tool.OldPosX = (prefabTool.transform as RectTransform).anchoredPosition.x;
            
            if (tool.ToolToolType == ToolType.Crayon)
            {
               OnToolChanged?.Invoke(tool);
               ToolSelect(prefabTool.transform as RectTransform);
               _selectedTool = tool;
            }
            prefabTool.GetComponent<Button>().onClick.AddListener(() => OnToolChanged?.Invoke(tool));
            prefabTool.GetComponent<Button>().onClick.AddListener(()=>ToolSelect(prefabTool.transform as RectTransform));
            prefabTool.GetComponent<Button>().onClick.AddListener(() => _selectedTool = tool );
         }
      }

      private void ToolSelect(RectTransform transform)
      {
         if (transform.gameObject==_currentTool) // tap himself tool
         {
            if (transform.anchoredPosition.x < _selectedTool.OldPosX)
            {
               ToolUnselectedAnimation(_currentTool.transform as RectTransform);
               OnToolChanged?.Invoke(null);
            }
            else
            {
               ToolSelectedAnimation(transform);
               OnToolChanged?.Invoke(_selectedTool);
            }
         }
         else if (_currentTool != null) // tap another tool
         {
            ToolUnselectedAnimation(_currentTool.transform as RectTransform);
            ToolSelectedAnimation(transform);
            _currentTool = transform.gameObject;
         }
         else if (_currentTool == null) // tool unselected
         {
            ToolSelectedAnimation(transform);
            _currentTool = transform.gameObject;
         }
      }

      private void ToolUnselectedAnimation(RectTransform RTtool)
      {
         RTtool.DOAnchorPos(new Vector2(_selectedTool.OldPosX, RTtool.anchoredPosition.y), 0.3f);
      }

      private void ToolSelectedAnimation(RectTransform RTtool)
      {
         RTtool.DOAnchorPos(new Vector2(0, RTtool.anchoredPosition.y), 0.3f);
      }
      private Image GetColorPart()
      {
         if (_selectedTool.ColorPartSpriteSprite == null)
            return null;
         else
         {
            var sprite=SearchColorGameObject(_selectedTool.ColorPartSpriteSprite.name);
            return sprite;
         }
      }

      private Image SearchColorGameObject(string name)
      {
         foreach (var item in _toolsGameObject)
         {
            var image=item.GetComponent<Image>();
            if (name == image.sprite.name)
            {
               return image;
               
            }
            else
            {
               for (int i = 0; i < item.transform.childCount; i++)
               {
                  image = item.transform.GetChild(i).GetComponent<Image>();
                  if (name == image.sprite.name)
                     return image;
               }
              
            }
         }
         return null;
      }
   }
}