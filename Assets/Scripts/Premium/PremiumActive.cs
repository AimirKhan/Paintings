using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PremiumActive : MonoBehaviour
{
    [SerializeField] private bool ActiveOnPremium = true;
    void Start()
    {
        if (Purchaser.isPremium())
        {
            gameObject.SetActive(ActiveOnPremium);
        }
        else
        {
            gameObject.SetActive(!ActiveOnPremium);
        }
    }
}
