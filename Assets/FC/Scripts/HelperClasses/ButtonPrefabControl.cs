using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPrefabControl : MonoBehaviour
{
    [SerializeField] private GameObject m_BackLight;
    [SerializeField] private GameObject m_SubstrateLight;
    [SerializeField] private GameObject m_SubstrateDark;
    [SerializeField] private GameObject m_ShadowImage;

    [SerializeField] private float m_Size;

    public void SetSizeImage(float sizeX, float sizeY, float aspectRatioX, float aspectRatioY)
    {
        var rectBackLight = m_BackLight.GetComponent<RectTransform>();
        var rectSubstrateLight = m_SubstrateLight.GetComponent<RectTransform>();
        var rectSubstrateDark = m_SubstrateDark.GetComponent<RectTransform>();
        var rectShadowImage = m_ShadowImage.GetComponent<RectTransform>();

        rectBackLight.sizeDelta = new Vector2(sizeX * aspectRatioX, sizeY * aspectRatioY);
        rectSubstrateLight.sizeDelta = new Vector2(sizeX, sizeY);
        rectSubstrateDark.sizeDelta = new Vector2(sizeX, sizeY);
        rectShadowImage.sizeDelta = new Vector2(sizeX, sizeY);
    }

    public void SetActiveNewLevel()
    {
        m_BackLight.SetActive(true);
        m_SubstrateLight.SetActive(true);
        m_SubstrateDark.SetActive(false);
        m_ShadowImage.SetActive(false);
    }

    public void SetActiveLevel()
    {
        m_BackLight.SetActive(false);
        m_SubstrateLight.SetActive(true);
        m_SubstrateDark.SetActive(false);
        m_ShadowImage.SetActive(false);
    }

    public void SetLockLevel()
    {
        m_BackLight.SetActive(false);
        m_SubstrateLight.SetActive(false);
        m_SubstrateDark.SetActive(true);
        m_ShadowImage.SetActive(true);
    }

    public GameObject GetParentActiveObject() => m_SubstrateLight;
    public GameObject GetParentLockObject() => m_SubstrateDark;
}
