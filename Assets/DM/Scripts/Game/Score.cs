using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    // [SerializeField] private int score; // ������� ������ ����� �����
    [SerializeField] private int[] figureScore;

    public int[] FigureScore
    {
        get
        {
            return figureScore;
        }
        set
        {
            figureScore = value;
        }
    }
    /* ������� ������ ����� �����
    public int CommonScore
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }
    */
}
