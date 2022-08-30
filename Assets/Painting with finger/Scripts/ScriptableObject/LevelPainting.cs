using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace PaintingManual
{
   
   [CreateAssetMenu(menuName = "Painting manual/Coloring",fileName = "New coloring")]
   public class LevelPainting : ScriptableObject
   {
      [SerializeField] private GameObject _coloringPrefab;
      [SerializeField] private Texture2D _copyTexture2D;
      [SerializeField] private bool _itsFree;
      [SerializeField] private int _id;
      [SerializeField] private bool _stickerReceived;
      [SerializeField] private bool _itsColored;

      private Dictionary<string, bool> _spriteColored;
      public Dictionary<string, bool> SpriteColored => _spriteColored;
      public GameObject ColoringPrefab => _coloringPrefab;
      public Texture2D CopyTexture => _copyTexture2D;
      public PolygonCollider2D[] ColliderComponent { get; private set; }
      public SpriteRenderer[] RendererComponent { get; private set; }
      public Color32[] Pixels { get;  set; }
      public bool Free => _itsFree;

      public bool Colored
      {
         get => _itsColored;
         set => _itsColored = value;
      }

      public bool StickerReceived
      {
         get => _stickerReceived;
         set => _stickerReceived = value;
      }
      public int ID => _id;
      protected void OnEnable()
      {
         Deserialize();
         Pixels = _coloringPrefab.GetComponent<SpriteRenderer>().sprite.texture.GetPixels32();
      }

      public void CachingComponents(GameObject prefabClone)
      {
         var layerCount = prefabClone.transform.childCount;
         ColliderComponent = new PolygonCollider2D[layerCount];
         RendererComponent = new SpriteRenderer[layerCount];
         _spriteColored = new Dictionary<string, bool>();
         for (int i = 0; i < layerCount; i++)
         {
            var layer=prefabClone.transform.GetChild(i).gameObject;
            var collider = layer.GetComponent<PolygonCollider2D>();
            collider.enabled = true;
            ColliderComponent[i] = collider;
            var renderer = layer.GetComponent<SpriteRenderer>();
            RendererComponent[i] = renderer;
            SpriteColored.Add(renderer.name,false);
         }
      }

      public void Serialization()
      {

#if UNITY_ANDROID || UNITY_IOS
         BinaryFormatter bf = new BinaryFormatter(); 
         FileStream file = File.Create(Application.persistentDataPath +$"/pm{_coloringPrefab.name}.dat");
         var bytes=_coloringPrefab.GetComponent<SpriteRenderer>().sprite.texture.EncodeToPNG();
         bf.Serialize(file,StickerReceived);
         bf.Serialize(file,bytes);
         file.Close();
         Debug.Log("Game data saved!");
#endif
      }

      public void Deserialize()
      {
#if UNITY_ANDROID || UNITY_IOS
         if (File.Exists(Application.persistentDataPath + $"/pm{_coloringPrefab.name}.dat"))
         {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath +$"/pm{_coloringPrefab.name}.dat", FileMode.Open);
            var stickerReceived = (bool)bf.Deserialize(file);
            var bytes = (byte[]) bf.Deserialize(file);
            _coloringPrefab.GetComponent<SpriteRenderer>().sprite.texture.LoadImage(bytes);
            _stickerReceived = stickerReceived;
            file.Close();
            Debug.Log("Game data loaded!");
         }
#endif
      }
   }
}


