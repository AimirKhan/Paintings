using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using CandyCoded.HapticFeedback;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_MainMusic;
    [SerializeField] private AudioClip m_ClickStartButtonSound;
    [SerializeField] private AudioClip m_ClickButtonSound;
    [SerializeField] private AudioClip m_ClickColoringImage;
    [SerializeField] private AudioMixerGroup m_MixerMaster;


    private bool m_isMusicPlays = true;

    private void Start()
    {
        m_MainMusic.Play();
    }

    public void OnClickButtonStart()
    {
        SoundPlayer.instance.Play(m_ClickStartButtonSound.name, 1);
        //if (!m_ClickStartButtonSound.isPlaying)
        //{
        //    m_ClickStartButtonSound.Play();
        //}
    }

    public void OnClickButton()
    {
        SoundPlayer.instance.Play(m_ClickButtonSound.name, 1);

        HapticFeedback.MediumFeedback();

        //m_ClickButtonSound.PlayOneShot(m_ClickButtonSound.clip);   
    }

    public void OnColoringImage()
    {
        SoundPlayer.instance.Play(m_ClickColoringImage.name, 0.1f);

        HapticFeedback.MediumFeedback();
        //m_ClickColoringImage.PlayOneShot(m_ClickColoringImage.clip);
    }

    public void OnClickSoundButton()
    {
        if (m_isMusicPlays)
        {
            m_MixerMaster.audioMixer.SetFloat("MasterVolume", -80);

            m_isMusicPlays = false;
        }
        else
        {
            m_MixerMaster.audioMixer.SetFloat("MasterVolume", 0);

            m_isMusicPlays = true;
        }
    }
}
