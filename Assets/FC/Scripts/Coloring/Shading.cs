using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PaintedOverEvent : UnityEvent<int, DataShading, bool> {}

public class Shading : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    [SerializeField] private Palette m_Palette;

    [SerializeField] private Particle m_Particle;

    [SerializeField] private SoundManager m_SoundManager;

    [SerializeField] private int m_ID;

    public PaintedOverEvent PaintedOver;

    private DataShading m_DataShading;
    private Color m_OldColor = Color.clear;
    private Color m_NewColor = new Color(255f, 254f, 255f);
    private int m_OldMater = 0;
    private int m_NewMater = 0;

    private void Awake()
    {
        PaintedOver = new PaintedOverEvent();

        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void AnimationChangeColor(SpriteRenderer spriteRenderer, Color startColor, Color endColor, float duration)
    {
        Tween ChangeColor
            = DOTween.To(
                () => startColor,
                color => m_SpriteRenderer.color = color,
                endColor,
                duration);

        ChangeColor.Play();
    }

    public DataShading GetOldColor()
    {
        var oldDataShading = new DataShading(m_OldColor, m_OldMater);
        return oldDataShading;
    }

    public void Init(Sprite spriteObj, Palette paletteObj, Vector3 positionObj,
        bool isShading, int ID, Color color, int numbMaterial, SoundManager soundManager, Particle particle)
    {
        m_Palette = paletteObj;

        m_Particle = particle;

        m_SoundManager = soundManager;

        m_ID = ID;

        m_NewColor = color;

        m_NewMater = numbMaterial;

        m_SpriteRenderer.material = m_Palette.GetMaterials()[numbMaterial];

        m_SpriteRenderer.color = m_NewColor;

        m_SpriteRenderer.sprite = spriteObj;

        m_DataShading = new DataShading(m_NewColor, m_NewMater);

        if (isShading)
        {
            gameObject.AddComponent<PolygonCollider2D>();
        }
        else
        {
            //Debug.Log("Eyes Invoke");
            PaintedOver.Invoke(m_ID, m_DataShading, false);
        }

        transform.localPosition = positionObj;

        m_SpriteRenderer.sortingOrder = Mathf.Abs((int)positionObj.z);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        m_OldColor = m_NewColor;
        m_NewColor = m_Palette.GetColor().Color;

        m_OldMater = m_NewMater;
        m_NewMater = m_Palette.GetColor().NumbMaterial;

        //Debug.Log($"Old {m_OldMater}, New {m_NewMater}");
        m_SpriteRenderer.material = m_Palette.GetColor().Material;
        //Debug.Log($"Material {m_SpriteRenderer.material.name}");

        m_SpriteRenderer.material.Lerp(m_SpriteRenderer.material,
            m_Palette.GetColor().Material, Mathf.MoveTowards(0f, 1f, 0.25f));
       
        AnimationChangeColor(m_SpriteRenderer, m_OldColor, m_NewColor, 0.25f);

        //m_SpriteRenderer.color = m_NewColor;
        //Debug.Log($"materials {m_SpriteRenderer.material}, {m_Palette.GetColor().Material}");

        //Debug.Log($"Old Color {m_SpriteRenderer.color}, New Color {m_NewColor}");
        
        m_DataShading.Color = m_NewColor;
        m_DataShading.NumbMaterial = m_NewMater;

        m_SoundManager.OnColoringImage();
        //Debug.Log("1" + m_SpriteRenderer.color);
        PaintedOver.Invoke(m_ID, m_DataShading, true);
        //Debug.Log("ID = " + m_ID);

        m_Particle.Click(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public bool CheckOldColor()
    {
        //Debug.Log($"OldColor {m_OldColor}, NewColor {m_NewColor}\n OldMater {m_OldMater}, NewMater {m_NewMater}");
        //Debug.Log("check" + (m_OldColor == m_SpriteRenderer.color));
        if (m_OldColor == m_NewColor && m_OldMater == m_NewMater)
        {
            //Debug.Log("false");
            return false;
        }
        else
        {
            //Debug.Log("true");

            return true;
        }

    }

    public void ResetColor(DataShading value)
    {
        m_NewColor = value.Color;

        m_NewMater = value.NumbMaterial;

        //m_SpriteRenderer.material.Lerp(m_SpriteRenderer.material,
        //    m_Palette.GetColor().Material, Mathf.MoveTowards(0f, 1f, 0.25f));

        //m_SpriteRenderer.DOColor(m_NewColor, 0.25f);

        m_SpriteRenderer.color = value.Color;

        m_SpriteRenderer.material = m_Palette.GetMaterials()[value.NumbMaterial];
    }

    private void OnDestroy()
    {
        PaintedOver.RemoveAllListeners();
    }
}
