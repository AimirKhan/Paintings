using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BallonSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_BullonPrefab;
    [SerializeField] private Sprite[] m_Sprites;
    [SerializeField] private Material[] m_Materials;
    //private Sprite[] m_Sprites;
    private RectTransform m_RectTransform;
    private Vector2 m_StartPosition;
    private float m_Width;
    private int m_BalloonColor;

    private int m_CountBalloons;

    private StringBuilder debugOutput;

    private void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Width = m_RectTransform.rect.width;
        m_CountBalloons = 0;
    }

    private void InstanceBalloons()
    {
        if (m_CountBalloons > 15)
        {
            m_CountBalloons = 0;
            Debug.Log(debugOutput.ToString());
            StopBalloons();
        }
        else
        {
            // Debug.Log(m_RectTransform.localPosition);
            GameObject newBall = Instantiate(m_BullonPrefab, m_RectTransform.localPosition, m_RectTransform.rotation);
            newBall.transform.SetParent(transform);
            newBall.GetComponent<RectTransform>().localPosition = m_RectTransform.localPosition;
            Vector3 newVec = new Vector3(m_RectTransform.localPosition.x + Random.Range(-1000.0f, 1000.0f), m_RectTransform.localPosition.y, m_RectTransform.localPosition.z);
            newBall.GetComponent<RectTransform>().localPosition = newVec;
            m_BalloonColor = Random.Range(0, m_Sprites.Length);
            newBall.GetComponent<Image>().sprite = m_Sprites[m_BalloonColor];
            newBall.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystemRenderer>().material = m_Materials[m_BalloonColor];
            newBall.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            m_CountBalloons++;
            // Debug.Log(m_CountBalloons);

            debugOutput.AppendLine($"[{DateTime.Now}] Position: {m_RectTransform.localPosition} > Count: {m_CountBalloons}");
        }
        
    }

    public void OnEnable()
    {
        InvokeRepeating("InstanceBalloons", 0f, 0.4f);
        debugOutput = new StringBuilder();
    }
    public void StartBalloons()
    {
        InvokeRepeating("InstanceBalloons", 0f, 0.4f);
    }

    public void OnDisable()
    {
        CancelInvoke("InstanceBalloons");
        m_CountBalloons = 0;
        debugOutput.Clear();
    }

    public void StopBalloons()
    {
        CancelInvoke("InstanceBalloons");
        m_CountBalloons = 0;
    }

}
