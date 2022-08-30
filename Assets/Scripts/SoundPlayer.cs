using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer instance;

    List<AudioClip> sounds = new List<AudioClip>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // replace to Zenject injection

            object[] soundObjects = Resources.LoadAll("Game/Sounds");

            for (int i = 0; i < soundObjects.Length; i++)
            {
                sounds.Add( (AudioClip)soundObjects[i] );
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Play(string name, float volume)
    {
        GameObject newSoundObject = new GameObject(name);
        AudioSource newSoundSource = newSoundObject.AddComponent<AudioSource>();
        AudioClip clip;

        clip = sounds.First(testClip => testClip.name == name);

        newSoundSource.clip = clip;
        newSoundSource.volume = volume * CustomSettings.SoundVolume;
        newSoundSource.Play();

        Destroy(newSoundObject, clip.length);
    }
    /*
    public void Play(string name, float volume)
    {
        if (CustomSettings.IsSoundOn == true)
        {
            GameObject newSoundObject = new GameObject(name);
            AudioSource newSoundSource = newSoundObject.AddComponent<AudioSource>();
            AudioClip clip;

            clip = sounds.First (testClip => testClip.name == name);

            newSoundSource.clip = clip;
            //newSoundSource.volume = volume * CustomSettings.Volume;
            newSoundSource.volume = volume;
            newSoundSource.Play();

            Destroy(newSoundObject, clip.length);
        }
    }
    */
}
