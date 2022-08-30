using System.Security.Cryptography.X509Certificates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_controller : MonoBehaviour
{
    [SerializeField] private Background_obj close;
    [SerializeField] private Background_obj open;
    [SerializeField] private Background_prefab zero_back_prefab;
    [SerializeField] private SpriteRenderer shadow;
    [SerializeField] private LineRenderer line;

    void Awake()
    {
        open.gameObject.SetActive(true);
        close.gameObject.SetActive(true);
        close.loadBack(zero_back_prefab);
    }

    public void Close(Background_prefab newBack)
    {
        close.loadBack(newBack);
        Close();
    }

    public void LoadZeroScreen()
    {
        close.loadBack(zero_back_prefab);
        Close();
    }

    public void Close()
    {
        close.gameObject.SetActive(true);
    }

    public void Open(Background_prefab newBack)
    {
        open.loadBack(newBack);
        shadow.sprite = newBack.GetShadow();
        line.material = newBack.GetLine();
        Open();
    }

    public void Open()
    {
        close.gameObject.SetActive(false);
    }

    public void Resize(float Width, float Height, float cameraGridSize)
    {
        close.Resize(Width, Height, cameraGridSize);
        open.Resize(Width, Height, cameraGridSize);
    }

    public bool isClose()
    {
        return close.gameObject.activeSelf;
    }
}
