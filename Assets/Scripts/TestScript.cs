using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class TestScript : MonoBehaviour
{
    [Inject] private SaveLevelsComplete m_LevelsComplete;

    [SerializeField] private BallonSpawner m_BalloonSpawner; 
    [SerializeField] private CompleteStickers m_StickComplete;
    private int m_CountLevels;

    private void Awake()
    {
        m_LevelsComplete.LoadCountLevels();
        m_CountLevels = m_LevelsComplete.m_LevelsComplete;
    }

    public void OnClick()
    {
        //m_CountLevels++;
        m_LevelsComplete.m_LevelsComplete++;
        m_LevelsComplete.SaveCountLevels();
        if (m_LevelsComplete.m_LevelsComplete % 3 == 0)
        {
            m_StickComplete.SetCurrentSticker(m_LevelsComplete.m_LevelsComplete / 3);
            //m_StickComplete.OnClickTapStickerButton();
        }
        Debug.Log(m_LevelsComplete.m_LevelsComplete);
        //m_LevelsComplete.SaveCountLevels();
        
    }

    public void OnClickToStartMenu()
    {
        StartCoroutine(LoadSceneTest());
    }

    IEnumerator LoadSceneTest()
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(0);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        Debug.Log("Pro :" + asyncOperation.progress);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            Debug.Log("Loading progress: " + (asyncOperation.progress * 100) + "%");

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                //Change the Text to show the Scene is ready
                Debug.Log("Press the space bar to continue");
                //Wait to you press the space key to activate the Scene
                //if (Input.GetKeyDown(KeyCode.Space))
                //Activate the Scene
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void OnClickBalloonsStart()
    {
        m_BalloonSpawner.StartBalloons();

    }

    public void OnClickBalloonsStop()
    {
        m_BalloonSpawner.StopBalloons();

    }
}
