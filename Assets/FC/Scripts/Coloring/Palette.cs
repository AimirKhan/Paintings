using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Palette : MonoBehaviour
{
    [SerializeField] private GameObject m_ButtonColorInticator;
    [SerializeField] private GameObject m_ButtonTextureInticator;
    [SerializeField] private GameObject m_StartParent;

    [SerializeField] private Image colorIndicator;
    [SerializeField] private Image colorSecondIndicator;
    [SerializeField] private Image colorThirdIndicator;
    [SerializeField] private Image colorFourthIndicator;
    [SerializeField] private ArrayColors2D[] m_ArrayColorsButton;
    [SerializeField] private ArrayColors2D[] m_ArrayTextureButton;
    [SerializeField] private Material[] m_ArrayMaterials;

    private DataPalette m_DataPalette = new DataPalette();
    private Color m_Color;
    private Color m_SecondColor;
    private Color m_ThirdColor;
    private int m_NumbMaterial;

    private RectTransform m_RTColorInticator;
    private RectTransform m_RTtextureInticator;
    private RectTransform m_RTNewParent;
    private RectTransform m_RTOldParent;

    public float SizeSelectColor = 1.1f;

    private void Start()
    {
        m_RTNewParent = m_StartParent.GetComponent<RectTransform>();
        m_RTOldParent = m_RTNewParent;

        m_RTColorInticator = m_ButtonColorInticator.GetComponent<RectTransform>();
        m_RTtextureInticator = m_ButtonTextureInticator.GetComponent<RectTransform>();

        ChangeParent(m_StartParent, false);

        ChangeSize(m_StartParent.GetComponent<RectTransform>(), SizeSelectColor);

        ChangeColor(0, false);
    }

    public void OnButtonClick(int value)
    {
        if (value < m_ArrayColorsButton.Length)
        {
            ChangeColor(value, false);
        }
    }

    public void OnButtonTextureClick(int value)
    {
        if (value < m_ArrayMaterials.Length)
        {
            ChangeColor(value, true);
        }
    }

    public void OnButtonClick(GameObject value)
    {
        m_RTOldParent = m_RTNewParent;
        m_RTNewParent = value.GetComponent<RectTransform>();

        ChangeParent(value, false);

        ChangeSize(m_RTNewParent, SizeSelectColor, m_RTOldParent);
    }

    public void OnButtonTextureClick(GameObject value)
    {
        m_RTOldParent = m_RTNewParent;
        m_RTNewParent = value.GetComponent<RectTransform>();

        ChangeParent(value, true);

        ChangeSize(m_RTNewParent, SizeSelectColor, m_RTOldParent);
    }

    public void ResetColor()
    {
        ChangeColor(0, false);

        ChangeParent(m_StartParent, false);
    }

    private void ChangeColor(int matIndex, bool isTexture)
    {
        if (!isTexture)
        {
            m_Color = m_ArrayColorsButton[matIndex].FirstColor;
            m_SecondColor = m_ArrayColorsButton[matIndex].SecondColor;
            m_ThirdColor = m_ArrayColorsButton[matIndex].ThirdColor;

            m_NumbMaterial = 0;
        }
        else
        {
            m_Color = Color.white;
            m_SecondColor = m_ArrayTextureButton[matIndex-1].SecondColor;
            m_ThirdColor = m_ArrayTextureButton[matIndex-1].ThirdColor;

            m_NumbMaterial = matIndex;
        }

        colorIndicator.color = m_Color;
        colorSecondIndicator.color = m_SecondColor;
        colorThirdIndicator.color = m_ThirdColor;
        colorFourthIndicator.color = m_ThirdColor;

        colorIndicator.material = m_ArrayMaterials[m_NumbMaterial];

        m_DataPalette.Color = m_Color;
        m_DataPalette.NumbMaterial = m_NumbMaterial;
        m_DataPalette.Material = m_ArrayMaterials[m_NumbMaterial];
        //Debug.Log($"Material {m_ArrayMaterials[m_NumbMaterial].name}");
    }

    private void ChangeParent(GameObject gameObject, bool isTexture)
    {
        if (isTexture)
        {
            Debug.Log("Texture");
            m_ButtonTextureInticator.SetActive(true);

            m_ButtonColorInticator.SetActive(false);

            m_ButtonTextureInticator.transform.SetParent(gameObject.transform);

            m_RTtextureInticator.localPosition = Vector2.zero;

            m_RTtextureInticator.localScale = Vector2.one;
        }
        else
        {
            m_ButtonColorInticator.SetActive(true);

            m_ButtonTextureInticator.SetActive(false);

            m_ButtonColorInticator.transform.SetParent(gameObject.transform);

            m_RTColorInticator.localPosition = Vector2.zero;

            m_RTColorInticator.localScale = Vector2.one;
        }
    }

    /// <summary>
    /// /
    /// </summary>
    /// <param name="newRectTrans"></param>
    /// <param name="oldRectTrans"></param>
    private void ChangeSize(RectTransform newRectTrans, float sizeImage, RectTransform oldRectTrans = null)
    {
        if (oldRectTrans == newRectTrans)
        {
            Sequence repeat = DOTween.Sequence();
            repeat.Append(oldRectTrans.DOScale(1, 0.3f).SetEase(Ease.InBack)).
            Append(newRectTrans.DOScale(sizeImage, 0.4f).SetEase(Ease.OutBack));
            repeat.Play();
        }
        else
        {
            oldRectTrans.DOScale(1, 0.3f).SetEase(Ease.InBack);

            newRectTrans.DOScale(SizeSelectColor, 0.4f).SetEase(Ease.OutBack);
        }
    }

    public DataPalette GetColor()
    {
        //Debug.Log("Palette get color");
        return m_DataPalette;
    }

    public Material[] GetMaterials() => m_ArrayMaterials;
}