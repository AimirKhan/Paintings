using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuControl : MonoBehaviour
{
    [SerializeField] private string playButtonSound;
    [SerializeField] private string completeSound;
    [SerializeField] private string settingsButtonSound;
    [SerializeField] private string exitButtonSound;
    public enum enMenuType
    {
        GameScreen,
        CompleteScreen,
    }
    public enMenuType MenuType = enMenuType.GameScreen;
    [Space]
    public GameObject gameScreenUI;
    public GameObject gameScreenObject;
    [Space]
    public GameObject completeScreenUI;
    public GameObject completeScreenObject;

    void Refresh()
    {
        switch (MenuType)
        {
            case enMenuType.GameScreen:
                gameScreenUI.SetActive(true);
                gameScreenObject.SetActive(true);
                completeScreenUI.SetActive(false);
                completeScreenObject.SetActive(false);
                break;
            case enMenuType.CompleteScreen:
                // Enable settings window
                gameScreenUI.SetActive(false);
                gameScreenObject.SetActive(true);
                completeScreenUI.SetActive(true);
                completeScreenObject.SetActive(true);
                break;
            default:
                Debug.LogError("Такого состояния нет");
                break;
        }
    }

    // Move "Start" game to right place
    public void Start()
    {
        //MenuType = enMenuType.Normal;
        //Refresh();
        OnPlayButtonClick();
    }

    public void OnPlayButtonClick()
    {
        MenuType = enMenuType.GameScreen;
        Refresh();
        SoundPlayer.instance.Play(playButtonSound, 1);
    }

    public void ExitButton()
    {
        MenuType = enMenuType.GameScreen;
        Refresh();
        SoundPlayer.instance.Play(exitButtonSound, 1);
    }

    public void OnCompleteScreenClick()
    {
        MenuType = enMenuType.CompleteScreen;
        Refresh();
        SoundPlayer.instance.Play(completeSound, 1);
    }
}
