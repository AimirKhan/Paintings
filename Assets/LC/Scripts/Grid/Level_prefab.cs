using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_prefab : MonoBehaviour
{
    [Header("Level")]
    [SerializeField] private int level_index = -1;
    [SerializeField] private TextAsset json;
    [SerializeField] private List<int> stars_index;

    [Header("Visual")]
    [SerializeField] private Sprite locked;
    [SerializeField] private Sprite unlocked;
    [SerializeField] private Background_prefab back;

    [Header("Grid and more")]
    [SerializeField] private Sprite gridPreviewLock;
    [SerializeField] private Sprite gridPreviewUnlock;
    [SerializeField] private Image grid_person;
    [SerializeField] public bool premium = false;
    [NonSerialized] public bool complete = false;

    private info lines = new info();

    private bool loaded = false;


    private static Picture_manager picture_Manager;
    private static Screen_manager screen_Manager;

    void Awake()
    {
        if (gridPreviewLock != null && gridPreviewUnlock != null)
        {
            if (complete)
                grid_person.sprite = gridPreviewUnlock;
            else
                grid_person.sprite = gridPreviewLock;
        }
        else
        {
            Debug.Log("Level " + level_index + ": without grid preview");
        }

        if (picture_Manager == null)
        {
            picture_Manager = FindObjectOfType<Picture_manager>();

            if (picture_Manager == null)
                Debug.LogError("Level_prefab: Picture_manager == null");
        }

        if (screen_Manager == null)
        {
            screen_Manager = FindObjectOfType<Screen_manager>();

            if (screen_Manager == null)
                Debug.LogError("Level_prefab: Screen_manager == null");
        }
    }

    public void UnlockGrid()
    {
        grid_person.sprite = gridPreviewUnlock;
    }

    public void StartLevel()
    {
        if (premium)
        {
            if (!Purchaser.isPremium())
            {
                picture_Manager.OpenLockedLevel();
                return;
            }
        }

        picture_Manager.LoadLevel(this);
        screen_Manager.CloseGrid();

    }

    public void Load()
    {
        if (unlocked.bounds.size.x != locked.bounds.size.x)
            Debug.LogWarning("Unlocked.height: " + Convert.ToString(unlocked.bounds.size.x * unlocked.pixelsPerUnit) +
            " != Locked.height: " + Convert.ToString(locked.bounds.size.x * locked.pixelsPerUnit));

        if (unlocked.bounds.size.y != locked.bounds.size.y)
            Debug.LogWarning("Unlocked.width: " + Convert.ToString(unlocked.bounds.size.x * unlocked.pixelsPerUnit) +
            " != Locked.width: " + Convert.ToString(locked.bounds.size.y * locked.pixelsPerUnit));


        lines = JsonUtility.FromJson<info>(json.text);
        loaded = true;
    }

    public void Clear()
    {
        lines.dots.Clear();
        loaded = false;
    }

    public Vector3 getLine(int i, float scale = 0.01f)
    {
        return new Vector3(
            ( lines.dots[lines.dots.Count - i - 1].x - (locked.bounds.size.x * locked.pixelsPerUnit) / 2) * scale,
            (-lines.dots[lines.dots.Count - i - 1].y + (locked.bounds.size.y * locked.pixelsPerUnit) / 2) * scale,
            0);
    }

    public int GetSize()
    {
        return lines.dots.Count;
    }

    public Vector2 GetResolution()
    {
        return new Vector2(locked.bounds.size.x * locked.pixelsPerUnit,
                           locked.bounds.size.y * locked.pixelsPerUnit);
    }

    public Sprite GetLocked() { return locked; }
    public Sprite GetUnlocked() { return unlocked; }

    public Background_prefab GetBackground()
    {
        //if (back == null) Debug.LogError("No background!");
        return back;
    }

    public void SetLevelId(int info) { level_index = info; }
    public int GetLevelId() { return level_index; }

    public int GetLevelIndex() { return level_index; }

    public bool isLoaded() { return loaded; }

    public Vector2 GetJsonSize() { return new Vector2(lines.Width, lines.Height); }

    public List<int> GetStarList()
    {
        List<int> tmp = stars_index;
        tmp.Sort();
        // TODO: проверка на повторы
        if (tmp.Count == 0)
            Debug.LogError("Star list is empty!");

        if (tmp[tmp.Count - 1] >= lines.dots.Count)
            Debug.LogError("Picture_prefab. Out of range: " + Convert.ToString(tmp[tmp.Count - 1]) + "/" + Convert.ToString(lines.dots.Count));


        if (tmp.Count <= 4)
        {
            tmp = new List<int>();
            var rand = new System.Random();
            int dots_count = lines.dots.Count;

            int Max = rand.Next(10, 20);
            Debug.Log("Create random stars. Count: " + Max);
            for (int i = 0; i < Max; i++)
            {
                tmp.Add(dots_count / Max * i);
            }
        }

        return tmp;
    }

    private class info
    {
        public info() { }
        public int Height;
        public int Width;
        public List<Vector2> dots = new List<Vector2>();
    }

    private void ExampleJson()
    {
        info tmp = new info();
        for (int i = 0; i < 5; i++)
            tmp.dots.Add(new Vector2(i, i));

        tmp.Height = 5;
        tmp.Width = 10;
        Debug.Log(JsonUtility.ToJson(tmp));
    }

    public void DebugGetJson()
    {
        Debug.Log(JsonUtility.ToJson(this));
    }

    // Debug
    public void setLocked(Sprite newSprite)
    {
        locked = newSprite;
    }

    public void setBack(Background_prefab newBack)
    {
        back = newBack; 
    }
}

