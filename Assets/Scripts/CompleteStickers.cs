using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CompleteStickers : MonoBehaviour
{
    [SerializeField] private GameObject m_StickerPanel;
    [SerializeField] private RectTransform m_PositionButtonHome;
    [SerializeField] private StickerData[] m_StickersSprites;
    [SerializeField] private Button m_ButtonSticker;
    [SerializeField] private GameObject m_Rays;
    private RectTransform m_ButtonRT;
    private RectTransform m_RaysRT;
    private SpriteRenderer m_ButtonSprite;
    private Image m_StickerPanelImage;
    Sequence m_Sequence;

    public void OnClickTapStickerButton()
    {

        m_RaysRT = m_Rays.GetComponent<RectTransform>();
        m_ButtonRT = m_ButtonSticker.GetComponent<RectTransform>();
        m_ButtonSprite = m_ButtonSticker.GetComponent<SpriteRenderer>();
        m_StickerPanelImage = m_StickerPanel.GetComponent<Image>();
        m_Sequence = DOTween.Sequence();
        m_Sequence.Append(m_ButtonRT
            .DOScale(new Vector3(0.0f, 0.0f, 0.0f), 1.3f)
            ).Join(m_ButtonRT
            .DOAnchorPos(new Vector2(m_PositionButtonHome.localPosition.x, m_PositionButtonHome.localPosition.y), 1.3f).SetEase(Ease.OutCirc)
            ).Join(m_StickerPanelImage
            .DOFade(0f, 0.5f)//.SetEase(Ease.OutCirc)
            ).Join(m_RaysRT
            .DOScale(new Vector3(0.0f, 0.0f, 0.0f), 1.3f)//.SetEase(Ease.OutCirc)
            );
        m_Sequence.Play().OnComplete(() => m_StickerPanel.SetActive(false));
        
    }

    /*private void InactiveStickPanel()
    {
        m_StickerPanel.SetActive(false);
    }*/

    public void SetCurrentSticker(int id)
    {
        Debug.Log(id + " id");
        m_StickerPanel.SetActive(true);
        m_ButtonRT = m_ButtonSticker.GetComponent<RectTransform>();
        m_ButtonSticker.GetComponent<Image>().sprite = m_StickersSprites[id - 1].StickerActive;
        m_ButtonRT.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        m_ButtonRT.localScale = new Vector3(1.0f, 1.0f, 1.0f);
    }


}
