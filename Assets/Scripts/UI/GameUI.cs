using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private Canvas _overlayCanvas;
    [SerializeField] private Canvas _worldCanvas;
    
    public Canvas OverlayCanvas => _overlayCanvas;
    public Canvas WorldCanvas => _worldCanvas;
    
    [SerializeField] private InGameUI _inGameUI;
    [SerializeField] private PauseUI _pauseUI;
    
    private void Start()
    {
        _pauseUI.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameEventInvoker.onPauseGame += OnPauseGame;
        GameEventInvoker.onUnpauseGame += OnUnpauseGame;
    }

    private void OnDisable()
    {
        GameEventInvoker.onPauseGame -= OnPauseGame;
        GameEventInvoker.onUnpauseGame -= OnUnpauseGame;
    }

    private void OnUnpauseGame()
    {
        _pauseUI.gameObject.SetActive(false);
    }

    private void OnPauseGame()
    {
        _pauseUI.gameObject.SetActive(true);
    }
}