using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Figure", menuName = "Create Figure", order = 52)]
public class FiguresData : ScriptableObject
{
    [SerializeField] private string figureName;
    [SerializeField] private Sprite figureSprite;
    [SerializeField] private Sprite m_ColoredFigureSprite;
    [SerializeField] private Sprite m_MascotSprite;
    [SerializeField] private int figureId;

    public string FigureName => figureName;

    public Sprite FigureSprite => figureSprite;

    public Sprite ColoredFigureSprite => m_ColoredFigureSprite;

    public Sprite MascotSprite => m_MascotSprite;

    public int FigureId => figureId;
}
