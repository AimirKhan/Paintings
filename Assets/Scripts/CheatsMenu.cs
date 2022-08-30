using System;
using System.Text;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CheatsMenu : MonoBehaviour
{

    [SerializeField] private GameObject _panelButtons;
    [SerializeField] private GameObject m_ButtonsForOnOff;
    [SerializeField] private GameObject _graphyPrefab;
    [SerializeField] private GameObject _lunarConsolePrefab;
    [SerializeField] private Button[] _buttons;

    private static CheatsMenu _instance;
    private GameObject _graphyInstance;
    private GameObject _lunarConsoleInstance;
    private bool _opened;
    private Vector2 _position;
    private DateTime m_TimeStart = DateTime.Now;
    private TimeSpan m_Timer;

    private int countClick = 0;

    public event Action OnResetProgress;
    public event Action OnPremiumClick;
    
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            if (_graphyInstance == null && _lunarConsoleInstance == null)
            {
                _graphyInstance=Instantiate(_graphyPrefab);
                _graphyInstance.SetActive(false);
                _lunarConsoleInstance=Instantiate(_lunarConsolePrefab);

                if(_lunarConsoleInstance != null)
                {
                    _lunarConsoleInstance.SetActive(!_lunarConsoleInstance.activeSelf);
                }

            }
            DontDestroyOnLoad(_instance);
        }
        else
        {
            Destroy(gameObject);
        }
        AddFunctionsToButtons();

        _panelButtons.SetActive(false);
        m_ButtonsForOnOff.SetActive(true);
    }

    private void OnDisable()
    {
        RemoveFunctionsFromButtons();
    }

    private void AddFunctionsToButtons()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            int number = i;
            _buttons[i].onClick.AddListener(()=>FunctionButton(number));
        }
    }

    private void RemoveFunctionsFromButtons()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].onClick.RemoveAllListeners();
        }
    }

    private void FunctionButton(int number)
    {
        switch (number)
        {
            case 0:
                ClickCheatMenu();
                return;
            case 1:
                OnResetProgress?.Invoke();
                ClearProgress();
                return;
            case 2:
                ClickGraphyButton();
                return;
            case 3:
                ClickLunarConsoleButton();
                return;
            case 4:
                ClickPremButton();
                return;
            case 5://sticker
                return;
        }
    }

    public void ClearProgress()
    {
        OnResetProgress?.Invoke();

        DirectoryInfo directory = new DirectoryInfo(Application.persistentDataPath);
        StringBuilder output = new StringBuilder();

        output.AppendLine("Deleted files: \n");

        foreach (FileInfo file in directory.GetFiles())
        {
            output.AppendLine("File: " + file.Name);
            file.Delete();
        }

        /*
        foreach (DirectoryInfo dir in directory.GetDirectories())
        {
            output.AppendLine("Dir: " + dir.Name);
        }
        */

        AnalyticsHelper.Instance.SendResetProgress();

        Debug.Log(output.ToString());
        UpdateScene();
    }

    public void ClickCheatMenu()
    {
        if (_panelButtons.transform is RectTransform rt)
        {
            if (!_opened)
            {
                rt.DOAnchorPosY(-242.26f, 0.5f);
                rt.DOSizeDelta(new Vector2(rt.sizeDelta.x, 484.52f), 0.5f);
                _opened = true;
            }
            else
            {
                rt.DOAnchorPosY(-73.07f, 0.5f);
                rt.DOSizeDelta(new Vector2(rt.sizeDelta.x, 146.14f), 0.5f);
                _opened = false;
            }
        }
    }

    public void ClickGraphyButton()
    {
        if (_graphyInstance != null)
        {
            _graphyInstance.SetActive(!_graphyInstance.activeSelf);
            _buttons[2].GetComponentInChildren<Text>().text = "Graphy is " + _graphyInstance.activeSelf;
            UpdateScene();
        }
        else
        {
            Debug.LogWarning("Graphy is null");
        }
    }

    public void ClickLunarConsoleButton()
    {
        if (_lunarConsoleInstance != null)
        {
            _lunarConsoleInstance.SetActive(!_lunarConsoleInstance.activeSelf);
            _buttons[3].GetComponentInChildren<Text>().text = "Lunar is " + _lunarConsoleInstance.activeSelf;
            UpdateScene();
        }
    }

    private void ClickPremButton()
    {
        Purchaser.DebugActivePremium(!Purchaser.isPremium());
        _buttons[4].GetComponentInChildren<Text>().text = "Premium is " + Purchaser.isPremium();
        OnPremiumClick?.Invoke();
        UpdateScene();
    }

    public void OnClickHiddenButton()
    {
        m_Timer = DateTime.Now.Subtract(m_TimeStart);

        if (m_Timer.Seconds < 1)
        {
            //Debug.Log("Second < 5");
            countClick++;
        }
        else
        {
            //Debug.Log("Second >= 5");

            countClick = 0;
            m_TimeStart = DateTime.Now;
            return;
        }

        if (countClick == 15)
        {
            //Debug.Log("Count == 15");

            _panelButtons.SetActive(!_panelButtons.activeSelf);

            countClick = 0;
        }

        m_TimeStart = DateTime.Now;
    }

    private void UpdateScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
