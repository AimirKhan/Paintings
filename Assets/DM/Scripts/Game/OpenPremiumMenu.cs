using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Paintings.DM.Premium
{
    public class OpenPremiumMenu : MonoBehaviour
    {
        [SerializeField] private Button m_PremiumButton;

        private const string premium_opened = "premium_opened";
        private bool m_WillOpenedPremiumMenu = false;

        void Awake()
        {
            m_WillOpenedPremiumMenu = PlayerPrefsHelper.GetBool(premium_opened, m_WillOpenedPremiumMenu);
        }

        void OnEnable()
        {
            if (m_WillOpenedPremiumMenu)
            {
                Debug.Log("KEK");
                m_PremiumButton.onClick.Invoke();
                m_WillOpenedPremiumMenu = false;
                PlayerPrefsHelper.SetBool(premium_opened, m_WillOpenedPremiumMenu);
            }
        }
    }
}