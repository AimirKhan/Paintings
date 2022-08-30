using System;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;
using Random = UnityEngine.Random;

namespace PaintingManual
{
   public class Painting : MonoBehaviour
   {
      private const int StampTextureSize = 64;
      private const int TextureSize = 1024;
      private const int MinAlpha = 100;

      [SerializeField] private Camera _camera;
      [SerializeField] private Brush _brush = new Brush();
      [SerializeField] private StickersCounter _stickerCounter;
      [Range(0, 1f)] [SerializeField] private float _percentForComplete = 0.9f;
      [Range(0, 1f)] [SerializeField] private float _a=1f;

      private bool _collidersOff;
      private Color32[] _pixelsBrush;
      private Color32[] _copyTexture;
      private Vector2? _oldPos;
      private HashSet<Vector2> _position;
      private DateTime _startTime;

      private InitializationGame _initializationGame;
      private PaletteManager _paletteManager;
      private ToolManager _toolManager;
      private Sprite _currentSprite;
      private LevelPainting _level;
      private ButtonsControl _buttonsControl;
      public event Action<string> TurnedOff;
      public event Action TurnedOn;

      protected void Awake()
      {
         _position = new HashSet<Vector2>();
      }

      protected void OnEnable()
      {
         _startTime = DateTime.Now;

         _initializationGame = FindObjectOfType<InitializationGame>();
         _paletteManager = FindObjectOfType<PaletteManager>();
         _toolManager = FindObjectOfType<ToolManager>();
         _buttonsControl = FindObjectOfType<ButtonsControl>();

         _initializationGame.OnSelectedLevel += GetLevel;
         _paletteManager.OnColorChanged += SetColor;
         _toolManager.OnToolChanged += ChangeTool;
      }

      protected void OnDisable()
      {
         _initializationGame.OnSelectedLevel -= GetLevel;
         _paletteManager.OnColorChanged -= SetColor;
         _toolManager.OnToolChanged -= ChangeTool;
      }

      private void ChangeTool(Tool tool)
      {
         if (tool == null)
         {
            _brush.ToolType = ToolType.None;
         }
         else
         {
            _brush.ToolType = tool.ToolToolType;
            _brush.Color = tool.ColorReceived;
            _position.Clear();
            if (tool.ToolToolType == ToolType.Brush || tool.ToolToolType == ToolType.Marker ||
                tool.ToolToolType == ToolType.Spray)
            {
               _pixelsBrush = tool.Stampil.texture.GetPixels32();
            }
         }
      }

      private void SetColor(Color obj)
      {
         if (_brush.ToolType != ToolType.Eraser || _brush.ToolType != ToolType.None) _brush.Color = obj;
      }

      private void GetLevel(LevelPainting level)
      {
         _level = level;
         _copyTexture = _level.CopyTexture.GetPixels32();
      }

      protected void Update()
      {
#if UNITY_EDITOR
         if (Input.GetMouseButton(0))
         {
            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null)
            {
               if (!_collidersOff)
               {
                  TurnedOff?.Invoke(hit.collider.name);
                  _collidersOff = true;
               }

               var currentPosition = Input.mousePosition;
               PaintProcess(hit.collider.name, hit.transform as RectTransform);
               _oldPos = currentPosition;
            }
         }

         if (Input.GetMouseButtonUp(0))
         {
            if (_collidersOff)
            {
               TurnedOn?.Invoke();
               _collidersOff = false;
               CalculatePercentPainted();
               _currentSprite = null;
               CheckColored();
               _oldPos = null;
            }
         }
#endif
#if UNITY_IOS || UNITY_ANDROID
         if (Input.touchCount > 0)
         {
            for (int i = 0; i < Input.touchCount; i++)
            {
               var touch = Input.GetTouch(i);
               if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved && Input.touchCount < 2)
               {
                  RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(touch.position), Vector2.zero);
                  if (hit.collider != null)
                  {
                     if (!_collidersOff)
                     {
                        TurnedOff?.Invoke(hit.collider.name);
                        _collidersOff = true;
                        HapticFeedback.MediumFeedback();
                     }

                     var currentPosition = touch.position;
                     PaintProcess(hit.collider.name, hit.transform as RectTransform);
                     _oldPos = currentPosition;
                  }
               }
               else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
               {
                  if (_collidersOff)
                  {
                     TurnedOn?.Invoke();
                     _collidersOff = false;
                     CalculatePercentPainted();
                     CheckColored();
                     _currentSprite = null;
                     _oldPos = null;
                  }
               }
            }
         }
#endif
      }

      private void PaintProcess(string layer, RectTransform rt)
      {
         if (_currentSprite == null)
         {
            var index = Array.FindIndex(_level.ColliderComponent, x => x.name == layer);
            _currentSprite = _level.RendererComponent[index].sprite;
         }

         var newPosTexture = Mouse2Texture(rt, _currentSprite, Input.mousePosition);
         Vector2 oldPosTexture = _oldPos == null
            ? Mouse2Texture(rt, _currentSprite, Input.mousePosition)
            : Mouse2Texture(rt, _currentSprite, _oldPos.Value);
         Bresenham(oldPosTexture, newPosTexture);
         _currentSprite.texture.Apply();
         
      }
      
      private void Bresenham(Vector2 oldPos, Vector2 newPos)
      {
         int x0 = (int) oldPos.x, x1 = (int) newPos.x, y0 = (int) oldPos.y, y1 = (int) newPos.y;
         int dx = Math.Abs(x1 - x0), sx = x0 < x1 ? 1 : -1;
         int dy = Math.Abs(y1 - y0), sy = y0 < y1 ? 1 : -1;
         int err = (dx > dy ? dx : -dy) / 2, e2;
         int count = 0;
         int h;
         if (_brush.ToolType == ToolType.Crayon || _brush.ToolType == ToolType.Eraser)
            h = 10;
         else
            h = 100;
         for (;;)
         {
            if (count % h == 0) 
            {
               if(_brush.ToolType==ToolType.Crayon || _brush.ToolType==ToolType.Eraser)
                  PaintCircle(x0, y0);
               else if (_brush.ToolType==ToolType.Brush || _brush.ToolType==ToolType.Marker || _brush.ToolType==ToolType.Spray)
                  PaintStamp(x0,y0);
            }

            if (x0 == x1 && y0 == y1) break;
            e2 = err;
            if (e2 > -dx)
            {
               err -= dy;
               x0 += sx;
            }

            if (e2 < dy)
            {
               err += dx;
               y0 += sy;
            }

            count++;
         }
      }
      
      private void PaintStamp(int cx, int cy)
      {
         int x, y;
         Color32[] tempArray = _level.Pixels;
         var textureRect = _currentSprite.textureRect;
         for (int i = 0; i < StampTextureSize; i++)
         {
            x = cx - StampTextureSize / 2 + i;
            for (int j = 0; j < StampTextureSize; j++)
            {
               y = cy - StampTextureSize / 2 + j;
               if (y > textureRect.yMin && y < textureRect.yMax && x > textureRect.xMin && x < textureRect.xMax)
               {
                     if (tempArray[y * TextureSize + x].a != 0 && _pixelsBrush[j * StampTextureSize + i].a > MinAlpha)
                     {
                         _pixelsBrush[j * StampTextureSize + i]=_brush.Color;
                         tempArray[y * TextureSize + x] = _pixelsBrush[j * StampTextureSize + i];
                     }
               }
            }
         }

         _currentSprite.texture.SetPixels32(tempArray);
      }
      
      private void PaintCircle(int cx, int cy)
      {
         int x, y;
         Color32[] tempArray = _level.Pixels;
         int radius = _brush.Size;
         var r2 = radius * radius;
         for (y = 0; y < r2; y++)
         {
            int y2 = (-radius + y) * (-radius + y);
            int deltaY = cy - radius + y;

            for (x = 0; x < r2; x++)
            {
               int x2 = (-radius + x) * (-radius + x);
               int deltaX = cx - radius + x;
               if (x2 + y2 < r2)
               {
                  if (deltaY < TextureSize)
                  {
                     if (CheckTextureCoordinateInRectangle(deltaX, deltaY))
                     {
                        if (_brush.ToolType == ToolType.Crayon)
                        {
                           if (tempArray[deltaY * TextureSize + deltaX].a != 0)
                              tempArray[deltaY * TextureSize + deltaX] = _brush.Color;
                        }

                        if (_brush.ToolType == ToolType.Eraser)
                        {
                           tempArray[deltaY * TextureSize + deltaX] = _copyTexture[deltaY * TextureSize + deltaX];
                        }
                     }
                  }
               }
            }
         }

         _currentSprite.texture.SetPixels32(tempArray);
      }

      private Color test(Color pixel,Color brush)
      {
         Vector4 color1 = new Vector4(1 - pixel.r, 1 - pixel.g, 1 - pixel.b, 1);
         Vector4 color2 = new Vector4(1 - brush.r, 1 - brush.g, 1 - brush.b, 1);
         var b = new Vector4(color1.x * color2.x, color1.y * color2.y, color1.z * color2.z, 1f);
         var a = Vector4.one - b;
         return new Color(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z), _a);
      }

      //translate mouse position to texture pixel
      private Vector2 Mouse2Texture(RectTransform rectTransform, Sprite sprite, Vector2 mousePosition)
      {
         Vector2 localpoint;
         RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, _camera, out localpoint);
         Vector2 normalizedPoint = Rect.PointToNormalized(rectTransform.rect, localpoint);
         var deltaX = Mathf.FloorToInt(normalizedPoint.x * sprite.textureRect.width) + sprite.textureRect.xMin;
         var deltaY = Mathf.FloorToInt(normalizedPoint.y * sprite.textureRect.height) + sprite.textureRect.yMin;
         return new Vector2(deltaX, deltaY);
      }

      private bool CompareColor(Color a, Color b)
      {
         const float accdelta = 0.001f;
         bool result = false;
         if (Mathf.Abs(a.r - b.r) < accdelta)
            if (Mathf.Abs(a.g - b.g) < accdelta)
               if (Mathf.Abs(a.b - b.b) < accdelta)
                  result = true;
         return result;
      }

      private void CalculatePercentPainted()
      {
         var texture = _currentSprite.texture;
         var rect = _currentSprite.textureRect;
         var pixels = texture.GetPixels((int) rect.x, (int) rect.y, (int) rect.width, (int) rect.height);
         int count = 0;
         int countOpaque = 0;
         for (int i = 0; i < pixels.Length; i++)
         {
            if (pixels[i].a > 0.6)
            {
               countOpaque++;
               if (CompareColor(pixels[i], _brush.Color)) count++;
            }
         }

         var percent = (float) count / countOpaque;
         if (percent >= _percentForComplete)
         {
            _level.SpriteColored[_currentSprite.name] = true;
         }
      }

      private bool CheckTextureCoordinateInRectangle(int x, int y)
      {
         var textureRect = _currentSprite.textureRect;
         if (y > textureRect.yMin && y < textureRect.yMax && x > textureRect.xMin && x < textureRect.xMax)
            return true;
         else
            return false;
      }

      private void CheckColored()
      {
         int count = 0;
         foreach (var item in _level.SpriteColored)
         {
            if (item.Value) count++;
         }

         if (count == _level.SpriteColored.Count)
         {
            _buttonsControl.OnButtonAnimation?.Invoke();
            if (!_level.StickerReceived)
            {
               Complete();
            }
         }
      }

        private void Complete()
        {
            _level.StickerReceived = true;
            TimeSpan timer = DateTime.Now.Subtract(_startTime);
            int time = (timer.Hours * 60 * 60) + (timer.Minutes * 60) + timer.Seconds;
            _stickerCounter.UpdateLevelsStickers(1, _level.ID, 0, time);
            Debug.Log($"{_level.name}:Sticker Received");
        }

        public void DebugComplete()
        {
            Complete();
        }
   }
}