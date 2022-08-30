using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float cameraHeight = _camera.orthographicSize * 2;
        float spriteRatio = spriteRenderer.sprite.bounds.size.x /
            spriteRenderer.sprite.bounds.size.y;
        Vector2 cameraSize = new Vector2(_camera.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        { // Landscape (or equal)
            if (_camera.aspect >= spriteRatio)
            {
                scale *= cameraSize.x / spriteSize.x;
            }
            else
            {
                scale *= cameraSize.y / spriteSize.y;
            }
        }
        else
        { // Portrait
            if (_camera.aspect >= spriteRatio)
            {
                scale *= cameraSize.x / spriteSize.x;
            }
            else
            {
                scale *= cameraSize.y / spriteSize.y;
            }
        }

        transform.position = Vector2.zero; // Optional
        transform.localScale = scale;
    }
}
