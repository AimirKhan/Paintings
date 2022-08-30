using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level_", menuName = "Create Level", order = 51)]
public class LevelData : ScriptableObject
{
    [SerializeField] private string levelName;
    [SerializeField] private Sprite flatPicture;
    [SerializeField] private Sprite _pixelPicture;
    [SerializeField] private int fieldWidthCount;
    [SerializeField] private int fieldHeightCount;
    [SerializeField] private int[] cellId;
    [SerializeField] private int[] usedFigures;
    [SerializeField] private int[] figuresGoalCount;
    [SerializeField] private int levelId;

    public string LevelName => levelName;

    public Sprite FlatPicture => flatPicture;

    public Sprite PixelPicture => _pixelPicture;

    public int FieldWidthCount => fieldWidthCount;

    public int FieldHeightCount => fieldHeightCount;

    public int[] CellId => cellId;

    public int[] UsedFigures => usedFigures;

    public int[] FiguresGoalCount => figuresGoalCount;

    public int LevelId => levelId;
}