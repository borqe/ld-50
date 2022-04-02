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
    [SerializeField] private StartUI _startUI;

    [Header("Popups")] 
    [SerializeField] private GameObject _popupPrefab;
    
    private void Start()
    {
        _pauseUI.gameObject.SetActive(false);
        _worldCanvas.worldCamera = Camera.main;
    }

    private void OnEnable()
    {
        GameEventInvoker.onPauseGame += OnPauseGame;
        GameEventInvoker.onUnpauseGame += OnUnpauseGame;
        GameEventInvoker.onEndGame += OnEndGame;
    }

    private void OnDisable()
    {
        GameEventInvoker.onPauseGame -= OnPauseGame;
        GameEventInvoker.onUnpauseGame -= OnUnpauseGame;
        GameEventInvoker.onEndGame -= OnEndGame;
    }

    private void OnEndGame()
    {
        _pauseUI.gameObject.SetActive(false);
        _startUI.gameObject.SetActive(true);
    }

    private void OnUnpauseGame()
    {
        _pauseUI.gameObject.SetActive(false);
    }

    private void OnPauseGame()
    {
        _pauseUI.gameObject.SetActive(true);
    }
    

    public void CreatePopup(Vector3 worldPosition)
    {
        var popup = Instantiate(_popupPrefab, _worldCanvas.transform);
        popup.transform.position = worldPosition;
    }
}