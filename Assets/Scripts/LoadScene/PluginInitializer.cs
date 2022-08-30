using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PluginInitializer: MonoBehaviour
{
    [SerializeField] private NotificationsiOS m_NotificationsiOS;
    [SerializeField] private Purchaser m_Purchaser;
    [SerializeField] private GameObject m_AppsFlyerPrefab;

    public bool isAppsFlyer = true;
    public bool isAmplitude = true;
    public bool isPushNotification = true;
    public bool isIAP = true;

    public IEnumerator LoadPlugins()
    {
        if (isAppsFlyer)
        {
            var m_ApFl = Instantiate(m_AppsFlyerPrefab);
            DontDestroyOnLoad(m_ApFl);

            yield return null;
            yield return null;
        }

        if (isAmplitude)
        {
            InitAmplitude();

            yield return null;
            yield return null;
            //Debug.Log("Ampl load");
        }

        if (isPushNotification)
        {
#if UNITY_IOS
            InitPUSH();
            //Debug.Log("iOS APNs loaded");
#endif

            yield return null;
            yield return null;
        }

        if (isIAP)
        {
            InitIAP();

            yield return null;
            yield return null;
        }

        //Debug.Log("init finish");
        
    }

    private void InitAmplitude()
    {
        AmplitudeAdapter.GetInstance();

        AmplitudeAdapter.Instance.Initialization();

        if (PlayerPrefs.GetInt("FirstTimeOpening", 1) == 1)
        {
            AnalyticsHelper.GetInstance().SetOnceUserProperty();

            AnalyticsHelper.Instance.SetOnceUserProperty();

            AnalyticsHelper.Instance.SendFirstLaunch();

            PlayerPrefs.SetInt("FirstTimeOpening", 0);
        }
        else
        {
            AnalyticsHelper.GetInstance();
        }
    }

    private void InitPUSH()
    {
#if UNITY_IOS
        m_NotificationsiOS.RequestPUSHes();
#endif
    }

    private void InitIAP()
    {
        m_Purchaser.Initialize();
    }
}