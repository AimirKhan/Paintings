using UnityEngine;
using UnityEngine.UI;

public class Grid_SizeFix : MonoBehaviour
{
    [SerializeField] private float personScale = 1;

    [Header("Prefab")]
    [SerializeField] private Image Back;
    [SerializeField] private Image Person;
    [SerializeField] private Image Frame;

    static private float size = 512;

    public void Resize()
    {
        Vector2 tmp = GetComponentInParent<GridLayoutGroup>().cellSize;
        Resize(tmp.x, tmp.y);
    }

    public void SetZero()
    {
        Resize(size, size);
    }

    public void Resize(float w, float h)
    {
        //BackFrameResize(w, h);
        PersonResize(w, h);
    }

    public void SetFrame(Sprite NewFrame)
    {
        Frame.sprite = NewFrame;
        Frame.SetNativeSize();
    }

    private void BackFrameResize(float w, float h)
    { 
        Back.rectTransform.sizeDelta = new Vector2(w, h);
        Back.rectTransform.localScale = new Vector3(1, 1, 1);

        Frame.rectTransform.sizeDelta = new Vector2(w, h);
        Frame.rectTransform.localScale = new Vector3(1, 1, 1);
    }

    private void PersonResize(float w, float h)
    {
        Person.SetNativeSize();
        float scaleW = w / (Person.sprite.bounds.size.x * Person.sprite.pixelsPerUnit);
        float scaleH = h / (Person.sprite.bounds.size.y * Person.sprite.pixelsPerUnit);
        
        if (scaleW <= scaleH) Person.rectTransform.localScale = new Vector3(scaleW * personScale, scaleW * personScale, 1);
        else Person.rectTransform.localScale = new Vector3(scaleH * personScale, scaleH * personScale, 1);
    }
}
