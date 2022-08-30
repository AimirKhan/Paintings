using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CandyCoded.HapticFeedback;
using Zenject;

public class LevelSelectorController : MonoBehaviour
{
    [Inject] SaveSerial m_SaveSerial;
    [Inject] LevelController m_LevelController;
    [Header("Common variables")]
    [SerializeField] private GameObject m_LevelsContainer;
    public GameObject LevelsContainer => m_LevelsContainer;
    [SerializeField] private GameObject m_LevelCellPrefab;
    [SerializeField] private Button m_BtPremium;
    [SerializeField] private int m_DMGameSceneIndex;
    [Header("Level preview tablet")]
    [SerializeField] private float m_ImageScale = 1;
    [SerializeField] private float m_PixelImageScale = .5f;
    [SerializeField] private Sprite m_UnresolvedLevelTable;
    [SerializeField] private Sprite m_LockedLevelTable;
    [Header("Status values")]
    [SerializeField] private float m_LockIconScale = .5f;
    [SerializeField] private float m_SuccessIconScale = .5f;
    [SerializeField] private Vector2 m_LockIconPos;
    [SerializeField] private Vector2 m_SuccessIconPos;
    [SerializeField] private Sprite m_LockIconSprite;
    [SerializeField] private Sprite m_SuccessIconSprite;

    private GameObject[] m_InstanceButtons;
    private int m_LevelsLength;
    private RectTransform m_LevelContainerRT;
    private GridLayoutGroup m_LevelContainerLayoutGroup;

    private void Start()
    {
        Initilize();
    }

    public void Initilize()
    {
        m_LevelsLength = m_LevelController.LevelsArray.Length;
        m_LevelContainerRT = m_LevelsContainer.GetComponent<RectTransform>();
        m_LevelContainerLayoutGroup = m_LevelsContainer.GetComponent<GridLayoutGroup>();
        // Levels count
        FillLevelsToPanel();
    }

    private void FillLevelsToPanel()
    {
        Vector3 imageScale = Vector2.one * m_ImageScale;
        Vector3 pixelImageScale = Vector2.one * m_PixelImageScale;
        m_InstanceButtons = new GameObject[m_LevelsLength];

        for (int i = 0; i < m_LevelsLength; i++)
        {
            GameObject instanceButton = m_InstanceButtons[i];
            instanceButton = Instantiate(m_LevelCellPrefab, m_LevelsContainer.transform);
            int p = i; // For Listener, because i hold last number
            Button levelButton = instanceButton.AddComponent<Button>();
            Image instanceButtonImage = instanceButton.GetComponent<Image>();
            // Button component
            //Button levelButton = instanceButton.GetComponent<Button>();
            levelButton.transition = 0;
            levelButton.onClick.AddListener(() =>
                StartSceneLevel(p));
            levelButton.onClick.AddListener(() =>
                HapticFeedback.LightFeedback());
            //TODO Next code need to move separate function
            // Level Image
            GameObject tablet = Instantiate(m_LevelCellPrefab);
            tablet.transform.SetParent(instanceButton.transform, false);
            Image tabletImage = tablet.GetComponent<Image>();
            tabletImage.raycastTarget = false;
            // Lock Icon
            GameObject statusIcon = Instantiate(m_LevelCellPrefab);
            statusIcon.transform.SetParent(instanceButton.transform, false);
            Image statusIconImage = statusIcon.GetComponent<Image>();
            // Unresolved level
            if (m_SaveSerial.passedLevelsToSave[i] == 0)
            {
                instanceButtonImage.sprite = m_UnresolvedLevelTable;
                tabletImage.transform.localScale = pixelImageScale;
                tabletImage.sprite = m_LevelController.LevelsArray[i].PixelPicture;
                tabletImage.rectTransform.sizeDelta =  m_LevelController.LevelsArray[i].PixelPicture.rect.size;
                statusIcon.SetActive(false);
                //tabletImage.material = m_GrayscaleMaterial;
            }
            // Resolved level
            else if (m_SaveSerial.passedLevelsToSave[i] == 1)
            {
                if (m_LevelController.LevelsArray[i].FlatPicture == null)
                {
                    tabletImage.transform.localScale = pixelImageScale;
                    tabletImage.sprite = m_LevelController.LevelsArray[i].PixelPicture;
                    tabletImage.rectTransform.sizeDelta = m_LevelController.LevelsArray[i].PixelPicture.rect.size;
                }
                else
                {
                    tabletImage.transform.localScale = imageScale;
                    tabletImage.sprite = m_LevelController.LevelsArray[i].FlatPicture;
                    tabletImage.rectTransform.sizeDelta = m_LevelController.LevelsArray[i].FlatPicture.rect.size;
                }
                // Set lock icon pos
                SetAnchors(statusIconImage);
                statusIcon.SetActive(true);
                statusIconImage.transform.localScale *= m_SuccessIconScale;
                statusIconImage.rectTransform.anchoredPosition = m_SuccessIconPos;
                statusIconImage.sprite = m_SuccessIconSprite;
                statusIconImage.rectTransform.sizeDelta = m_SuccessIconSprite.rect.size;
            }
            // Locked level
            if (!Purchaser.isPremium() & m_SaveSerial.premiumLevelToSave[i] == 1)
            {
                instanceButtonImage.sprite = m_LockedLevelTable;
                tabletImage.transform.localScale = pixelImageScale;
                tabletImage.sprite = m_LevelController.LevelsArray[i].PixelPicture;
                tabletImage.rectTransform.sizeDelta =  m_LevelController.LevelsArray[i].PixelPicture.rect.size;
                // Set lock icon pos
                SetAnchors(statusIconImage);
                statusIcon.SetActive(true);
                statusIconImage.transform.localScale *= m_LockIconScale;
                statusIconImage.rectTransform.anchoredPosition = m_LockIconPos;
                statusIconImage.sprite = m_LockIconSprite;
                statusIconImage.rectTransform.sizeDelta = m_LockIconSprite.rect.size;
            }
        }
    }               

    public void StartSceneLevel(int levelNumber)
    {
        if (Purchaser.isPremium() | m_SaveSerial.premiumLevelToSave[levelNumber] == 0)
        {
            m_LevelController.CurrentLevel = levelNumber;
            SceneManager.LoadScene(m_DMGameSceneIndex);
        }
        else
        {
            m_BtPremium.onClick.Invoke();
        }
    }

    private void InitLevelsStates()
    {
        m_SaveSerial.LoadGame();
        for (int i = 0; i < m_LevelController.LevelsArray.Length; i++)
        {
            if(m_SaveSerial.passedLevelsToSave == null
                || m_SaveSerial.passedLevelsToSave.Length != m_LevelController.LevelsArray.Length)
            {
                m_SaveSerial.passedLevelsToSave = new int[m_LevelController.LevelsArray.Length];
                m_SaveSerial.SaveGame();
            }
        }
    }

    private void SetAnchors(Image image)
    {   
        image.rectTransform.anchorMin = new Vector2(1, 0);
        image.rectTransform.anchorMax = new Vector2(1, 0);
        image.rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
}
