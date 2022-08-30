using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Common;

public class PlayerPrefsHelper
{
    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(B64X.Encrypt(key, SystemInfo.deviceUniqueIdentifier), value);
        PlayerPrefs.Save();
    }
    public static void SetString(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
    }
    public static void SetFloat(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
    }
    public static void SetBool(string key, bool value)
    {
        SetInt(key, value ? 1 : 0);
    }
    public static int GetInt(string key, int defaultValue)
    {
        return PlayerPrefs.GetInt(B64X.Encrypt(key, SystemInfo.deviceUniqueIdentifier), defaultValue);
    }
    public static string GetString(string key, string defaultValue)
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    public static float GetFloat(string key, float defaultValue)
    {
        return PlayerPrefs.GetFloat(key, defaultValue);
    }
    public static bool GetBool(string key, bool defaultValue)
    {
        return (GetInt(key, defaultValue ? 1 : 0)) == 1 ? true : false;
    }
}
