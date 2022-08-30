using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpriteScalerNew : MonoBehaviour
{
    [Inject] Camera m_Camera;
    //[SerializeField] private Camera _camera;
    [Tooltip("Fit sprite to long or short screen side")]
    [SerializeField] private bool _IsLongScreenSideFit = true;
    [Tooltip("Force center sprite position")]
    [SerializeField] private bool _IsCenterScreen = true;
    [Tooltip("Can change additional scale")]
    [SerializeField] private float _customScale = 1;
    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localScale = Vector2.one;

        float cameraHeight = m_Camera.orthographicSize * 2;
        float spriteRatio = spriteRenderer.sprite.bounds.size.x /
            spriteRenderer.sprite.bounds.size.y;
        Vector2 cameraSize = new Vector2(m_Camera.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if(_IsLongScreenSideFit)
        {
            if (cameraSize.x >= cameraSize.y)
            { // Landscape (or equal)
                if (m_Camera.aspect >= spriteRatio)
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
                if (m_Camera.aspect >= spriteRatio)
                {
                    scale *= cameraSize.x / spriteSize.x;
                }
                else
                {
                    scale *= cameraSize.y / spriteSize.y;
                }
            }
        }
        else
        {
            if (cameraSize.x >= cameraSize.y)
            { // Landscape (or equal)
                if (m_Camera.aspect >= spriteRatio)
                {
                    scale *= cameraSize.y / spriteSize.y;
                }
                else
                {
                    scale *= cameraSize.x / spriteSize.x;
                }
            }
            else
            { // Portrait
                if (m_Camera.aspect >= spriteRatio)
                {
                    scale *= cameraSize.y / spriteSize.y;
                }
                else
                {
                    scale *= cameraSize.x / spriteSize.x;
                }
            }
        }

        if(_IsCenterScreen)
        {
            transform.position = Vector2.zero; // Optional
        }
        transform.localScale = scale * _customScale;
    }
}
