using UnityEngine;
using UnityEngine.Events;

namespace PaintingManual
{
   public class InitializationGame : MonoBehaviour
   {
      [SerializeField] private RectTransform _workspaceTransform;

      private GameObject _prefab;
      private LevelPainting _level;
      
      public UnityAction<LevelPainting> LevelSelected;
      public event UnityAction<LevelPainting> OnSelectedLevel;

      protected void Awake()
      {
         LevelSelected = CreateColoring;

      }

      protected void OnDisable()
      {
         if (_prefab != null)
         {
            Destroy(_prefab);
            _level.Serialization();
         }
      }

      private void CreateColoring(LevelPainting level)
      {
         _level = level;
         OnSelectedLevel?.Invoke(level);
         _prefab = Instantiate(level.ColoringPrefab, _workspaceTransform);
         SetupPosition(_prefab);
         level.CachingComponents(_prefab);
         
         AnalyticsHelper.Instance.SendLevelStarted(1,_level.ID);
         
      }
      private void SetupPosition(GameObject coloring)
      {
         if (coloring.transform is RectTransform rt)
         {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = new Vector2(1f, 0.95f);
            rt.localScale = new Vector3(100f, 100f, 0); // pixel per unit = scale 100:1
            rt.offsetMin = Vector2.zero; // zero deviation from the edges (left,buttom)
            rt.offsetMax = Vector2.zero; //                                (right,up)
         }
      }
      
   }
}