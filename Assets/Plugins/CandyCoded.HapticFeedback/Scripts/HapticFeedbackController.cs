// Copyright (c) Scott Doxey. All Rights Reserved. Licensed under the MIT License. See LICENSE in the project root for license information.

using UnityEngine;

namespace CandyCoded.HapticFeedback
{

    public class HapticFeedbackController : MonoBehaviour
    {
        public bool isHapticON = true;

        public void LightFeedback()
        {
            if (isHapticON)
            {
                HapticFeedback.LightFeedback();
            }
        }

        public void MediumFeedback()
        {
            if (isHapticON)
            {
                HapticFeedback.MediumFeedback();
            }
        }

        public void HeavyFeedback()
        {
            if (isHapticON)
            {
                HapticFeedback.HeavyFeedback();

            }
        }
    }
}
