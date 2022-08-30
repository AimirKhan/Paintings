using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private StickersCounter m_StickersCounter;
    [SerializeField] private Button m_ButtonPremium;
    [SerializeField] private CreateButtonImage m_CreateButtonImage;
    [SerializeField] private CreateColoringPage m_CreateColoringPage;
    [SerializeField] private SoundManager m_SoundManager;
    [SerializeField] private SaveManager m_SaveManager;
    [SerializeField] private Palette m_Palette;
    [SerializeField] private UI m_UI;
    [SerializeField] private ColoringPage[] m_ColoringPage;
    [SerializeField] int m_NumbersOfOpenLevels = 5;

    void Start()
    {
        if (Purchaser.isPremium())
        {
            m_NumbersOfOpenLevels = m_ColoringPage.Length;
        }

        m_CreateButtonImage.CreateButtons(GetComponent<GameManager>());

        m_CreateColoringPage.EventGameEnd.AddListener(EndGame);

        //AmplitudeAdapter.GetInstance().Initialization();
    }

    private void EndGame(ColoringPage page, SaveArray saveArray, bool isFirstEnd, int nubmUpdate)
    {
        m_SaveManager.SavePage(page.name, saveArray.Colors, saveArray.IsStickerReceived, saveArray.NumbsMaterial);

        m_UI.NumbUpdate = nubmUpdate;

        if (isFirstEnd)
        {
            UI.StopAnimationExitButton();
            m_UI.OnFinishGame();
        }
        else
        {
            m_UI.OnFinishGame();
        }
    }

    public CreateButtonImage CreateButtonImage => m_CreateButtonImage;

    public SoundManager SoundManager => m_SoundManager;

    public CreateColoringPage CreateColoringPage => m_CreateColoringPage;

    public SaveManager SaveManager => m_SaveManager;

    public UI UI => m_UI;

    public ColoringPage[] ColoringPage => m_ColoringPage;

    public Button ButtonPremium => m_ButtonPremium;

    public Palette Palette => m_Palette;

    public StickersCounter StickersCounter => m_StickersCounter;

    public int NumbersOfOpenLevels => m_NumbersOfOpenLevels;

    private void OnDestroy() => m_CreateColoringPage.EventGameEnd.RemoveListener(EndGame);
}
