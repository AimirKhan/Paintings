using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.Linq;

[Serializable]
class SaveLevelsCount
{
    public int completeLevel;
}

[Serializable]
class SaveTapObject
{
    public string isTapObjects;
}


public class SaveTap : MonoBehaviour
{
    public int[] m_IsTapObjects;

    public void SaveTapStickers()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using FileStream file = File.Create(Application.persistentDataPath + "/SaveTapStickers.dat");
        {
            SaveTapObject data = new SaveTapObject();
            string tempTapObjects = string.Join(";", m_IsTapObjects);
            data.isTapObjects = tempTapObjects;
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Game data saved!");
        }
    }

    public void LoadTapStickers()
    {
        if (File.Exists(Application.persistentDataPath
          + "/SaveTapStickers.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/SaveTapStickers.dat", FileMode.Open);
            SaveTapObject data = (SaveTapObject)bf.Deserialize(file);
            file.Close();

            int[] tempPassedLevels = data.isTapObjects.Split(';').Select(int.Parse).ToArray();
            m_IsTapObjects = tempPassedLevels;

            Debug.Log("Game data loaded!");
        }
        else
            Debug.Log("There is no save data!");
    }
}

public class SaveLevelsComplete : MonoBehaviour
{
    public int m_LevelsComplete;


    public int[] m_IsTapObjects;

    public void SaveTapStickers()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using FileStream file = File.Create(Application.persistentDataPath + "/SaveTapStickers.dat");
        {
            SaveTapObject data = new SaveTapObject();
            string tempTapObjects = string.Join(";", m_IsTapObjects);
            data.isTapObjects = tempTapObjects;
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Game data saved!");
        }
    }

    public void LoadTapStickers()
    {
        if (File.Exists(Application.persistentDataPath
          + "/SaveTapStickers.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/SaveTapStickers.dat", FileMode.Open);
            SaveTapObject data = (SaveTapObject)bf.Deserialize(file);
            file.Close();

            int[] tempPassedLevels = data.isTapObjects.Split(';').Select(int.Parse).ToArray();
            m_IsTapObjects = tempPassedLevels;

            Debug.Log("Game data loaded!");
        }
        else
            Debug.Log("There is no save data!");
    }

    public void SaveCountLevels()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using FileStream file = File.Create(Application.persistentDataPath + "/SaveDataLevelsComplete.dat");
        {
            SaveLevelsCount data = new SaveLevelsCount();
            data.completeLevel = m_LevelsComplete;
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Game data saved!");
        }
    }


    public void LoadCountLevels()
    {
        if (File.Exists(Application.persistentDataPath
          + "/SaveDataLevelsComplete.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath
              + "/SaveDataLevelsComplete.dat", FileMode.Open);
            SaveLevelsCount data = (SaveLevelsCount)bf.Deserialize(file);
            file.Close();
            m_LevelsComplete = data.completeLevel;

            Debug.Log("Game data loaded!");
        }
        else
            Debug.Log("There is no save data!");
    }

    public void ResetData()
    {
        if (File.Exists(Application.persistentDataPath
          + "/SaveDataLevelsComplete.dat"))
        {
            File.Delete(Application.persistentDataPath
              + "/SaveDataLevelsComplete.dat");
            m_LevelsComplete = 0;
            Debug.Log("SaveDataLevelsComplete reset complete!");
        }
        else
            Debug.Log("No save data to delete.");

        if (File.Exists(Application.persistentDataPath
          + "/SaveTapStickers.dat"))
        {
            File.Delete(Application.persistentDataPath
              + "/SaveTapStickers.dat");
            m_IsTapObjects = null;
            Debug.Log("SaveTapStickers reset complete!");
        }
        else
            Debug.Log("No save data to delete.");
    }
}
