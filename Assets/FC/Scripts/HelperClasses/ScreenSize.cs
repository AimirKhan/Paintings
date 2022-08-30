using UnityEngine;

public class ScreenSize : MonoBehaviour
{

    public Vector2 GetScreenSizeUnit()
    {
        float height = Camera.main.orthographicSize * 2.0f;
        float width = height * Screen.width / Screen.height;

        Vector2 screenSize = new Vector2(width, height);

        return screenSize;
    }

}
