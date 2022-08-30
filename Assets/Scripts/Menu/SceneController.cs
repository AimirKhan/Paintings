using UnityEngine.SceneManagement;
using UnityEngine;
using Zenject;
using System.Collections;

public class SceneController : MonoBehaviour
{
    // [SerializeField] private int[] m_GameScenes;
    //[SerializeField] private Image m_FadeImage;
    [Inject] private SaveLevelsComplete m_LevelsComplete;
    private ScrollViewController scrollViewController;
    [SerializeField] private GameObject m_StartMenuPanel;
    [SerializeField] private GameObject m_StickersPanel;
    [SerializeField] private GameObject m_SettingsPanel;

    //[SerializeField] private GameObject m_PremiumPanel;

    //private int m_SceneIndex = 0;

    void Start()
    {
        scrollViewController = GetComponentInParent<ScrollViewController>();
    }

    public void OnClick(int ind)
    {
        //scrollViewController.OnClickButtonScene(ind);
        SwitchToScene(ind);
    }

    public void OnClickStikerButton()
    {
        m_StartMenuPanel.SetActive(false);
        m_SettingsPanel.SetActive(false);
        m_StickersPanel.SetActive(true);

        //m_PremiumPanel.SetActive(false);
    }

    public void OnClickPremiumButton()
    {
        //m_StartMenuPanel.SetActive(false);
        //m_StickersPanel.SetActive(false);
        //m_SettingsPanel.SetActive(false);
        //m_PremiumPanel.SetActive(true);
    }

    public void OnClickSettingsButton()
    {
        //m_StartMenuPanel.SetActive(false);
        //m_PremiumPanel.SetActive(false);
        //m_StickersPanel.SetActive(false);
        m_SettingsPanel.SetActive(true);
    }

    public void OnClickExitSettingsButton()
    {
        //m_StartMenuPanel.SetActive(false);
        //m_PremiumPanel.SetActive(false);
        //m_StickersPanel.SetActive(false);
        m_SettingsPanel.SetActive(false);
    }

    public void OnClickExitPremiumButton()
    {
        //m_StartMenuPanel.SetActive(false);
        //m_PremiumPanel.SetActive(false);
        //m_StickersPanel.SetActive(false);
        //m_SettingsPanel.SetActive(false);
    }

    public void OnClickHomeButton()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //m_PremiumPanel.SetActive(false);
            m_StickersPanel.SetActive(false);
            m_SettingsPanel.SetActive(false);
            m_StartMenuPanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
        
    }

    public void OnClickTest()
    {
        StartCoroutine(LoadSceneTest());
        //SceneManager.LoadScene(6);
    }

    IEnumerator LoadSceneTest()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(6);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        //Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            //Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                //Debug.Log("Press the space bar to continue");
                //Wait to you press the space key to activate the Scene
                //if (Input.GetKeyDown(KeyCode.Space))
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void OnClickClear()
    {
        m_LevelsComplete.ResetData();
    }

    /*public void OnClickChoiceSceneBtn(int sceneNumber)
    {
        SwitchToScene(sceneNumber);
        Debug.Log(sceneNumber);
        //Tween ClosingSceneTween = m_FadeImage.DOFade(1f, 0.1f);

        //ClosingSceneTween.Play().OnComplete(() => SwitchToScene(sceneNumber));
    }
    */
    private void SwitchToScene(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
    }

    static public void ReloadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    static public int GetActiveScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
