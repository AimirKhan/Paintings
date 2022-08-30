using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonAppSettings : MonoBehaviour
{
    [SerializeField] private int maxFPS = 60;
    private void Awake()
    {
        Application.targetFrameRate = maxFPS;
    }
}
