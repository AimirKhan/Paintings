using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CandyCoded.HapticFeedback;

public class Mascot_animation : MonoBehaviour
{
    [SerializeField] private float m_BlinktimeMin;
    [SerializeField] private float m_BlinktimeMax;

    private Animator m_Anim;
    private IEnumerator m_Blinks, m_Throws;
    private bool m_IsAnimEnd = false;

   void Start()
    {
        m_Anim = GetComponent<Animator>();
        
        m_Anim.Play("Base Layer.dm_mascot_idle");

        m_Blinks = Blinking();
        StartCoroutine(m_Blinks);
    }

    private void OnMouseDown()
    {
        if (m_IsAnimEnd == false) 
        {
            StartCoroutine(ThrowATile());
            HapticFeedback.MediumFeedback();
        }
    }

    private IEnumerator ThrowATile() 
    {
        m_IsAnimEnd = true;
        if (m_Blinks != null)
        {
            StopCoroutine(m_Blinks);
        }
        m_Anim.Play("throw.dm_mascot_throw");
        yield return new WaitForSeconds(2f);
        StartCoroutine(m_Blinks);
        m_IsAnimEnd = false;
        yield break;
    }

    private IEnumerator Blinking()
    {
        float randomTime = Random.Range(m_BlinktimeMin, m_BlinktimeMax);
        while (true)
        {
            yield return new WaitForSeconds(randomTime);
            m_Anim.Play("blinks.dm_mascot_blink");
        }
    }
}
