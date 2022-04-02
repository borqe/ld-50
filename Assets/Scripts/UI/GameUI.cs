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
}
