using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class Bootstrapping : MonoBehaviour
{
    [SerializeField] private PluginInitializer m_PluginInitializer;
    [SerializeField] private RectTransform mosaicRT_0;
    [SerializeField] private RectTransform mosaicRT_1;
    [SerializeField] private RectTransform mosaicRT_2;

    [Header("Animation settings")]
    [SerializeField] private float m_JumpPower = 50f;
    [SerializeField] private float m_Duration = 1f;
    [SerializeField] private float m_DelaySecondMosaic = 0.25f;
    [SerializeField] private float m_DelayThirdMosaic = 0.5f;
    [SerializeField] private Ease m_EaseMosaic;
    private DateTime m_StartTime;

    private int numbLoadScene;

    void Start()
    {
        m_StartTime = DateTime.Now;
        DontDestroyOnLoad(gameObject);

        Tween jump_0 = mosaicRT_0.DOJump(mosaicRT_0.position, m_JumpPower, 1, m_Duration)
            .SetEase(m_EaseMosaic);
        Tween jump_1 = mosaicRT_1.DOJump(mosaicRT_1.position, m_JumpPower, 1, m_Duration)
            .SetEase(m_EaseMosaic).SetDelay(m_DelaySecondMosaic);
        Tween jump_2 = mosaicRT_2.DOJump(mosaicRT_2.position, m_JumpPower, 1, m_Duration)
            .SetEase(m_EaseMosaic).SetDelay(m_DelayThirdMosaic);

        numbLoadScene = SceneManager.GetActiveScene().buildIndex + 1;

        Sequence queue = DOTween.Sequence();
        queue.Append(jump_0);
        queue.Join(jump_1);
        queue.Join(jump_2);
        queue.SetLoops(-1, LoopType.Restart);

        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        yield return StartCoroutine(m_PluginInitializer.LoadPlugins());

        Debug.Log("Load scene");
        AsyncOperation operation = SceneManager.LoadSceneAsync(numbLoadScene);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            if(operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;

                TimeSpan timer = DateTime.Now.Subtract(m_StartTime);
                int time = (timer.Hours * 60 * 60) + (timer.Minutes * 60) + timer.Seconds;

                AnalyticsHelper.Instance.SendLoadingComplete(time);

                break;
            }

            
        }
    }
}
