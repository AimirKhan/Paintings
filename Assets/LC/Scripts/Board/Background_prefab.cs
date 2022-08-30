using UnityEngine;

public class Background_prefab : MonoBehaviour
{
    [SerializeField] private Sprite picture;
    [SerializeField] private Sprite shadow;
    [SerializeField] private Material line_material;

    public Sprite GetPicture() { return picture; }
    public Sprite GetShadow() { return shadow; }
    public Material GetLine() { return line_material; }
}
