using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }
    [SerializeField] private EmojiData EmojiData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            EmojiDatabase.Create(EmojiData);
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
    [SerializeField] private GameObject _popupEmoji;

    private void Start()
    {
        _worldCanvas.worldCamera = Camera.main;
    }    

    public PopupBase CreateTimerPopup(Vector3 worldPosition)
    {
        var popup = Instantiate(_popupPrefab, _worldCanvas.transform);
        popup.transform.position = worldPosition;
        return popup.GetComponent<PopupBase>();
    }

    public PopupEmoji CreateEmojiPopup(Vector3 worldPosition, EmojiType emojiType)
    {
        var popup = Instantiate(_popupEmoji, _worldCanvas.transform).GetComponent<PopupEmoji>();
        popup.transform.position = worldPosition;
        popup.Setup(emojiType);
        return popup;
    }
}