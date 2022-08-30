using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mainmenu
{
    public class MagnetObjects : MonoBehaviour
    {
        private RectTransform m_ContentRect;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            m_ContentRect = GetComponent<RectTransform>();
        }
    }
}
