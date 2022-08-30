using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
public class SettingsPanelButton : MonoBehaviour
{
    [SerializeField] private string m_SupportEmail = "ceo@ooopdn.com";
    [SerializeField] private string m_TermOfUse = "https://ooopdn.wixsite.com/ooopdn/terms-of-use";
    [SerializeField] private string m_PrivacyPolicy = "https://ooopdn.wixsite.com/ooopdn";
    public void onClickRateUs()
    {
    #if UNITY_IOS

        Device.RequestStoreReview();

    #endif
    }

    public void onClickSupport()
    {
        Application.OpenURL("mailto:" + m_SupportEmail);
    }

    public void onClickPrivacyPolicy()
    {
        Application.OpenURL(m_PrivacyPolicy);
    }

    public void onClickTermOfUse()
    {
        Application.OpenURL(m_TermOfUse);
    }
}
