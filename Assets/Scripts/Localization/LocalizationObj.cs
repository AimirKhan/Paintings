using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationObj : MonoBehaviour
{
    [SerializeField] private int index;
    private Text m_text;

    private void Start()
    {
        m_text = GetComponent<Text>();
        m_text.text = LocalizationManager.GetPhrase(index);
    }
}
