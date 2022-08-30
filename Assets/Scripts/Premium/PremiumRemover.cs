using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PremiumRemover : MonoBehaviour
{
    void Awake()
    {
        if (Purchaser.isPremium())
        {
            Destroy(gameObject);
        }
    }
}
