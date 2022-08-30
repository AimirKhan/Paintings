using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayerTemp : MonoBehaviour
{
    [SerializeField] private string m_Game;
    [SerializeField] private int[] m_SceneIndexes;
    [SerializeField] private int m_OneSongRepeat = 2;
    [SerializeField] private float m_MusicVolume = 1f;

    private List<AudioClip> sounds = new List<AudioClip>();
    private new AudioSource audio;

    private static MusicPlayerTemp m_MusicPlayerInstance;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (m_MusicPlayerInstance == null)
        {
            m_MusicPlayerInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.activeSceneChanged += Initialization;
        audio = GetComponent<AudioSource>();

        object[] soundObjects = Resources.LoadAll("Game/Music/" + m_Game);

        for (int i = 0; i < soundObjects.Length; i++)
        {
            sounds.Add((AudioClip)soundObjects[i]);
        }
    }

    private void Start()
    {
        var currentScene = SceneManager.GetActiveScene();
        Initialization(currentScene, currentScene);
    }

    void StartMusic()
    {
        if (audio == null)
        {
            return;
        }
        int currentSceneId = SceneManager.GetActiveScene().buildIndex;
        for (int index = 0; index < m_SceneIndexes.Length; index++)
        {
            if (currentSceneId == m_SceneIndexes[index] & !audio.isPlaying)
            {
                StartCoroutine(PlayMusic());
                break;
            }
            else if (currentSceneId == m_SceneIndexes[index] & audio.isPlaying)
            {
                break;
            }
            else if ( index == m_SceneIndexes.Length - 1)
            {
                Destroy(gameObject);
                SceneManager.activeSceneChanged -= Initialization;
            }
        }

    }

    IEnumerator PlayMusic()
    {
        while (CustomSettings.MusicVolume >= 0.01)
        {
            audio.clip = sounds[Random.Range(0, sounds.Count)];
            for (int i = 0; i < m_OneSongRepeat; i++)
            {
                audio.Play();
                yield return new WaitForSeconds(audio.clip.length);
            }
        }
    }

    private void Initialization(Scene current, Scene next)
    {
        // Делаем вещи при смене сцены
        Debug.Log("scene changed");
        StartMusic();
    }
}
