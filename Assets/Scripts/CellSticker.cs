using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSticker : MonoBehaviour
{

    [SerializeField] private GameObject m_GlowSticker;
    //[SerializeField] private GameObject m_ImageSticker;

    [SerializeField] private Image m_GlowStickerImage;
    [SerializeField] private Image m_ImageSticker;
    [SerializeField] private Animator animator;
    private int m_state = 0;

    //private Image m_ImageSticker;
    public GameObject GlowSticker { get { return m_GlowSticker; } set { m_GlowSticker = value; } }

    public Image GlowStickerImage { get { return m_GlowStickerImage; } set { m_GlowStickerImage = value; } }

    public Image ImageSticker { get { return m_ImageSticker; } set { m_ImageSticker = value; } }

    public void setState(int state)
    {
        m_state = state;
    }

    public int getState()
    {
        return m_state;
    }

    public void OnEnable()
    {
        if (m_state == 1)
        {
            animator.Play("wait");
        }
    }

    public Animator GetAnimator()
    {
        return animator;
    }

        /*private void Start()
        {
            m_ImageSticker = GetComponent<Image>();
        }*/

    }
