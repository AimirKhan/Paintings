using UnityEngine;
using UnityEngine.SceneManagement;

public class Screen_manager : MonoBehaviour
{
    private Grid_controller m_grid;
    private ClickZone_controller m_clicker_zone;
    [SerializeField] private GameObject m_endScreen;
    [SerializeField] private GameObject m_premiumBt;
    [SerializeField] private GameObject m_settings;
    [SerializeField] private GameObject m_settingsBt;
    private Board_controller m_board;

    // state
    bool gridActive;
    bool gameActive;
    bool endSceneActive;

    void Start()
    {
        if (m_grid == null)
        {
            m_grid = FindObjectOfType<Grid_controller>();
            if (m_grid == null) Debug.LogError("Screen_manager: Grid_controller == null");
        }

        if (m_clicker_zone == null)
        {
            m_clicker_zone = FindObjectOfType<ClickZone_controller>();
            if (m_clicker_zone == null) Debug.LogError("Screen_manager: ClickZone_controller == null");
        }

        if (m_board == null)
        {
            m_board = FindObjectOfType<Board_controller>();
            if (m_board == null) Debug.LogError("Screen_manager: board == null");
        }

        UpdateState();
    }

    public void CloseGrid()
    {
        m_grid.gameObject.SetActive(false);
        m_settingsBt.SetActive(false);

        if (m_premiumBt != null)
        {
            m_premiumBt.SetActive(false);
        }
        m_settings.SetActive(false); 
    }
    public void OpenGrid()
    {
        m_grid.gameObject.SetActive(true);
        m_settingsBt.SetActive(true);

        if (m_premiumBt != null)
        {
            m_premiumBt.SetActive(true);
        }
        m_settings.SetActive(true);
    }

    public void CloseEndScreen()
    {
        m_endScreen.SetActive(false);
    }
    public void OpenEndScreen()
    {
        m_endScreen.SetActive(true);
    }

    public void HomeBtClick()
    {
        UpdateState();

        if (endSceneActive)
        {
            m_endScreen.gameObject.SetActive(false);
        }

        if (gameActive)
        {
            if (!endSceneActive)
            {
                m_grid.LevelAborted();
            }

            m_board.Back_close();
            OpenGrid();
            return;
        }

        if (gridActive)
        {
            SceneManager.LoadScene(1);
        }
    }

    private void UpdateState()
    {
        gridActive = m_grid.gameObject.activeSelf;
        gameActive = !m_board.backIsClose();
        endSceneActive = m_endScreen.activeSelf;
    }
}
