using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomSettings
{
    private const string sound_volume_key = "sound_volume";
    private const string music_volume_key = "music_volume";
    public static float SoundVolume
    {
        //get => PlayerPrefsHelper.GetBool(soundVolumeKey, true);
        //set => PlayerPrefsHelper.SetBool(soundVolumeKey, value);
        get => PlayerPrefsHelper.GetFloat(sound_volume_key, 1);
        set => PlayerPrefsHelper.SetFloat(sound_volume_key, value);
    }
    public static float MusicVolume
    {
        get => PlayerPrefsHelper.GetFloat(music_volume_key, 1);
        set => PlayerPrefsHelper.SetFloat(music_volume_key, value);
    }
}
