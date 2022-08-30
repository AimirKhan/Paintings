using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectShifter : MonoBehaviour
{
    [SerializeField] private Transform gameObjectTransform;

    private void Start()
    {
        gameObjectTransform = transform;
    }
}
