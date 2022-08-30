using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class debug_viewer : MonoBehaviour
{
    [SerializeField] Board_controller board;
    [SerializeField] Sprite[] img;
    [SerializeField] Background_prefab[] backs;
    [SerializeField] Level_prefab obj;
    [SerializeField] Text info;

    private int active_index = -1;
    private int active_back = 0;

    public void Next()
    {
        if (active_index + 1 >= img.Length)
            return;

        active_index++;
        LoadSprite(active_index);
    }

    public void Prev()
    {

        if (active_index - 1 < 0)
            return;

        active_index--;
        LoadSprite(active_index);
    }

    public void LoadSprite(int index)
    {
        info.text = "Level: " + (active_index + 1);
        obj.setLocked(img[index]);
        obj.StartLevel();
    }

    public void SwapBack()
    {
        if (active_back + 1 >= backs.Length)
            active_back = 0;
        else
            active_back++;

        board.Back_open(backs[active_back]);
        obj.setBack(backs[active_back]);
        obj.StartLevel();
    }
}
