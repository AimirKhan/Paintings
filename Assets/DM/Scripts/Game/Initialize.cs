using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Initialize : MonoBehaviour
{
    [Inject] private LevelController m_LevelController;

    void Start()
    {
        m_LevelController.Initialize();
    }
}
