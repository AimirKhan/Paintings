using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotatePage : MonoBehaviour
{
    public Animator rotatePage;
    public ParticleSystem starsEmmit;

    private void Start()
    {
        rotatePage = GetComponent<Animator>();
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 80), "Play"))
        {
            rotatePage.SetTrigger("TrChangePage");
            starsEmmit.Play();
        }
    }

}
