using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameFieldCreation : MonoBehaviour
{
    [Inject] LevelController m_LevelController;
    [Inject] Camera m_Camera;
    [SerializeField] private GameObject figure;
    public Transform fieldSpawnObj;
    [SerializeField] private Vector3 fieldPosShift;
    [SerializeField] private float startScale;
    [SerializeField] private FiguresData[] figureSprite;
    [SerializeField] private Figures m_Figures;
    public Figures Figures => m_Figures;

    private GameObject[,] virtualField;
    private GameObject parentGameObject;
    private Vector2 startPosition;
    private Vector2 tempPosition;
    public float m_cellSize = 1;
    public int m_ColumnSize;

    private List<MouseActions> m_CellsPos = new List<MouseActions>();
    public List<MouseActions> CellsPos => m_CellsPos;

    private bool[] m_activeCell;// Решённая или не решённая ячейки
    
    public void NextLevel()
    {
        Destroy(parentGameObject);
        CreateField();
    }

    public void CreateField()
    {
        m_CellsPos.Clear();
        LevelData levelData = m_LevelController.Levels;
        m_activeCell = new bool[m_LevelController.Levels.CellId.Length];
        m_ColumnSize = (int)Mathf.Sqrt(m_activeCell.Length);

        parentGameObject = new GameObject("ParentGameObject");
        parentGameObject.transform.parent = fieldSpawnObj;

        startPosition = new Vector2(m_cellSize * (m_ColumnSize - 1) / 2, m_cellSize * (m_ColumnSize - 1) / 2);
        virtualField = new GameObject[m_ColumnSize, m_ColumnSize];
        int m_cellCount = 0;

        for (int h = 0; h < m_ColumnSize; h++)
        {
            for (int w = 0; w < m_ColumnSize; w++)
            {
                tempPosition = startPosition - new Vector2(m_cellSize * w, m_cellSize * h);
                GameObject cell = virtualField[w, h];
                cell = Instantiate(figure, parentGameObject.transform);
                //cell = Instantiate(figure, fieldSpawnObj);
                cell.transform.position = tempPosition;
                int figureId = levelData.CellId[m_cellCount];
                cell.GetComponent<SpriteRenderer>().sprite
                    = figureSprite[figureId].FigureSprite;
                var mouseActions = cell.GetComponent<MouseActions>();
                mouseActions.Init(m_LevelController, m_Figures, levelData.CellId[m_cellCount],
                                  m_activeCell[m_cellCount]); // Init
                if (figureId != 0)
                {
                    m_CellsPos.Add(mouseActions);
                }
                m_cellCount++;
            }
        }
        fieldSpawnObj.position += fieldPosShift;
        Vector2 newScale = Vector2.one * (m_Camera.orthographicSize * 2f / levelData.FieldHeightCount);
        fieldSpawnObj.localScale = newScale * startScale;
    }
}
