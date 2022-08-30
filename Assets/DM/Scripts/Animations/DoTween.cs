using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoTween : MonoBehaviour
{
    [SerializeField] private Transform m_Transform;
    [SerializeField] private Vector2 m_StartScale;
    [SerializeField] private Vector2 m_EndScale;
    [SerializeField] private float m_Duration;
    [SerializeField] private Ease m_Ease;

    [SerializeField] private Vector2 m_EndFadeOutScale;
    [SerializeField] private Ease m_FadeOutEase;
    [SerializeField] private Color m_EndColor;
    [SerializeField] private float m_ColorDuration;
    [SerializeField] private Ease m_RecolorEase;

    public void PlayTransformAnim()
    {
        Tween scaleTween
            = DOTween.To(
                () => m_StartScale,
                scale => m_Transform.localScale = scale,
                m_EndScale,
                m_Duration)
            .SetEase(m_Ease);
        scaleTween.Play();
    }

    public void SelectedFigure(Transform transform)
    {
        Tween scaleTween
            = DOTween.To(
                () => m_StartScale,
                scale => transform.localScale = scale,
                m_EndScale,
                m_Duration)
            .SetEase(m_Ease);
        scaleTween.Play();
    }
    public void DeselectedFigure(Transform transform)
    {
        Tween scaleTween
            = DOTween.To(
                () => (Vector2)transform.localScale,
                scale => transform.localScale = scale,
                m_StartScale,
                m_Duration)
            .SetEase(m_Ease);
        scaleTween.Play();
    }
    public void FadeOutAnimation(Transform transform, Image image)
    {
        Tween scaleTween
            = DOTween.To(
                () => (Vector2)transform.localScale,
                scale => transform.localScale = scale,
                m_EndFadeOutScale,
                m_Duration)
            .SetEase(m_FadeOutEase);

        Tween recolorTween
            = DOTween.To(
                () => image.color,
                color => image.color = color,
                m_EndColor,
                m_ColorDuration)
            .SetEase(m_RecolorEase);

        Sequence fadeOutAnim = DOTween.Sequence();
        fadeOutAnim.Append(scaleTween).Join(recolorTween);
        fadeOutAnim.Play();
    }
}
