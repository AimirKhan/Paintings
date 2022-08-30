using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Zenject;

public class LevelController : MonoBehaviour
{
    [Inject] SaveSerial m_SaveSerial;
    [SerializeField] private StickersCounter m_StickersCounter;
    [SerializeField] private CameraController m_CameraController;
    [SerializeField] private Figures figures;
    [SerializeField] private GameFieldCreation m_GameFieldCreation;
    [SerializeField] private NextLevelController m_NextLevelController;
    [SerializeField] private MainMenuControl m_MainMenuControl;
    [SerializeField] private LevelData[] levels;
    [Header("Variables")]
    [SerializeField] private int currentLevel;
    [SerializeField] private string playButtonSound;
    [SerializeField] private string nextButtonSound;
    [SerializeField] private string exitButtonSound;

    private bool gameComplete;
    private List<LevelData> m_AllowedLevels = new List<LevelData>();
    private DateTime m_StartTime;
    private int m_HelperCallCount;

    private void Awake()
    {
        m_SaveSerial.LoadGame();
        AllowedLevels();
        CurrentLevel = m_SaveSerial.currentLevelToSave;
        if (currentLevel > (m_AllowedLevels.Count - 1))
        {
            CurrentLevel = 0;
        }
    }

    public void Initialize()
    {
        CheckLevelCompletion(false, false);
        PlayGameButton();
        m_StartTime = DateTime.Now;
    }

    public void PlayGameButton()
    {
        // Start game
        SoundPlayer.instance.Play(playButtonSound, 1);
        //gameFieldSpawn.PlayTransformAnim();
        figures.EnableUsedFigures(true);
        if (figures.CommonScore == figures.CommonScoreGoal)
        {
            LevelComplete();
        }
    }

    public void LevelComplete()
    {
        TimeSpan timer = DateTime.Now.Subtract(m_StartTime);
        int time = (timer.Hours * 60 * 60) + (timer.Minutes * 60) + timer.Seconds;
        // Level complete
        m_MainMenuControl.OnCompleteScreenClick();
        if (CurrentLevel == (m_AllowedLevels.Count - 1))
        {
            gameComplete = true;
            Debug.Log("Game Complete");
        }
        var passedLevel = m_SaveSerial.passedLevelsToSave[CurrentLevel];
        if (passedLevel == 0)
        {
            //TODO helper value need to add
            m_StickersCounter.UpdateLevelsStickers(3, currentLevel, m_HelperCallCount, time);
        }
        //passedLevel = 1;
        m_SaveSerial.passedLevelsToSave[CurrentLevel] = 1; // Mark level complete
        CurrentLevel++;
    }

    public void LevelAborted()
    {
        AnalyticsHelper.Instance.SendLevelAborted(3, currentLevel);
    }

    public void NextLevel()
    {
        CheckLevelCompletion(true, false);
    }

    public void RestartLevel()
    {
        CurrentLevel--;
        CheckLevelCompletion(true, true);
    }

    public void ExitButton()
    {
        // Transition to main screen
        SoundPlayer.instance.Play(exitButtonSound, 1);
        figures.SelectedID = -1;
    }

    public void CompleteScreenExitToMenuClick()
    {
        // Transition to main screen from complete screen
        SoundPlayer.instance.Play(exitButtonSound, 1);
        //m_LevelComplete.ExitToMenuClick();
        m_NextLevelController.ExitToMenu();
    }
    public void CheckLevelCompletion(bool isNextLevel, bool isRestart)
    {
        m_HelperCallCount = 0; // Set helper counter to zero on start level
        if (gameComplete)
        {
            gameComplete = false;
            if (!isRestart)
            {
                CurrentLevel = 0;
                if (!Purchaser.isPremium())
                {
                    PlayerPrefsHelper.SetBool("premium_opened", true);
                }
                SceneManager.LoadScene(2);
                return;
            }
        }

        if (CurrentLevel <= m_AllowedLevels.Count - 1)
        {
            AnalyticsHelper.Instance.SendLevelStarted(3, currentLevel);
            if (isNextLevel)
            {
                m_GameFieldCreation.NextLevel();
                m_NextLevelController.NextLevel();
                //SoundPlayer.instance.Play(nextButtonSound, 1);
                //TODO Here needs dispose destroy methods
                figures.EnableUsedFigures(false);
                //TODO Here needs set first figure on start level
            }
            else
            {
                m_GameFieldCreation.CreateField();
            }
        }
        else
        {
            CurrentLevel = 0;
            CompleteScreenExitToMenuClick();
            m_MainMenuControl.ExitButton();

        }
    }

    public int CurrentLevel
    {
        get
        {
            return currentLevel;
        }
        set
        {
            currentLevel = value;
            m_SaveSerial.currentLevelToSave = value;
            m_SaveSerial.SaveGame();
        }
    }
    /*
    public LevelData Levels
    {
        get => levels[currentLevel];
        set => levels[currentLevel] = value;
    }
    */

    public LevelData Levels
    {
        get => m_AllowedLevels[currentLevel];
        set => m_AllowedLevels[currentLevel] = value;
    }

    public LevelData[] LevelsArray => levels;

    public LevelData[] AllowedLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            //int passedLevel = m_SaveSerial.passedLevelsToSave[i];
            int passedLevel = m_SaveSerial.premiumLevelToSave[i];
            if (Purchaser.isPremium() | passedLevel == 0)
            {
                m_AllowedLevels.Add(levels[i]);
            }
        }
        return m_AllowedLevels.ToArray();
    }

    public int HelperCallCount
    {
        get => m_HelperCallCount;
        set => m_HelperCallCount = value;
    }
}