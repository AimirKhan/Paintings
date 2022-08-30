using System;
using System.Text;
using UnityEngine;
using TMPro;
using CandyCoded.HapticFeedback;

public class AgeInputController : MonoBehaviour
{
    [SerializeField] private int maxAge = 90;
    [SerializeField] private int minAge = 18;
    [SerializeField] private AgeTestLoader loader;
    [SerializeField] private AudioSource clickSound;
    [SerializeField] private HapticFeedbackController m_HapticFeedback;

    private TextMeshProUGUI[] input;
    private int activeIndex;
    private int inputLen = 4;
    private string emptyKey = "-";

    void Start()
    {
        input = GetComponentsInChildren<TextMeshProUGUI>();

        if (input.Length != inputLen)
        {
            Debug.LogError("input size = " + input.Length);
        }

        ClearText();
    }

    private void OnEnable()
    {
        if (input != null)
        {
            ClearText();
        }
    }

    public void Addtext(string text)
    {
        m_HapticFeedback.MediumFeedback();
        clickSound.Play();
        input[activeIndex].text = text;
        activeIndex++;
        CompleteText();
    }

    public void RemoveText()
    {
        clickSound.Play();
        if (activeIndex <= 0)
        {
            return;
        }

        activeIndex--;
        input[activeIndex].text = emptyKey;
    }
    private void ClearText()
    {
        activeIndex = 0;

        for (int i = 0; i < input.Length; i++)
        {
            input[i].text = emptyKey;
        }
    }

    private void CompleteText()
    {
        if (activeIndex == inputLen)
        {
            Unlock();
            ClearText();
        }
    }

    private void Unlock()
    {
        StringBuilder tmp = new StringBuilder();

        for (int i = 0; i < input.Length; i++)
        {
            tmp.Append(input[i].text);
        }

        int inputResult = 0;

        try
        {
            inputResult = Convert.ToInt32(tmp.ToString());
        }
        catch
        {
            Debug.LogError("Convert to Int error. Input text: " + tmp.ToString());
        }

        int finalAge = System.DateTime.Now.Year - inputResult;

        if (finalAge >= minAge && finalAge <= maxAge)
        {
            //Debug.Log("UNLOCK: age=" + finalAge.ToString());
            loader.OnCorrectInput();
        }
        else
        {
            //Debug.Log("LOCK: age=" + finalAge.ToString());
            loader.OnUncorrectInput();
        }
    }
}
