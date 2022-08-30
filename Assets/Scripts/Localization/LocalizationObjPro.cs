using UnityEngine;
using TMPro;

public class LocalizationObjPro : MonoBehaviour
{
    [SerializeField] private int index;
    private TextMeshProUGUI m_text;

    private void Start()
    {
        m_text = GetComponent<TextMeshProUGUI>();
        m_text.SetText(LocalizationManager.GetPhrase(index));
    }
}
