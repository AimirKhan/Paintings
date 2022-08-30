using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class Balloon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject m_PS;
    [SerializeField] private string m_ExplodeSound;
    private RectTransform m_RectTransform;
    Sequence m_Sequence;

    void Start()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_Sequence = DOTween.Sequence();
        m_Sequence.Append(m_RectTransform
            .DOMove(new Vector3(m_RectTransform.position.x, m_RectTransform.position.y * -1, m_RectTransform.position.z), Random.Range(4.0f, 8.0f))
            );
        m_Sequence.Play().OnComplete(DestroyBulloon);

        var canvas = gameObject.GetComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 111;
    }

    //public void OnClick()
    //{
    //    SoundPlayer.instance.Play(m_ExplodeSound, 1);
    //    gameObject.GetComponent<Image>().enabled = false;
    //    m_PS.SetActive(true);
    //    Destroy(gameObject, 5);
    //    m_Sequence.Kill();
    //}

    //public void OnPointerClick()
    //{
    //    SoundPlayer.instance.Play(m_ExplodeSound, 1);
    //    gameObject.GetComponent<Image>().enabled = false;
    //    m_PS.SetActive(true);
    //    Destroy(gameObject, 5);
    //    m_Sequence.Kill();
    //}

    

    private void DestroyBulloon()
    {
        Destroy(gameObject);
        m_Sequence.Kill();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SoundPlayer.instance.Play(m_ExplodeSound, 1);
        gameObject.GetComponent<Image>().enabled = false;
        m_PS.SetActive(true);
        Destroy(gameObject, 5);
        m_Sequence.Kill();
    }

    /*private void PlayParticles(Vector2 position)
    {
        //Color color;
        //Material material;
        //Material material = _palette.GetColor().Material;
        //Color color = _palette.GetColor().Color;
        m_PS.transform.position = position;
        var mainPS = m_PS.main;
        mainPS.startColor = color;
        m_PS.GetComponent<Renderer>().material = material;
        m_PS.Play();
        //Debug.Log($"Color = {color}");
    }*/

}
