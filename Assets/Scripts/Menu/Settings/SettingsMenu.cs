using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer m_MainMixer;
    [SerializeField] private Slider m_SoundVolumeSlider;
    [SerializeField] private Slider m_MusicVolumeSlider;
    //[SerializeField] private Text m_ApplicationVersionText;
    [SerializeField] private string m_SupportURL; // Support site
    [SerializeField] private GameObject m_SettingsWindow;

    void Start()
    {
        CloseSettingsPanel();
        m_SoundVolumeSlider.value = CustomSettings.SoundVolume;
        m_MusicVolumeSlider.value = CustomSettings.MusicVolume;
        //m_SoundEnableToggle.isOn = CustomSettings.IsSoundOn;
        //m_ApplicationVersionText.text = Application.version;
    }

    public void ChangeSoundVolumeSlider(float soundSliderValue)
    {
        CustomSettings.SoundVolume = m_SoundVolumeSlider.value;
        m_MainMixer.SetFloat("SoundTone", Mathf.Log10(soundSliderValue) * 20);
    }

    public void ChangeMusicVolumeSlider(float musicSliderValue)
    {
        CustomSettings.MusicVolume = m_MusicVolumeSlider.value;
        m_MainMixer.SetFloat("MusicTone", Mathf.Log10(musicSliderValue) * 20);
    }

    public void OpenSupportURL()
    {
        Application.OpenURL(m_SupportURL);
    }

    public void CloseSettingsPanel()
    {
        m_SettingsWindow.SetActive(false);
    }

    public void RestorePurchase()
    {
        var iap = FindObjectOfType<Purchaser>();
        if (iap != null)
        {
            iap.Restore();
        }
        else
        {
            Debug.LogError("Purchaser is null. Can't restore purchase.");
        }
    }
}