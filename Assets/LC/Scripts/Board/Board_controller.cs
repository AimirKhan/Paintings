using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board_controller : MonoBehaviour
{
    [SerializeField] private float gameScale = 1;
    [SerializeField] private SpriteRenderer locked;
    [SerializeField] private SpriteRenderer unlocked;
    [SerializeField] private LineRenderer line;
    [SerializeField] private StarFinderHelper starFinderHelper;

    private Camera main_camera;
    private float cameraSize = -1;
    private float cameraGridSize = -1;
    private Background_controller background;
    private Animator animator;

    void Awake()
    {
        main_camera = FindObjectOfType<Camera>();
        cameraSize = main_camera.orthographicSize * 200;
        cameraGridSize = main_camera.orthographicSize * 2;

        if (background == null)
        {
            background = GetComponentInChildren<Background_controller>();
            if (background == null) Debug.LogError("Board_controller: No Background_controller");
        }

        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("Board_controller: No animator");
    }

    void Start()
    {
        Resize();
    }

    public SpriteRenderer GetLocked() { return locked; }

    public SpriteRenderer GetUnlocked()  { return unlocked; }

    public float getCameraSize() { return cameraSize; }

    public float getCameraGridSize() { return cameraGridSize; }

    public Vector2 getCameraResolution() { return new Vector2(main_camera.pixelWidth, main_camera.pixelHeight); }

    public float getGameScale() { return gameScale; }

    public void Unlock_picture(bool Unlock = true)
    {
        if (Unlock)
        {
            animator.Play("board_unlockType2");
            starFinderHelper.Stop();
        }
        else
        {
            animator.Play("board_idle");
        }

        //locked.gameObject.SetActive(!Unlock);
        //unlocked.gameObject.SetActive(Unlock);
    }

    public void Back_close()
    {
        starFinderHelper.Stop();
        background.Close();
    }

    public void Back_open(Background_prefab newBack)
    {
        background.Open(newBack);
    }
    //
    // Size fix
    //

    public void SetZero()
    {
        // TODO Установка дефолтных значений
    }

    public void Resize()
    {
        Resize(main_camera.pixelWidth, main_camera.pixelHeight);
    }

    public void Resize(float Width, float Height)
    {
        float scaleW;
        float scaleH;

        if (Width <= Height)
        {
            float ratio = Height / Width;
            Height = cameraSize * ratio;
            Width = cameraSize;
        }
        else
        {
            float ratio = Width / Height;
            Height = cameraSize;
            Width = cameraSize * ratio;
        }
        
        background.Resize(Width, Height, cameraGridSize);

        // locked
        scaleW = Width / (locked.sprite.bounds.size.x * locked.sprite.pixelsPerUnit);
        scaleH = Height / (locked.sprite.bounds.size.y * locked.sprite.pixelsPerUnit);

        locked.transform.localScale = new Vector3(scaleH, scaleW);

        if (scaleW <= scaleH) locked.transform.localScale = new Vector3(scaleW * gameScale, scaleW * gameScale, 1);
        else locked.transform.localScale = new Vector3(scaleH * gameScale, scaleH * gameScale, 1);

        // unlocked
        if (unlocked.sprite.bounds.size.x * unlocked.sprite.pixelsPerUnit == locked.sprite.bounds.size.x * locked.sprite.pixelsPerUnit &&
        unlocked.sprite.bounds.size.y * unlocked.sprite.pixelsPerUnit == locked.sprite.bounds.size.y * locked.sprite.pixelsPerUnit)
        {
            unlocked.transform.localScale = locked.transform.localScale;
        }
        else
        {
            Debug.LogWarning("Locked size (" + locked.sprite.bounds.size.x * locked.sprite.pixelsPerUnit + 
                "x" + locked.sprite.bounds.size.y * locked.sprite.pixelsPerUnit 
                + ") != Unlocked size (" + unlocked.sprite.bounds.size.x * unlocked.sprite.pixelsPerUnit + 
                "x" + unlocked.sprite.bounds.size.y * unlocked.sprite.pixelsPerUnit + ")");

            scaleW = Width / (unlocked.sprite.bounds.size.x * unlocked.sprite.pixelsPerUnit);
            scaleH = Height / (unlocked.sprite.bounds.size.y * unlocked.sprite.pixelsPerUnit);

            if (scaleW <= scaleH) unlocked.transform.localScale = new Vector3(scaleW * gameScale, scaleW * gameScale, 1);
            else unlocked.transform.localScale = new Vector3(scaleH * gameScale, scaleH * gameScale, 1);
        }
    }

    public bool backIsClose()
    {
        return background.isClose();
    }
}
