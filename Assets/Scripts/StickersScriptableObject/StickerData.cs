using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sticker_", menuName = "Create Sticker")]
public class StickerData : ScriptableObject
{
    [SerializeField] private int m_StickerId;
    [SerializeField] private Sprite m_StickerBlocked;
    [SerializeField] private Sprite m_StickerGlow;
    [SerializeField] private Sprite m_StickerActive;


    public Sprite StickerBlocked => m_StickerBlocked;
    public Sprite StickerGlow => m_StickerGlow;
    public Sprite StickerActive => m_StickerActive;
    public int StickerId => m_StickerId;
}
