using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Zenject;
//using DG.Tweening;
using UnityEngine.UI;
using CandyCoded.HapticFeedback;

public class StickersController : MonoBehaviour
{
    private const int BLOCK = 0;
    private const int WAIT = 1;
    private const int TAP = 2;
    private const int COUNT_FOR_NEW_STICKER = 4;

    [Inject] private SaveLevelsComplete m_LevelsComplete;

    [SerializeField] private int m_CountLevels;
    [SerializeField] private int m_CountStickers;
    private int m_CountNewSticker = 0;

    [SerializeField] private HapticFeedbackController m_HapticFeedback;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject m_CellPrefabStickerGlow;
    //[SerializeField] private CellSticker m_CellSticker;
    //[SerializeField] private Image m_CellPrefabSticker;
    [SerializeField] private Sprite m_Glow;

    [SerializeField] private GameObject m_StickersContent;
    //[SerializeField] private GameObject[] m_Stickers;
    [SerializeField] private StickerData[] m_StickersSprite;
    private Color[] m_StickersColors;
    private GridLayoutGroup m_StickersContentGroup;
    private RectTransform m_StickersContentRT;
    private GameObject[] m_InstanceButtons;

    private bool isStartAnim;
    private System.Random rand = new System.Random();

    private void Awake()
    {
        m_LevelsComplete.LoadCountLevels();
        
        m_CountLevels = m_LevelsComplete.m_LevelsComplete;

        //Debug.Log(m_CountLevels + "m_CountLevels");
    }

    private void Start()
    {
        InitTapStickers();
        m_CountStickers = m_CountLevels / COUNT_FOR_NEW_STICKER;

        //Debug.Log(m_CountStickers + "   m_CountStickers");

        m_StickersContentGroup = m_StickersContent.GetComponent<GridLayoutGroup>();
        m_StickersContentRT = m_StickersContent.GetComponent<RectTransform>();

        FillGridStickers();

        if (m_CountNewSticker > 0)
        {
            m_Animator.SetBool("StartAnim", true);
            //Debug.Log("Start anim)");//start anim button
        }
    }

    private void FillGridStickers()
    {
        m_InstanceButtons = new GameObject[m_StickersSprite.Length];

        for (int i = 0; i < m_StickersSprite.Length; i++)
        {
            GameObject instanceButton = m_InstanceButtons[i];
            instanceButton = Instantiate(m_CellPrefabStickerGlow, m_StickersContent.transform);
            RectTransform instanceButtonRT = instanceButton.GetComponent<RectTransform>();
            CellSticker cellSticker = instanceButton.GetComponent<CellSticker>();
            Button levelButton = instanceButton.AddComponent<Button>();
            int p = i;
            levelButton.onClick.AddListener(() =>
                TapSticker(p, levelButton, cellSticker, instanceButtonRT));

            //Image instanceButtonImage = instanceButton.GetComponent<Image>();

            cellSticker.ImageSticker.transform.localScale = new Vector3(1f, 1f, 1f);
            if (i < m_CountStickers)
            {
                if (m_StickersSprite[i] != null)
                {
                    if (m_LevelsComplete.m_IsTapObjects[i] == BLOCK)
                    {
                        cellSticker.GlowSticker.SetActive(true);
                        cellSticker.GlowStickerImage.sprite = m_StickersSprite[i].StickerGlow;
                        levelButton.enabled = true;
                        m_LevelsComplete.m_IsTapObjects[i] = WAIT;

                        m_LevelsComplete.SaveTapStickers();
                        //WaitAnimation(instanceButtonRT);
                    }

                    if (m_LevelsComplete.m_IsTapObjects[i] == WAIT)
                    {
                    }

                    if (m_LevelsComplete.m_IsTapObjects[i] == TAP)
                    {
                    }

                    m_CountNewSticker = 0;
                    //instanceButtonImage.sprite = m_StickersSprite[i].StickerActive;
                    cellSticker.ImageSticker.sprite = m_StickersSprite[i].StickerActive;

                    cellSticker.setState(m_LevelsComplete.m_IsTapObjects[i]);
                }
            }
            else
            {
                if (m_StickersSprite[i] != null)
                {
                    cellSticker.ImageSticker.sprite = m_StickersSprite[i].StickerBlocked;
                    
                    levelButton.enabled = false;
                }
            }
        }

        StringBuilder textOutput = new StringBuilder();

        for (int j = 0; j < m_StickersSprite.Length; j++)
        {
            textOutput.Append($"Sticker {j}) Now is ");

            if (m_LevelsComplete.m_IsTapObjects[j] == BLOCK)
            {
                textOutput.Append($"BLOCK [{BLOCK}]");
            }

            if (m_LevelsComplete.m_IsTapObjects[j] == WAIT)
            {
                m_CountNewSticker++;
                textOutput.Append($"WAIT [{WAIT}]");
            }

            if (m_LevelsComplete.m_IsTapObjects[j] == TAP)
            {
                textOutput.Append($"TAP [{TAP}]");
            }

            textOutput.Append("\n");
        }

        //Debug.Log(textOutput.ToString());

        ResizeStickersContent();
    }

    private void ResizeStickersContent()
    {
        int tempLength = m_StickersSprite.Length / 2;
        tempLength = tempLength > 0 ? m_StickersSprite.Length + 1 : m_StickersSprite.Length;
        Vector2 newContentSize = new Vector2(((m_StickersContentGroup.cellSize.x + m_StickersContentGroup.spacing.x) * tempLength) /
            m_StickersContentGroup.constraintCount, m_StickersContentRT.sizeDelta.y);

        m_StickersContentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newContentSize.x);
        m_StickersContentRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newContentSize.y);
    }
    /*
    private void WaitAnimation(RectTransform rt)
    {
        Tween m_Tween = rt.DOScale(new Vector3(0.9f, 0.9f, 0.9f), 1.0f).SetLoops(-1, LoopType.Yoyo);
        m_Tween.Play();
    }
     */

    private void TapSticker(int j, Button levelButton, CellSticker cellSticker, RectTransform rt)
    {
        m_HapticFeedback.MediumFeedback();

        if (m_LevelsComplete.m_IsTapObjects[j] == WAIT)
        {
            m_CountNewSticker--;
            cellSticker.GetAnimator().Play("Idle");
            cellSticker.setState(TAP);
        }

        if (cellSticker.getState() == TAP)
        {
            cellSticker.GetAnimator().Play($"rClick{rand.Next(1, 4)}");
        }

        //rt.DOKill();
        cellSticker.GlowSticker.SetActive(false);
        m_LevelsComplete.m_IsTapObjects[j] = TAP;
        //levelButton.enabled = false;
        m_LevelsComplete.SaveTapStickers();


        if (m_CountNewSticker == 0)
        {

            isStartAnim = false;
            m_Animator.SetBool("StartAnim", false);

            //Debug.Log("Stop anim)");//stop anim button
        }
        else
        {
            isStartAnim = true;
            m_Animator.SetBool("StartAnim", true);

        }
    }


    private void InitTapStickers()
    {
        m_LevelsComplete.LoadTapStickers();
        
        if (m_LevelsComplete.m_IsTapObjects == null
           || m_LevelsComplete.m_IsTapObjects.Length != m_StickersSprite.Length)
        {
            m_LevelsComplete.m_IsTapObjects = new int[m_StickersSprite.Length];
            for (int i = 0; i < m_LevelsComplete.m_IsTapObjects.Length; i++)
            {
                m_LevelsComplete.m_IsTapObjects[i] = BLOCK;
            }
            for (int j = 0; j < m_StickersSprite.Length; j++)
            {
                //Debug.Log(m_LevelsComplete.m_IsTapObjects[j] + "   isTapStickInit");
            }

            m_LevelsComplete.SaveTapStickers();
        }
        
    }

    public void RestartAnimator()
    {
        if (m_CountNewSticker == 0)
        {
            m_Animator.SetBool("StartAnim", false);

            //Debug.Log("Stop anim)");//stop anim button
        }
        else
        {
            m_Animator.SetBool("StartAnim", true);
        }
    }

    public void OnClickStickers()
    {
        if (m_CountNewSticker > 0)
        {
            AnalyticsHelper.Instance.SendStickersEntered(true);
        }
        else
        {
            AnalyticsHelper.Instance.SendStickersEntered(false);
        }
    }

    public int getCountLevels()
    {
        return m_CountLevels;
    }

    public int getCountStickers()
    {
        return m_CountStickers;
    }
    public int getCountNewStickers()
    {
        return m_CountNewSticker;
    }

    public void DebugAddlevel()
    {
        m_LevelsComplete.m_LevelsComplete++;
        m_LevelsComplete.SaveCountLevels();
    }
}
