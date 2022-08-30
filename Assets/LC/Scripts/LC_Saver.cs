using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class LC_Saver : MonoBehaviour
{
    public static void SaveLC(List<int> data)
    {
        LCsave buffer = new LCsave();
        buffer.completedLevels = data;

        string json = JsonUtility.ToJson(buffer);
        //Debug.Log(json);

        File.WriteAllText(Application.persistentDataPath + $"/LCdata.json", json);
    }

    public static List<int> LoadLC()
    {
        if (File.Exists(Application.persistentDataPath + $"/LCdata.json"))
        {
            string jsonLoad = File.ReadAllText(Application.persistentDataPath + $"/LCdata.json");
            LCsave data = JsonUtility.FromJson<LCsave>(jsonLoad);
            return data.completedLevels;
        }
        else
        {
            Debug.Log("[LC] No save data");
        }

        return null;
    }

    public static void DeleteSave()
    {
        LCsave buffer = new LCsave();
        string json = JsonUtility.ToJson(buffer);
        File.WriteAllText(Application.persistentDataPath + $"/LCdata.json", json);
    }

    [System.Serializable]
    public class LCsave
    {
        public List<int> completedLevels;
    }
}
