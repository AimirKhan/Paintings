using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

public class GameEndEvent : UnityEvent<ColoringPage, SaveArray, bool, int> { }
public class CreateColoringPage : MonoBehaviour
{
    public Vector2 Borders = new Vector2(0.5f, 0.9f);

    public GameEndEvent EventGameEnd;

    [SerializeField] private GameObject m_ParentImg;
    [SerializeField] private GameObject m_ParentDrPan;
    [SerializeField] private GameObject m_ImagePrefab;
    [SerializeField] private GameObject m_DrawPanelPrefab;
    [SerializeField] private Particle m_Particle;

    [SerializeField] private Palette m_Palette;
    [SerializeField] private ScreenSize m_ScreenSize;

    private float m_ImageSize;
    private Shading[] m_Shading;
    private Color[] m_Colors;
    private int[] m_NumbsMaterial;
    private Color m_StartColor = new Color(1, 0.99f, 1);
    //private bool[] m_IsPainted;

    private Stack<ColoringData> m_PouringSequence;
    private GameManager m_GameManager;
    private ColoringPage m_ColoringPage;
    private SaveManager.DataPage m_DataPage;
    private SoundManager m_SoundManager;
    private GameObject m_DrawPanel;

    private DataShading m_DataShading;
    private Color m_ColorImage;
    private int m_NumbMaterialImage;
    private bool m_IsFirstEnd;
    private bool m_IsStickerReceived;
    private int m_CountUnshading;
    private int m_NumbPage;
    private DateTime m_StartTime;

    private void Awake()
    {
        EventGameEnd = new GameEndEvent();

        m_GameManager = GetComponent<GameManager>();

        m_PouringSequence = new Stack<ColoringData>();
    }

    private void PrepareLevel()
    {
        m_PouringSequence.Clear();

        m_Palette.ResetColor();

        DeleteImage();

        if (m_DrawPanel != null)
        {
            Destroy(m_DrawPanel);
        }

        m_CountUnshading = 0;

        m_IsFirstEnd = false;
    }

    public void CreatePage(int numbPage, bool load)
    {
        m_StartTime = DateTime.Now;

        if (m_NumbPage >= m_GameManager.ColoringPage.Length)
        {
            m_GameManager.UI.OnFinishGame();

            return;
        }

        PrepareLevel();

        //Debug.Log($"m_PouringSequence = {m_PouringSequence.Count}");

        m_NumbPage = numbPage;
        
        m_ColoringPage = m_GameManager.ColoringPage[numbPage];

        m_DataPage = m_GameManager.SaveManager.LoadPage(m_ColoringPage.name);

        m_SoundManager = m_GameManager.SoundManager;

        m_ImageSize = m_ColoringPage.ImageSize;

        for (int i = 0; i < m_ColoringPage.m_Arts.Length; i++)
        {
            if (!m_ColoringPage.m_Arts[i].isShading)
            {
                m_CountUnshading++;
            }
        }

        //Debug.Log("m_countUnshading = " + m_CountUnshading);

        m_Colors = new Color[m_ColoringPage.m_Arts.Length];
        m_NumbsMaterial = new int [m_ColoringPage.m_Arts.Length];
        m_Shading = new Shading[m_ColoringPage.m_Arts.Length];

        Vector2 screenSizeInUnit = m_ScreenSize.GetScreenSizeUnit();

        float sizeDrawPanel = Mathf.Min(screenSizeInUnit.x * Borders.x, screenSizeInUnit.y * Borders.y);

        m_DrawPanel = Instantiate(m_DrawPanelPrefab, m_ParentDrPan.transform, false);
        var spriteRenderer = m_DrawPanel.GetComponent<SpriteRenderer>();

        float pixelPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        Vector2 panelSpriteSize = spriteRenderer.sprite.rect.size;
        Vector2 aspectRatio = new Vector2(pixelPerUnit / panelSpriteSize.x, pixelPerUnit / panelSpriteSize.y);

        m_DrawPanel.transform.localScale = new Vector2(sizeDrawPanel * aspectRatio.x, sizeDrawPanel * aspectRatio.y);

        m_ParentImg.transform.localScale = m_DrawPanel.transform.localScale * m_ImageSize;

        if (m_DataPage != null && load) //<load> need for reset level without load data
        {
            int whiteImageCount = 0;
            for (var imgNumb = 0; imgNumb < m_ColoringPage.m_Arts.Length; imgNumb++)
            {                
                m_ColorImage = m_DataPage.colors[imgNumb];
                m_NumbMaterialImage = m_DataPage.numbMat[imgNumb];
                m_Colors[imgNumb] = m_DataPage.colors[imgNumb];
                m_NumbsMaterial[imgNumb] = m_DataPage.numbMat[imgNumb];
                m_IsStickerReceived = m_DataPage.isStickerReceived;

                if (m_Colors[imgNumb] == m_StartColor && m_NumbsMaterial[imgNumb] == 0)
                {
                    whiteImageCount++;
                }
                //Debug.Log(whiteImageCount);

                    CreateImagePage(m_ColoringPage.m_Arts[imgNumb].Sprite, m_ColoringPage.m_Arts[imgNumb].PositionImage,
                m_ColoringPage.m_Arts[imgNumb].isShading, imgNumb, m_ColorImage, m_NumbMaterialImage);
            }

            if (whiteImageCount > m_CountUnshading)
            {
                m_IsFirstEnd = true;
            }
            else
            {
                m_IsFirstEnd = false;
            }
            //Debug.Log($"whiteImageCount = {whiteImageCount}, m_CountUnshading = {m_CountUnshading}");
        }
        else
        {
            m_IsFirstEnd = true;

            m_IsStickerReceived = false;

            for (var imgNumb = 0; imgNumb < m_ColoringPage.m_Arts.Length; imgNumb++)
            {
                m_ColorImage = m_StartColor;
                m_NumbMaterialImage = 0;
                m_Colors[imgNumb] = m_StartColor;
                m_NumbsMaterial[imgNumb] = 0;

                CreateImagePage(m_ColoringPage.m_Arts[imgNumb].Sprite, m_ColoringPage.m_Arts[imgNumb].PositionImage,
                m_ColoringPage.m_Arts[imgNumb].isShading, imgNumb, m_ColorImage, m_NumbMaterialImage);
            }
        }
    }

    
    private void CreateImagePage(Sprite sprite, Vector3 position, bool isShading, int imgNumb, Color color, int numbMaterial)
    {
        var image = Instantiate(m_ImagePrefab, m_ParentImg.transform, false);

        var shading = image.GetComponent<Shading>();

        shading.PaintedOver.AddListener(FingerPressing);

        m_Shading[imgNumb] = shading;

        shading.Init(sprite, m_Palette, position, isShading, imgNumb, color, numbMaterial, m_SoundManager, m_Particle);
    }

    private void FingerPressing(int id, DataShading dataShading, bool isPainted)
    {
        m_Colors[id] = dataShading.Color;
        m_NumbsMaterial[id] = dataShading.NumbMaterial;
        //Debug.Log($"id = {id}, color = {m_Colors[id]}, isPainted = {isPainted}");
        if (m_Shading[id].CheckOldColor() && isPainted)
        {
            //Debug.Log("Add");
            ColoringData coloringData = new ColoringData(id, m_Shading[id].GetOldColor().NumbMaterial,
                m_Shading[id].GetOldColor().Color);
            
            m_PouringSequence.Push(coloringData);

            //Debug.Log($"m_PouringSequence = {m_PouringSequence.Count}, {id}");
        }

        //m_IsPainted[id] = true;
        if (m_IsFirstEnd)
        {
            int whiteImageCount = 0;

            for (var i = 0; i < m_Colors.Length; i++)
            {
                //if (m_Colors[i] == Color.clear)
                //{
                //    return;
                //}

                if (m_Colors[i] == m_StartColor && m_NumbsMaterial[i] == 0)
                {
                    whiteImageCount++;

                    //Debug.Log($"whiteImageCount = {whiteImageCount}, m_CountUnshading = {m_CountUnshading}, imageID = {i}");
                    if (whiteImageCount > m_CountUnshading)
                    {
                        m_GameManager.UI.StopAnimationExitButton();

                        return;
                    }
                }
            }

            if (!m_IsStickerReceived)
            {
                TimeSpan timer = DateTime.Now.Subtract(m_StartTime);
                int time = (timer.Hours * 60 * 60) + (timer.Minutes * 60) + timer.Seconds;

                m_GameManager.StickersCounter.UpdateLevelsStickers(0, m_NumbPage, 0, time);
                m_IsStickerReceived = true;
            }

            m_GameManager.UI.PlayAnimationExitButton();
            //m_IsFirstEnd = false;
        }
    }

    public void CancelFill()
    {
        //Debug.Log("m_PouringSequence.Count = " + m_PouringSequence.Count);
        if (m_PouringSequence.Count > 0)
        {
            ColoringData coloringData = m_PouringSequence.Pop();
            m_DataShading = new DataShading(coloringData.Color, coloringData.NumbMaterial);

            m_Shading[coloringData.ID].ResetColor(m_DataShading);

            m_Colors[coloringData.ID] = coloringData.Color;

            m_NumbsMaterial[coloringData.ID] = coloringData.NumbMaterial;

            FingerPressing(coloringData.ID, m_DataShading, false);
        }


    }

    public void OnClickEndGameButton()
    {
        int whiteImageCount = 0;

        if (m_IsFirstEnd)
        {
            for (var i = 0; i < m_Colors.Length; i++)
            {
                if (m_Colors[i] == m_StartColor && m_NumbsMaterial[i] == 0)
                {
                    whiteImageCount++;

                    if (whiteImageCount > m_CountUnshading)
                    {
                        m_IsFirstEnd = false;
                        //return;
                    }
                }
            }
        }

        //m_isFirstEnd = false;
        //Debug.Log(m_Colors);
        var saveArray = new SaveArray(m_NumbsMaterial, m_IsStickerReceived, m_Colors);
        EventGameEnd.Invoke(m_ColoringPage, saveArray, m_IsFirstEnd, m_NumbPage);

        if (!m_IsStickerReceived)
        {
            AnalyticsHelper.Instance.SendLevelAborted(0, m_NumbPage);
        }
        //DeleteImage();
    }

    public void DeleteImage()
    {
        foreach (Transform child in m_ParentImg.transform)
        {
            Destroy(child.gameObject);
        }

        if (m_Shading != null)
        {
            for (var i = 0; i < m_Shading.Length; i++)
            {
                m_Shading[i].PaintedOver.RemoveListener(FingerPressing);
            }
        }
    }

    private void OnDestroy()
    {
        if (m_Shading != null)
        {
            for (var i = 0; i < m_Shading.Length; i++)
            {
                m_Shading[i].PaintedOver.RemoveListener(FingerPressing);
            }
        }
    }

}