using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public void SavePage(string name, Color[] colors, bool isStickerReceived, int[] numbMat)
    {
        var dataPage = new DataPage(colors, numbMat, isStickerReceived);

        string json = JsonUtility.ToJson(dataPage);
        //Debug.Log(json);;

        File.WriteAllText(Application.persistentDataPath + $"/{name}.json", json);
    }

    public DataPage LoadPage(string name)
    {
        if (File.Exists(Application.persistentDataPath + $"/{name}.json"))
        {
            //Debug.Log(Application.persistentDataPath + $"/{name}.json");

            string jsonLoad = File.ReadAllText(Application.persistentDataPath + $"/{name}.json");

            DataPage data = JsonUtility.FromJson<DataPage>(jsonLoad);

            return data;
        }

        return null;
    }

    [System.Serializable]
    public class DataPage
    {
        public Color[] colors;
        public int[] numbMat;
        public bool isStickerReceived;

        public DataPage(Color[] colors, int[] numbMat, bool isStickerReceived)
        {
            this.colors = new Color[colors.Length];
            this.numbMat = new int[numbMat.Length];
            this.isStickerReceived = isStickerReceived;

            for (int i = 0; i < colors.Length; i++)
            {
                this.colors[i] = colors[i];

                this.numbMat[i] = numbMat[i];
            } 
        }

        //public bool[] isShading;
    }
}
