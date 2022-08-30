using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestButtonAnim : Button
{
    //private List<Image> m_Image = new List<Image>();
    private CreateColoringPage m_CreateColoringPage;
    //private ColoringPage m_ColoringPage;
    private GameManager m_GameManager;
    //private SaveManager m_SaveManager;
    private SoundManager m_SoundManager;
    private AnalyticsHelper m_AnaliticsHelper;
    private Button m_ButtonPremium;
    private UI m_UI;
    private int m_NumbPage;
    private bool m_isOpen;

    protected override void Start()
    {
        base.Start();
        onClick.AddListener(MyOnClick);
        m_AnaliticsHelper = AnalyticsHelper.Instance;
    }

    private void MyOnClick()
    {
        if (m_isOpen)
        {
            m_SoundManager.OnClickButton();
            m_UI.OnClickCreatePage();
            m_UI.NumbUpdate = m_NumbPage;
            m_CreateColoringPage.CreatePage(m_NumbPage, true);
            m_AnaliticsHelper.SendLevelStarted(0, m_NumbPage);
            //Debug.Log("????? ?????????? ????? " + m_NumbPage);
        }
        else
        {
            m_SoundManager.OnClickButton();

            m_ButtonPremium.onClick.Invoke();
        }
    }
    
    public void Initialization(GameManager gameManager, List<Image> images, int numbPage, bool isOpen)
    {
        m_GameManager = gameManager;
        m_NumbPage = numbPage;
        m_CreateColoringPage = m_GameManager.CreateColoringPage;
        m_SoundManager = m_GameManager.SoundManager;
        //m_SaveManager = m_GameManager.SaveManager;
        m_UI = m_GameManager.UI;
        m_isOpen = isOpen;
        m_ButtonPremium = m_GameManager.ButtonPremium;
        //m_ColoringPage = gameManager.ColoringPage[numbPage];
        //m_Image.Clear();
        //m_Image.AddRange(images.ToArray());
    }

}
