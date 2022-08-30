using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateNewLevel : MonoBehaviour
{
    public LevelData newLevel;
    public GameObject parentArrayObject;

    [ContextMenu("WriteData")]
    public void WriteData()
    {
        for (int i = 0; i < parentArrayObject.transform.childCount; i++)
        {

        }
    }
}
