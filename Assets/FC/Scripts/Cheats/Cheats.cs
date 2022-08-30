using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    [SerializeField] private GameManager m_GameManager;

    private void LevelOpeningCheatsOn()
    {
        for (int numbPage = 0; numbPage < m_GameManager.ColoringPage.Length; numbPage++)
        {
            m_GameManager.CreateButtonImage.UpdateImagePage(numbPage);
        }
    }

    private void LevelOpeningCheatsOff()
    {
        m_GameManager.CreateButtonImage.CreateButtons(m_GameManager);
    }
}
