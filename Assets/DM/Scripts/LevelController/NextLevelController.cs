using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class NextLevelController : MonoBehaviour
{
    [Inject] LevelController m_LevelController;
    [SerializeField] private GameFieldCreation m_GameFieldCreation;
    [SerializeField] private Animator m_GameFieldAnim;

    public void NextLevel()
    {
        // Go to next level
        m_GameFieldAnim.Play("EmptyState");
        ResetTransform();
        m_GameFieldCreation.NextLevel();
    }

    public void ExitToMenu()
    {
        // Go to main menu
        m_GameFieldAnim.Play("EmptyState");
        m_GameFieldCreation.fieldSpawnObj.localEulerAngles = Vector3.zero;
    }

    public void ResetTransform()
    {
        m_GameFieldCreation.fieldSpawnObj.localPosition = Vector3.zero;
        m_GameFieldCreation.fieldSpawnObj.localEulerAngles = Vector3.zero;
        m_GameFieldCreation.fieldSpawnObj.localScale = Vector3.one;
    }
}
