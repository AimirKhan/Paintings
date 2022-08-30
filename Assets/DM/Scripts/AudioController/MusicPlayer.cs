using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private ArrayMusic[] m_LevelIndexes;
    [SerializeField] private int m_OneSongRepeat = 2;
    [SerializeField] private float m_MusicVolume = 0.3f;

    private new AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
        StartMusic();
    }

    private void StartMusic()
    {
        int currentSceneId = SceneManager.GetActiveScene().buildIndex;
        for (int index = 0; index < m_LevelIndexes.Length; index++)
        {
            if (currentSceneId == index)
            {
                StartCoroutine(PlayMusic(currentSceneId));
            }
        }
    }

    IEnumerator PlayMusic(int currScene)
    {
        Debug.Log("Current scene = " + currScene);
        var musicsArr = m_LevelIndexes[currScene].musics;
        /*
        while(CustomSettings.IsSoundOn)
            {
                for (int i = 0; i < m_OneSongRepeat; i++)
                {
                    audio.Play();
                    yield return new WaitForSeconds(audio.clip.length);
                }
                audio.clip = musicsArr[Random.Range(0, musicsArr.Length)];
        }
        */
        while(CustomSettings.MusicVolume != 0)
        {
            audio.clip = musicsArr[Random.Range(0, musicsArr.Length)];
            for (int i = 0; i < m_OneSongRepeat; i++)
                {
                    audio.Play();
                    yield return new WaitForSeconds(audio.clip.length);
                }
        }
    }
}

[System.Serializable]
public class ArrayMusic
{
    public AudioClip[] musics;
}