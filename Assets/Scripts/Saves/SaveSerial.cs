using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;

[Serializable]
class SaveData
{
    public int currentLevel;
    public string passedLevels;
    public string isPremium;
    public int savedInt;
    public float savedFloat;
    public bool savedBool;
}


public class SaveSerial : MonoBehaviour
{
    public int currentLevelToSave;
    public int[] passedLevelsToSave;
    public int[] premiumLevelToSave;
    public int intToSave;
    public float floatToSave;
    public bool boolToSave;

    [ContextMenu("Test/SaveGame")]
    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        using FileStream file = File.Create(Application.persistentDataPath + "/DMSaveData.dat");
        {
            SaveData data = new SaveData();
            data.currentLevel = currentLevelToSave;

            string tempPassedLevels = string.Join(";", passedLevelsToSave);
            data.passedLevels = tempPassedLevels;
            string tempPremiumLevels = string.Join(";", premiumLevelToSave);
            data.isPremium = tempPremiumLevels;
            data.savedInt = intToSave;
            data.savedFloat = floatToSave;
            data.savedBool = boolToSave;
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Save complete!");
        }
    }

    [ContextMenu("Test/LoadGame")]
    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/DMSaveData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath + "/DMSaveData.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            currentLevelToSave = data.currentLevel;
            
            int[] tempPassedLevels = data.passedLevels.Split(';').Select(int.Parse).ToArray();
            passedLevelsToSave = tempPassedLevels;
            int[] tempPremiumLevels = data.isPremium.Split(';').Select(int.Parse).ToArray();
            premiumLevelToSave = tempPremiumLevels;
            intToSave = data.savedInt;
            floatToSave = data.savedFloat;
            boolToSave = data.savedBool;
            Debug.Log("Load complete!");
        }
        else
            Debug.Log("There is no save data!");
    }

    [ContextMenu("Test/DeleteSave")]
    public void DeleteSaves()
    {
        if (File.Exists(Application.persistentDataPath + "/DMSaveData.dat"))
        {
            File.Delete(Application.persistentDataPath + "/DMSaveData.dat");
            Debug.Log("Save file delete successfuly");
        }
        else
        {
            Debug.Log("There is no save data!");
        }
    }
}
