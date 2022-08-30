using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFinderHelper : MonoBehaviour
{
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0.5f);
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Stop();
    }

    public void Stop()
    {
        animator.Play("Unactive");
        animator.StopPlayback();
    }

    public void SetPosition(Vector3 newPos, bool updateAnim = true)
    {
        transform.position = newPos + offset;

        if (updateAnim)
        {
            animator.Play("Delay", -1, 0f);
        }
    }
}
