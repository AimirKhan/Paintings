using UnityEngine;

[CreateAssetMenu(fileName = "ColoringData", menuName = "Coloring Pages/Page")]
public class ColoringPage : ScriptableObject
{
    public float ImageSize;
    public Sprite StartSprite;
    public Vector3 PositionStartImage;
    [SerializeField] public Arts[] m_Arts;

}

[System.Serializable]
public struct Arts
{
    public Sprite Sprite;
    public Vector3 PositionImage;
    public Vector3 PositionUIImage;
    public bool isShading;
}
