using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    // [SerializeField] private int score; // Подсчёт общего числа очков
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
    /* Подсчёт общего числа очков
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
