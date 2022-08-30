using UnityEngine;

public class Background_obj : MonoBehaviour
{
    private Background_prefab Active;

    [SerializeField] private SpriteRenderer mainBack;


    void Awake()
    {
        if (Active != null)
            loadBack();
    }

    public void loadBack()
    {
        mainBack.sprite = Active.GetPicture();
    }

    public void loadBack(Background_prefab newBack)
    {
        Active = newBack;
        loadBack();
    }

    public void Resize(float Width, float Height, float cameraGridSize)
    {
        if (mainBack.sprite == null)
        {
            Debug.LogError("No Backgrond");
            return;
        }

        float scaleW = Width / mainBack.sprite.texture.width; 
        float scaleH = Height / mainBack.sprite.texture.height;

        if (scaleW >= scaleH) mainBack.transform.localScale = new Vector3(scaleW, scaleW, 1);
        else mainBack.transform.localScale = new Vector3(scaleH, scaleH, 1);
    }
}
