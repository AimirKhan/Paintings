using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public UnityEvent m_EventSelectLevel;

    private int nubmUpdate;

    [SerializeField] private GameManager m_GameManage;

    [SerializeField] private Animator m_AnimatorExitButton;

    [SerializeField] private GameObject m_SelectPanel;
    [SerializeField] private GameObject m_GameWindow;
    [SerializeField] private GameObject m_CanvasUI;


    private void Awake()
    {
        m_EventSelectLevel = new UnityEvent();
    }

    public void PlayAnimationExitButton()
    {
        m_AnimatorExitButton.SetBool("EndGame", true);
    }
    public void StopAnimationExitButton()
    {
        m_AnimatorExitButton.SetBool("EndGame", false);
    }


    public void OnClickCreatePage()
    {
        m_GameWindow.SetActive(true);
        m_CanvasUI.SetActive(false);
        m_SelectPanel.SetActive(false);
        m_EventSelectLevel.Invoke();
    }

    public void OnFinishGame()
    {
        m_AnimatorExitButton.SetBool("EndGame", false);

        m_GameWindow.SetActive(false);
        m_CanvasUI.SetActive(true);
        m_SelectPanel.SetActive(true);

        m_GameManage.CreateButtonImage.UpdateImagePage(nubmUpdate);

        m_EventSelectLevel.Invoke();
    }

    public void OnClickResetLevel()
    {
        m_GameManage.CreateColoringPage.CancelFill();
    }

    public void ActiveWhitePanel(GameObject[] panelOn, GameObject[] panelOff)
    {
        foreach(GameObject panel in panelOn)
        {
            panel.SetActive(true);

        }

        foreach (GameObject panel in panelOff)
        {
            panel.SetActive(false);

        }
    }

    public int NumbUpdate
    {
        get
        {
            return nubmUpdate;
        }

        set
        {
            nubmUpdate = Mathf.Abs(value);
        }
    }

}