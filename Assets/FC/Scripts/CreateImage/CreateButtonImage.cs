using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CreateButtonImage : MonoBehaviour
{
    [SerializeField] private GameObject m_PrefabSelectButton;
    [SerializeField] private GameObject m_PrefabImage;
    [SerializeField] private GameObject m_PrefabLockLevel;
    [SerializeField] private GameObject m_PrefabParentImage;
    [SerializeField] private GameObject m_ButtonParent;
    [SerializeField] private CanvasScaler m_CanvasScaler;

    private float m_SizeImage;

    private GameObject[] m_Child;
    private GameObject[] m_Button;
    private GameManager m_gameManager;
    private List<Image> m_Image;

    public void CreateButtons(GameManager gameManager)
    {
        m_Image = new List<Image>();
        m_gameManager = gameManager;
        m_Child = new GameObject[m_gameManager.ColoringPage.Length];
        m_Button = new GameObject[m_gameManager.ColoringPage.Length];

        if (m_gameManager.ColoringPage == null)
        {
            throw new Exception("Colouring page not added.");
        }

        float size = 100f;
        Image prefabButtonImage = m_PrefabSelectButton.GetComponent<ButtonPrefabControl>().GetParentActiveObject().GetComponent<Image>();

        float buttonImageSize = Mathf.Min(prefabButtonImage.sprite.rect.size.x, prefabButtonImage.sprite.rect.size.y);
        float aspectRatio = size / buttonImageSize;

        for (var numbPage = 0; numbPage < m_gameManager.ColoringPage.Length; numbPage++)
        {
            bool isOpen;
            //Debug.Log($"? {m_gameManager.ColoringPage[numbPage].name} ????????? ????? {numbPage}");
            m_Image.Clear();

            m_Button[numbPage] = Instantiate(m_PrefabSelectButton, m_ButtonParent.transform, false);

            var parentControl = m_Button[numbPage].GetComponent<ButtonPrefabControl>();

            parentControl.SetSizeImage(500f, 377f, 1.14f, 1.17f);

            //parentControl.SetActiveNewLevel();

            m_SizeImage = m_gameManager.ColoringPage[numbPage].ImageSize + 0.2f;

            var uploadedData = m_gameManager.SaveManager.LoadPage(m_gameManager.ColoringPage[numbPage].name);

            GameObject child;

            if (uploadedData != null && numbPage < m_gameManager.NumbersOfOpenLevels)
            {
                child = Instantiate(m_PrefabParentImage, parentControl.GetParentActiveObject().transform, false);
                child.GetComponent<RectTransform>().localScale = new Vector2(aspectRatio * m_SizeImage, aspectRatio * m_SizeImage);

                CreateImage(m_gameManager.ColoringPage[numbPage], child.transform, uploadedData);

                for (var numbColor = 0; numbColor < uploadedData.colors.Length; numbColor++)
                {
                    m_Image[numbColor].color = uploadedData.colors[numbColor];
                }

                isOpen = true;

                parentControl.SetActiveLevel();
            }
            else if(numbPage < m_gameManager.NumbersOfOpenLevels)
            {
                child = Instantiate(m_PrefabParentImage, parentControl.GetParentActiveObject().transform, false);
                child.GetComponent<RectTransform>().localScale = new Vector2(aspectRatio * m_SizeImage, aspectRatio * m_SizeImage);

                isOpen = true;

                CreateStartImage(m_gameManager.ColoringPage[numbPage], child.transform, isOpen);

                parentControl.SetActiveNewLevel();
            }
            else if(uploadedData != null && numbPage >= m_gameManager.NumbersOfOpenLevels)
            {
                child = Instantiate(m_PrefabParentImage, parentControl.GetParentLockObject().transform, false);
                child.GetComponent<RectTransform>().localScale = new Vector2(aspectRatio * m_SizeImage, aspectRatio * m_SizeImage);

                CreateImage(m_gameManager.ColoringPage[numbPage], child.transform, uploadedData);

                for (var numbColor = 0; numbColor < uploadedData.colors.Length; numbColor++)
                {
                    m_Image[numbColor].color = uploadedData.colors[numbColor];
                }

                isOpen = false;

                parentControl.SetLockLevel();
            }
            else
            {
                child = Instantiate(m_PrefabParentImage, parentControl.GetParentLockObject().transform, false);
                child.GetComponent<RectTransform>().localScale = new Vector2(aspectRatio * m_SizeImage, aspectRatio * m_SizeImage);

                isOpen = false;

                CreateStartImage(m_gameManager.ColoringPage[numbPage], child.transform, isOpen);

                parentControl.SetLockLevel();
            }

            m_Button[numbPage].AddComponent<TestButtonAnim>().Initialization(m_gameManager, m_Image, numbPage, isOpen);
            //Debug.Log("????? ????? ??? ???????? " + numbPage);
            m_Child[numbPage] = child;

        }

        m_Image.Clear();

    }

    private void CreateImage(ColoringPage page, Transform parent, SaveManager.DataPage dataPage)
    {
        //Debug.Log($"????? ? {page.name} = {page.m_Arts.Length}");
        for (var numbImg = 0; numbImg < page.m_Arts.Length; numbImg++)
        {
            //Debug.Log("Loaded");

            CreateImagePage(parent, page.m_Arts[numbImg].PositionUIImage, page.m_Arts[numbImg].Sprite,
                dataPage.colors[numbImg], dataPage.numbMat[numbImg]);

            //Debug.Log(numbImg);
        }
    }

    private void CreateStartImage(ColoringPage page, Transform parent, bool isOpen)
    {
        //Debug.Log("No loaded");

        CreateImagePage(parent, page.PositionStartImage, page.StartSprite, Color.white, 0);
    }

    private void CreateImagePage(Transform parent, Vector3 position, Sprite sprite, Color color, int numbMaterial)
    {
        var imageObject = Instantiate(m_PrefabImage, parent, false);
        imageObject.GetComponent<RectTransform>().transform.localPosition = position;

        var image = imageObject.GetComponent<Image>();
        image.sprite = sprite;
        image.color = color;
        image.material = m_gameManager.Palette.GetMaterials()[numbMaterial];
        image.SetNativeSize();

        var canvas = imageObject.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = Mathf.Abs((int)position.z);

        m_Image.Add(image);
    }

    public void UpdateImagePage(int numbPage)
    {
        if (numbPage < m_gameManager.ColoringPage.Length)
        {
            //Debug.Log($"Update page number {numbPage}");
            var uploadedData = m_gameManager.SaveManager.LoadPage(m_gameManager.ColoringPage[numbPage].name);

            if (uploadedData != null)
            {
                foreach (Transform child in m_Child[numbPage].transform)
                {
                    Destroy(child.gameObject);
                }

                CreateImage(m_gameManager.ColoringPage[numbPage], m_Child[numbPage].transform, uploadedData);
                m_Button[numbPage].GetComponent<ButtonPrefabControl>().SetActiveLevel();
            }
            else
            {
                CreateStartImage(m_gameManager.ColoringPage[numbPage], m_Child[numbPage].transform, true);
                m_Button[numbPage].GetComponent<ButtonPrefabControl>().SetActiveNewLevel();
            }

        }
    }
}