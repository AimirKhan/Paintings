using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelComplete : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_StarsAnim;
    [SerializeField] private LevelController m_LevelController;
    [SerializeField] private GameObject m_FlatPicture;
    [SerializeField] private Animator m_GameFieldCreation;
    private SpriteRenderer m_FlatPictureSprite;
    private Animator m_FlatPictureAnim;
    private bool m_IsFlatPicture;

    private void Awake()
    {
        m_FlatPictureSprite = m_FlatPicture.GetComponent<SpriteRenderer>();
        m_FlatPictureAnim = m_FlatPicture.GetComponent<Animator>();
        //GetComponent<Animator>();
        ShowFlatPicture();
    }
    public void OnEnable()
    {
        PlayCompleteActions();
    }
    public void PlayCompleteActions()
    {
        ShowFlatPicture();
        //m_LevelController.CurrentLevel++; // Move to NextLevelButtonScript
        m_FlatPictureAnim.Play("FlatPictureAnim");
        m_GameFieldCreation.Play("GameFieldCompleteAnim");
        //m_StarsAnim.Play();
        m_IsFlatPicture = true;
    }

    public void ExitToMenuClick()
    {
        if (m_IsFlatPicture == true)
        {
            m_FlatPictureAnim.Play("FlatPictureAnimOut");
            m_GameFieldCreation.Play("GameFieldCompleteAnimIn");
        }
    }

    public void RotatePixelFlatPicture()
    {
        if(m_IsFlatPicture == true)
        {
            m_FlatPictureAnim.Play("FlatPictureAnimOut");
            m_GameFieldCreation.Play("GameFieldCompleteAnimIn");
            m_IsFlatPicture = false;
        }
        else
        {
            m_FlatPictureAnim.Play("FlatPictureAnim");
            m_GameFieldCreation.Play("GameFieldCompleteAnim");
            m_IsFlatPicture = true;
        }
    }

    private void ShowFlatPicture()
    {
        if (m_LevelController.Levels.FlatPicture == null)
        {
            m_FlatPictureSprite.sprite = m_LevelController.Levels.PixelPicture;
        }
        else
        {
            m_FlatPictureSprite.sprite = m_LevelController.Levels.FlatPicture;
        }
    }
}
