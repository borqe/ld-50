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

    [Header("Popups")] 
    [SerializeField] private GameObject _popupPrefab;
    
    private void Start()
    {
        _worldCanvas.worldCamera = Camera.main;
    }    

    public PopupBase CreatePopup(Vector3 worldPosition)
    {
        var popup = Instantiate(_popupPrefab, _worldCanvas.transform);
        popup.transform.position = worldPosition;
        return popup.GetComponent<PopupBase>();
    }
}