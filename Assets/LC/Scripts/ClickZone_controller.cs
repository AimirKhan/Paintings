using UnityEngine;
using UnityEngine.UI;

public class ClickZone_controller : MonoBehaviour
{
    private Board_controller board;

    private Button zone;

    CanvasScaler canvasScaler;

    void Awake()
    {
        if (board == null)
        {
            board = FindObjectOfType<Board_controller>();
            
            if (board == null) Debug.LogError("ClickZone_controller: Board_controller == null");
        }

        if (canvasScaler == null)
        {
            canvasScaler = FindObjectOfType<CanvasScaler>();
            
            if (canvasScaler == null) Debug.LogError("ClickZone_controller: CanvasScaler == null");
        }

        zone = GetComponentInChildren<Button>();
        SetActiveZone(false);
    }

    public void SetPosition(Vector3 newpos)
    {
        zone.transform.localPosition = newpos * canvasScaler.referenceResolution.y / board.getCameraGridSize();
    }

    public void SetActiveZone(bool active = true)
    {
        zone.gameObject.SetActive(active);
    }
}
