using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperCounter : MonoBehaviour
{
    private int m_count = 0;

    public int NumberOfCalls
    {
        get => m_count;
        private set => m_count = value;
    }
    
    public void SetZeroCount()
    {
        m_count = 0;
    }

    public void AddCall()
    {
        m_count++;
    }
}
